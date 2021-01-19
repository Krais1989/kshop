using FluentValidation;
using KShop.Products.Domain.Exceptions;
using KShop.Products.Domain.Validators;
using KShop.Products.Persistence;
using KShop.Products.Persistence.Entities;
using KShop.Communications.Contracts.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Products.Domain.Mediators
{
    public class ProductsReserveMediatorResponse
    {
    }
    public class ProductsReserveMediatorRequest : IRequest<ProductsReserveMediatorResponse>
    {
        public Guid OrderID { get; set; }
        public IDictionary<int, int> Positions { get; set; }
    }
    public class ProductsReserveMediatorHandler : IRequestHandler<ProductsReserveMediatorRequest, ProductsReserveMediatorResponse>
    {
        private readonly ILogger<ProductsReserveMediatorHandler> _logger;
        private readonly IValidator<ProductsReserveFluentValidatorDto> _validator;
        private readonly ProductsContext _catalogueContext;

        public ProductsReserveMediatorHandler(
            ILogger<ProductsReserveMediatorHandler> logger,
            IValidator<ProductsReserveFluentValidatorDto> validator,
            ProductsContext catalogueContext)
        {
            _logger = logger;
            _validator = validator;
            _catalogueContext = catalogueContext;
        }

        public async Task<ProductsReserveMediatorResponse> Handle(ProductsReserveMediatorRequest request, CancellationToken cancellationToken)
        {
            /* NOTE: Возможно стоит вынести резервирование продуктов заказа в отдельный механизм */

            /* TODO: применить  изоляции к ProductReserves, чтобы избежать конфликтов */

            var validatorDto = new ProductsReserveFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var orderProdIds = request.Positions.Select(pos => pos.Key).ToList();
            var stockProdPositions = await _catalogueContext.ProductPositions.Where(e => orderProdIds.Contains(e.ProductID)).ToListAsync();

            /* Группировка всех позиций по продукту с расчёт общего количества */
            var availableProductsGrouped = stockProdPositions
                .GroupBy(e => e.ProductID, (prodId, entities) => new { ProductID = prodId, Quantity = entities.Sum(e => e.Quantity) })
                .ToDictionary(e => e.ProductID, e => e.Quantity);

            /* Проверка наличия резервируемого товара в нужном количестве */
            foreach (var orderPos in request.Positions)
            {
                if (!availableProductsGrouped.ContainsKey(orderPos.Key) || availableProductsGrouped[orderPos.Key] < orderPos.Value)
                {
                    throw new OrderReserveException(request.OrderID);
                }
            }

            /* Снятие позиций товара согласно резервированию заказа */
            foreach (var orderPos in request.Positions)
            {
            }

            var reserves = request.Positions.Select(e =>
                new ProductReserve()
                {
                    OrderID = request.OrderID,
                    ProductID = e.Key,
                    Quantity = e.Value,
                    ReserveDate = DateTime.UtcNow,
                    Status = ProductReserve.EStatus.Reserved
                });

            var tokenSrc = new CancellationTokenSource();
            var cancToken = tokenSrc.Token;

            await _catalogueContext.ProductReserves.AddRangeAsync(reserves, cancToken);
            await _catalogueContext.SaveChangesAsync(cancToken);

            return new ProductsReserveMediatorResponse() { };
        }
    }
}
