﻿using FluentValidation;
using KShop.Carts.Persistence;
using KShop.Shared.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Carts.Domain.Mediators
{
    public class RemoveCartPositionMediatorResponse : BaseResponse
    {
    }
    public class RemoveCartPositionMediatorRequest : IRequest<RemoveCartPositionMediatorResponse>
    {
        public RemoveCartPositionMediatorRequest(uint userID, List<uint> productsIDs)
        {
            UserID = userID;
            ProductsIDs = productsIDs;
        }

        public uint UserID { get; private set; }
        public List<uint> ProductsIDs { get; private set; }
    }
    public class RemoveCartPositionMediatorHandler : IRequestHandler<RemoveCartPositionMediatorRequest, RemoveCartPositionMediatorResponse>
    {
        private readonly ILogger<RemoveCartPositionMediatorHandler> _logger;
        private readonly ICartKVRepository _cartsRepo;

        public RemoveCartPositionMediatorHandler(ILogger<RemoveCartPositionMediatorHandler> logger, ICartKVRepository cartsRepo)
        {
            _logger = logger;
            _cartsRepo = cartsRepo;
        }

        //private readonly IValidator<RemoveCartPositionMediatorFluentValidatorDto> _validator;

        public async Task<RemoveCartPositionMediatorResponse> Handle(RemoveCartPositionMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new RemoveCartPositionMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            var cartId = $"cart-{request.UserID}";
            var cart = await _cartsRepo.GetAsync(cartId);
            cart.RemoveRange(request.ProductsIDs);
            await _cartsRepo.ReplaceAsync(cartId, cart);

            return new RemoveCartPositionMediatorResponse();
        }
    }
}
