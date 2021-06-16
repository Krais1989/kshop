﻿using FluentValidation;
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

namespace KShop.Products.Domain
{

    public class ProductsReserveMediatorResponse
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
        public string ErrorMessage { get; private set; }
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }
    /// <summary>
    /// Запрос на резервацию заказа
    /// </summary>
    public class ProductsReserveMediatorRequest : IRequest<ProductsReserveMediatorResponse>
    {
        public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }
        public OrderPositionsMap OrderPositions { get; set; }
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

            var reserves = request.OrderPositions.Keys.Select(
                prodId => new ProductReserve()
                {
                    OrderID = request.OrderID,
                    ProductID = prodId,
                    Quantity = request.OrderPositions[prodId],
                    CreateDate = DateTime.UtcNow,
                    Status = ProductReserve.EStatus.Pending,
                    CustomerID = request.CustomerID
                }).ToList();

            await _context.AddRangeAsync(reserves);
            await _context.SaveChangesAsync(cancellationToken);

            var reservationData = new ProductsReserveMap(reserves.ToDictionary(e => e.ProductID, e => e.ID));

            return new ProductsReserveMediatorResponse(reservationData);
        }
    }
}
