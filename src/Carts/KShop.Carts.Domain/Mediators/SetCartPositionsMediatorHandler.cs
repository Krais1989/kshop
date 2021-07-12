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

    public class SetCartPositionsMediatorResponse : BaseResponse
    {
    }
    public class SetCartPositionsMediatorRequest : IRequest<SetCartPositionsMediatorResponse>
    {
        public SetCartPositionsMediatorRequest(uint userID, List<CartPosition> positions, bool useMerge)
        {
            UserID = userID;
            Positions = positions;
            UseMerge = useMerge;
        }

        public uint UserID { get; private set; }
        public List<CartPosition> Positions { get; private set; }
        public bool UseMerge { get; private set; }
    }
    public class SetCartPositionsMediatorHandler : IRequestHandler<SetCartPositionsMediatorRequest, SetCartPositionsMediatorResponse>
    {
        private readonly ILogger<SetCartPositionsMediatorHandler> _logger;
        private readonly ICartKVRepository _cartsRepo;

        public SetCartPositionsMediatorHandler(ILogger<SetCartPositionsMediatorHandler> logger, ICartKVRepository cartsRepo)
        {
            _logger = logger;
            _cartsRepo = cartsRepo;
        }

        //private readonly IValidator<SetCartPositionsMediatorFluentValidatorDto> _validator;

        public async Task<SetCartPositionsMediatorResponse> Handle(SetCartPositionsMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new SetCartPositionsMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            var cartId = $"cart-{request.UserID}";
            var cart = await _cartsRepo.GetAsync(cartId);

            if (request.UseMerge)
                cart.MergeRange(request.Positions);
            else
                cart.SetRange(request.Positions);

            await _cartsRepo.ReplaceAsync(cartId, cart);
            return new SetCartPositionsMediatorResponse();
        }
    }
}
