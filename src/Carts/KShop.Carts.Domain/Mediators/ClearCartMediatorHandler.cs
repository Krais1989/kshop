using FluentValidation;
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

    public class ClearCartMediatorResponse : BaseResponse
    {
    }
    public class ClearCartMediatorRequest : IRequest<ClearCartMediatorResponse>
    {
        public ClearCartMediatorRequest(uint userID)
        {
            UserID = userID;
        }

        public uint UserID { get; private set; }
    }
    public class ClearCartMediatorHandler : IRequestHandler<ClearCartMediatorRequest, ClearCartMediatorResponse>
    {
        private readonly ILogger<ClearCartMediatorHandler> _logger;
        private readonly ICartKVRepository _cartsRepo;
        //private readonly IValidator<ClearCartMediatorFluentValidatorDto> _validator;

        public ClearCartMediatorHandler(ILogger<ClearCartMediatorHandler> logger, ICartKVRepository cartsRepo)
        {
            _logger = logger;
            _cartsRepo = cartsRepo;
        }

        public async Task<ClearCartMediatorResponse> Handle(ClearCartMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new ClearCartMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            var cartId = $"cart-{request.UserID}";
            await _cartsRepo.DeleteAsync(cartId);

            return new ClearCartMediatorResponse();
        }
    }
}
