using FluentValidation;
using KShop.Catalogues.Domain.Exceptions;
using KShop.Catalogues.Domain.Validators;
using KShop.Catalogues.Persistence;
using KShop.Catalogues.Persistence.Entities;
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

namespace KShop.Catalogues.Domain.Mediators
{
    public class OrderReserveMediatorResponse
    {

    }
    public class OrderReserveMediatorRequest : IRequest<OrderReserveMediatorResponse>
    {
        public Guid OrderID { get; set; }
        public IEnumerable<ProductStack> Positions { get; set; }
    }
    public class OrderReserveMediatorHandler : IRequestHandler<OrderReserveMediatorRequest, OrderReserveMediatorResponse>
    {
        private readonly ILogger<OrderReserveMediatorHandler> _logger;
        private readonly IValidator<OrderReserveFluentValidatorDto> _validator;
        private readonly CatalogueContext _catalogueContext;

        public OrderReserveMediatorHandler(
            ILogger<OrderReserveMediatorHandler> logger,
            IValidator<OrderReserveFluentValidatorDto> validator,
            CatalogueContext catalogueContext)
        {
            _logger = logger;
            _validator = validator;
            _catalogueContext = catalogueContext;
        }

        public async Task<OrderReserveMediatorResponse> Handle(OrderReserveMediatorRequest request, CancellationToken cancellationToken)
        {
            /* NOTE: Возможно стоит вынести резервирование продуктов заказа в отдельный механизм */

            /* TODO: применить  изоляции к ProductReserves, чтобы избежать конфликтов */

            var validatorDto = new OrderReserveFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var orderProdIds = request.Positions.Select(pos => pos.ProductID).ToList();
            var stockProdPositions = await _catalogueContext.ProductPositions.Where(e => orderProdIds.Contains(e.ProductID)).ToListAsync();

            /* Группировка всех позиций по продукту с расчёт общего количества */
            var availableProductsGrouped = stockProdPositions
                .GroupBy(e => e.ProductID, (prodId, entities) => new { ProductID = prodId, Quantity = entities.Sum(e => e.Quantity) })
                .ToDictionary(e => e.ProductID, e => e.Quantity);

            /* Проверка наличия резервируемого товара в нужном количестве */
            foreach (var orderPos in request.Positions)
            {
                if (!availableProductsGrouped.ContainsKey(orderPos.ProductID) || availableProductsGrouped[orderPos.ProductID] < orderPos.Quantity)
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
                    ProductID = e.ProductID,
                    Quantity = e.Quantity,
                    ReserveDate = DateTime.UtcNow,
                    Status = ProductReserve.EStatus.Reserved
                });

            var tokenSrc = new CancellationTokenSource();
            var cancToken = tokenSrc.Token;

            await _catalogueContext.ProductReserves.AddRangeAsync(reserves, cancToken);
            await _catalogueContext.SaveChangesAsync(cancToken);

            return new OrderReserveMediatorResponse();
        }
    }
}
