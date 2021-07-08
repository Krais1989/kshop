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

namespace KShop.Products.Domain
{

    public class ProductsGetForOrderMediatorResponse
    {
        public List<ProductPresentation> Data { get; set; } = new List<ProductPresentation>();
    }
    public class ProductsGetForOrderMediatorRequest : IRequest<ProductsGetForOrderMediatorResponse>
    {
        public uint[] ProductsIDs { get; set; }
    }
    public class ProductsGetForOrderMediatorHandler : IRequestHandler<ProductsGetForOrderMediatorRequest, ProductsGetForOrderMediatorResponse>
    {
        private readonly ProductsContext _productsContext;

        public ProductsGetForOrderMediatorHandler(ProductsContext productsContext)
        {
            _productsContext = productsContext;
        }

        //private readonly ILogger<GetProductsForOrderMediatorHandler> _logger;
        //private readonly IValidator<GetProductsForOrderMediatorFluentValidatorDto> _validator;

        public async Task<ProductsGetForOrderMediatorResponse> Handle(ProductsGetForOrderMediatorRequest request, CancellationToken cancellationToken)
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
                .Where(e => request.ProductsIDs.Contains(e.ID))
                .OrderByDescending(e => e.ID)
                .ToListAsync();

            return new ProductsGetForOrderMediatorResponse() { Data = data };
        }
    }
}
