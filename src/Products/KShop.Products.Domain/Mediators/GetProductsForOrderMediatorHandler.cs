using FluentValidation;
using KShop.Products.Persistence;
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

    public class GetProductsForOrderMediatorResponse
    {
        public List<ProductPresentation> Data { get; set; } = new List<ProductPresentation>();
    }
    public class GetProductsForOrderMediatorRequest : IRequest<GetProductsForOrderMediatorResponse>
    {
        public uint[] ProductIDs { get; set; }
    }
    public class GetProductsForOrderMediatorHandler : IRequestHandler<GetProductsForOrderMediatorRequest, GetProductsForOrderMediatorResponse>
    {
        private readonly ProductsContext _productsContext;

        public GetProductsForOrderMediatorHandler(ProductsContext productsContext)
        {
            _productsContext = productsContext;
        }

        //private readonly ILogger<GetProductsForOrderMediatorHandler> _logger;
        //private readonly IValidator<GetProductsForOrderMediatorFluentValidatorDto> _validator;

        public async Task<GetProductsForOrderMediatorResponse> Handle(GetProductsForOrderMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new GetProductsForOrderMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            var data = await _productsContext.Products.AsNoTracking()
                .Include(e => e.Price)
                .Select(e => new ProductPresentation
                {
                    ID = e.ID,
                    Title = e.Title,
                    Description = e.Description,
                    CategoryID = e.CategoryID,
                    Price = e.Price,
                    Image = e.Image
                })
                .Where(e => request.ProductIDs.Contains(e.ID))
                .OrderByDescending(e => e.ID)
                .ToListAsync();

            return new GetProductsForOrderMediatorResponse() { Data = data };
        }
    }
}
