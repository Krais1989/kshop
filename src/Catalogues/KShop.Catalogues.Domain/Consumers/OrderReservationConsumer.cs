using KShop.Catalogues.Domain.Exceptions;
using KShop.Catalogues.Persistence;
using KShop.Catalogues.Persistence.Entities;
using KShop.Communications.Contracts.Orders;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Catalogues.Domain.Consumers
{

    public class OrderReservationConsumer : IConsumer<IOrderReserveEvent>
    {
        private readonly ILogger<OrderReservationConsumer> _logger;
        private readonly CatalogueContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderReservationConsumer(ILogger<OrderReservationConsumer> logger, IPublishEndpoint publishEndpoint, CatalogueContext dbContext)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IOrderReserveEvent> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");

            //await _publishEndpoint.Publish<Fault<IOrderReserveEvent>>(context.Message);
            //throw new Exception("FAULT TEST");
            //return;

            try
            {
                /* Подумать насчёт создания фонового процесса резервации товаров */
                /* Это позволит создать единую точку резервации избежав проблем конкюренси */
                /* Либо лочить записи БД */

                var msg = context.Message;

                var orderProdIds = msg.Positions.Select(pos => pos.ProductID).ToList();
                var stockProdPositions = await _dbContext.ProductPositions.Where(e => orderProdIds.Contains(e.ProductID)).ToListAsync();

                /* Группировка всех позиций по продукту с расчёт общего количества */
                var availableProductsGrouped = stockProdPositions
                    .GroupBy(e => e.ProductID, (prodId, entities) => new { ProductID = prodId, Quantity = entities.Sum(e => e.Quantity) })
                    .ToDictionary(e => e.ProductID, e => e.Quantity);

                /* Проверка наличия резервируемого товара в нужном количестве */
                foreach (var orderPos in msg.Positions)
                {
                    if (!availableProductsGrouped.ContainsKey(orderPos.ProductID) || availableProductsGrouped[orderPos.ProductID] < orderPos.Quantity)
                    {
                        throw new OrderReserveException(msg.OrderID);
                    }
                }

                /* Снятие позиций товара согласно резервированию заказа */
                foreach (var orderPos in msg.Positions)
                {
                }

                var reserves = msg.Positions.Select(e =>
                    new ProductReserve()
                    {
                        ProductID = e.ProductID,
                        Quantity = e.Quantity,
                        ReserveDate = DateTime.UtcNow,
                        Status = ProductReserve.EStatus.Reserving
                    });

                var tokenSrc = new CancellationTokenSource();
                var cancToken = tokenSrc.Token;

                await _dbContext.ProductReserves.AddRangeAsync(reserves, cancToken);
                await _dbContext.SaveChangesAsync(cancToken);

                await _publishEndpoint.Publish<IOrderReserveSuccessEvent>(new { OrderID = context.Message.OrderID });
            }
            catch (Exception e)
            {
                await _publishEndpoint.Publish<IOrderReserveFailureEvent>(new { OrderID = context.Message.OrderID });
                throw;
            }


        }
    }
}
