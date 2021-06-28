using FluentValidation;
using KShop.Carts.Persistence;
using KShop.Shared.Domain.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Carts.Domain.Mediators
{
    public class GetCurrentCartMediatorResponse : BaseResponse
    {
        public Cart Data { get; set; }
    }
    public class GetCurrentCartMediatorRequest : IRequest<GetCurrentCartMediatorResponse>
    {
        public uint UserID { get; set; }
    }
    public class GetCurrentCartMediatorHandler : IRequestHandler<GetCurrentCartMediatorRequest, GetCurrentCartMediatorResponse>
    {
        private readonly ILogger<GetCurrentCartMediatorHandler> _logger;
        private readonly ICartKVRepository _cartsRepo;

        public GetCurrentCartMediatorHandler(ILogger<GetCurrentCartMediatorHandler> logger, ICartKVRepository cartsRepo)
        {
            _logger = logger;
            _cartsRepo = cartsRepo;
        }

        //private readonly IValidator<GetCurrentCartMediatorFluentValidatorDto> _validator;

        public async Task<GetCurrentCartMediatorResponse> Handle(GetCurrentCartMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new GetCurrentCartMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            var cartId = $"cart-{request.UserID}";
            var cart = await _cartsRepo.GetAsync(cartId);
            return new GetCurrentCartMediatorResponse() { Data = cart };
        }
    }
}
