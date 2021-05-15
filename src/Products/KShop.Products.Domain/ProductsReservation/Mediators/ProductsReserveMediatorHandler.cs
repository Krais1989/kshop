﻿using FluentValidation;
using KShop.Products.Domain.ProductsReservation.Validators;
using KShop.Products.Persistence;
using KShop.Products.Persistence.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Products.Domain.ProductsReservation.Mediators
{

    public class ProductsReserveMediatorResponse
    {
    }
    /// <summary>
    /// Запрос на резервацию заказа
    /// </summary>
    public class ProductsReserveMediatorRequest : IRequest<ProductsReserveMediatorResponse>
    {
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public IDictionary<int, int> OrderPositions { get; set; }
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
                    ReserveDate = DateTime.UtcNow,
                    Status = ProductReserve.EStatus.Pending
                });

            await _context.ProductReserves.AddRangeAsync(reserves);

            return new ProductsReserveMediatorResponse();
        }
    }
}