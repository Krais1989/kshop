using KShop.Communications.Contracts.Products;
using KShop.Products.Persistence;
using KShop.Products.Persistence.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Products.Domain.ProductsReservation.BackgroundServices
{
    /// <summary>
    /// Класс-обёртка для организации надежных транзакций. При ошибке операции выполняют retry согласно стратегии.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResilientTransaction<T>
        where T : DbContext
    {
        private T _context;
        private ResilientTransaction(T context) => _context = context ?? throw new ArgumentNullException(nameof(context));

        public static ResilientTransaction<T> New(T context) => new ResilientTransaction<T>(context);

        public async Task ExecuteAsync(Func<Task> action)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                await action();
                await transaction.CommitAsync();
            });
        }
    }

    /// <summary>
    /// Фоновый сервис для резервации заказов
    /// </summary>
    public class ProductsReservationBackgroundService : BackgroundService
    {
        private readonly ILogger<ProductsReservationBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProductsReservationBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<ProductsReservationBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            while (!stoppingToken.IsCancellationRequested)
            {
                var db_context = scope.ServiceProvider.GetRequiredService<ProductsContext>();
                var publish_endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                var bus = scope.ServiceProvider.GetRequiredService<IBusControl>();

                Dictionary<Guid, List<ProductReserve>> reserving = null;

                await ResilientTransaction<ProductsContext>
                    .New(db_context)
                    .ExecuteAsync(async () =>
                    {
                        var reserve_pendings = await db_context.ProductReserves.AsNoTracking()
                            .Where(e => e.Status == ProductReserve.EStatus.Pending)
                            .GroupBy(e => e.OrderID)
                            .ToListAsync();

                        reserving = reserve_pendings.ToDictionary(e => e.Key, e => e.ToList());
                    });


                /* Политика резервирвания, варианты: */
                /* 1. Если не удаётся зарезервировать хотя бы одну позицию - отмена всей резервации */
                /* 2. Зарезервировать доступные позиции, уведомить о невозможности резервации остальных */
                /* 3. Дозаказать нехватающие позиции, разбить доставку на несколько частей с разной датой */

                /* Выбранная политика: резервировать все что получится */

                /* Оптимизировать: предварительно загружать доступные позиции товаров */

                /*  */
                foreach (var orderId in reserving.Keys)
                {
                    var res_positions = reserving[orderId];
                    var reserved_products = new Dictionary<int, int>();
                    foreach (var res_pos in res_positions)
                    {
                        await ResilientTransaction<ProductsContext>
                            .New(db_context)
                            .ExecuteAsync(async () => {
                                var db_positions = await db_context.ProductPositions.FirstOrDefaultAsync(e => e.ProductID == res_pos.ProductID);
                                if (db_positions.Quantity >= res_pos.Quantity)
                                {
                                    db_positions.Quantity -= res_pos.Quantity;
                                    res_pos.Status = ProductReserve.EStatus.Reserved;
                                    res_pos.ReserveDate = DateTime.UtcNow;

                                    reserved_products.Add(res_pos.ProductID, res_pos.Quantity);
                                }
                                else
                                {
                                    res_pos.Status = ProductReserve.EStatus.NotEnough;
                                    res_pos.ReserveDate = DateTime.UtcNow;
                                }

                                db_context.ProductPositions.Update(db_positions);
                                db_context.ProductReserves.Update(res_pos);
                                await db_context.SaveChangesAsync();
                            });
                    }

                    /* Отправить сообщение о резервации продуктов */
                    await publish_endpoint.Publish(
                        new ProductsReserveBSEvent
                        {
                            OrderID = orderId,
                            ReservedProducts = reserved_products
                        });
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }



        }
    }
}
