using FluentValidation;
using KShop.Products.Persistence;

using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using Microsoft.EntityFrameworkCore;

namespace KShop.Products.Domain
{

    public class ProductsReserveMediatorResponse : BaseResponse
    {
        public ProductsReserveMediatorResponse(ProductsReserveMap reservationData)
        {
            ReservationData = reservationData;
        }

        public ProductsReserveMediatorResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Данные в формате <id_продукта, id_резерва>
        /// </summary>
        public ProductsReserveMap ReservationData { get; private set; }
        public Money OrderPrice { get; set; }
    }
    /// <summary>
    /// Запрос на резервацию заказа
    /// </summary>
    public class ProductsReserveMediatorRequest : IRequest<ProductsReserveMediatorResponse>
    {
        public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }
        public List<ProductQuantity> OrderContent { get; set; }
    }
    public class ProductsReserveMediatorHandler : IRequestHandler<ProductsReserveMediatorRequest, ProductsReserveMediatorResponse>
    {
        private readonly ILogger<ProductsReserveMediatorHandler> _logger;
        private readonly IValidator<ProductsReserveFluentValidatorDto> _validator;

        private readonly ProductsContext _context;

        public ProductsReserveMediatorHandler(
            ILogger<ProductsReserveMediatorHandler> logger,
            IValidator<ProductsReserveFluentValidatorDto> validator,
            ProductsContext context)
        {
            _logger = logger;
            _validator = validator;
            _context = context;
        }

        public async Task<ProductsReserveMediatorResponse> Handle(ProductsReserveMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new ProductsReserveFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var reserves = request.OrderContent.Select(
                e => new ProductReserve()
                {
                    OrderID = request.OrderID,
                    ProductID = e.ProductID,
                    Quantity = e.Quantity,
                    CreateDate = DateTime.UtcNow,
                    Status = ProductReserve.EStatus.Pending,
                    CustomerID = request.CustomerID
                }).ToList();

            await _context.AddRangeAsync(reserves);
            await _context.SaveChangesAsync(cancellationToken);


            var order_content_map = reserves.ToDictionary(e => e.ProductID, e => e.Quantity);
            var product_price_map = (await _context.Products.Where(e => order_content_map.Keys.Contains(e.ID))
                .Select(e => new { e.ID, e.Price, e.Discount })
                .ToListAsync()).ToDictionary(e => e.ID, e => e.Price * e.Discount / 100m);

            //var order_price = order_content_map.Aggregate(
            //    new Money(0, product_price_map.Values.First().Currency),
            //    (acc, cur) => acc + (cur.Value * product_price_map[cur.Key].Price));

            var reservationData = new ProductsReserveMap(reserves.ToDictionary(e => e.ProductID, e => e.ID));
            return new ProductsReserveMediatorResponse(reservationData);
        }
    }
}
