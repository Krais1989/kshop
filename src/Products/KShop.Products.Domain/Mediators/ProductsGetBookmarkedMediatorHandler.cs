using FluentValidation;
using KShop.Products.Persistence;
using KShop.Shared.Domain.Contracts;
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

    public class ProductsGetBookmarkedMediatorResponse : BaseResponse
    {
        public List<ProductPresentation> Data { get; set; }
    }
    public class ProductsGetBookmarkedMediatorRequest : IRequest<ProductsGetBookmarkedMediatorResponse>
    {
        public uint UserID { get; set; }
    }
    public class ProductsGetBookmarkedMediatorHandler : IRequestHandler<ProductsGetBookmarkedMediatorRequest, ProductsGetBookmarkedMediatorResponse>
    {
        private readonly ILogger<ProductsGetBookmarkedMediatorHandler> _logger;
        private readonly ProductsContext _productsContext;

        public ProductsGetBookmarkedMediatorHandler(ILogger<ProductsGetBookmarkedMediatorHandler> logger, ProductsContext productsContext)
        {
            _logger = logger;
            _productsContext = productsContext;
        }

        public async Task<ProductsGetBookmarkedMediatorResponse> Handle(ProductsGetBookmarkedMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new DefaultFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            var data = await _productsContext.ProductBookmarks.AsNoTracking()
                .Include(e => e.Product)
                .ThenInclude(e => e.Price)
                .OrderByDescending(e => e.ProductID)
                .Where(e => e.CustomerID == request.UserID)
                .Select(e => new ProductPresentation
                {
                    ID = e.ProductID,
                    Title = e.Product.Title,
                    Description = e.Product.Description,
                    CategoryID = e.Product.CategoryID,
                    Price = e.Product.Price,
                    Image = e.Product.Image,
                    Discount = e.Product.Discount
                })
                .ToListAsync();

            return new ProductsGetBookmarkedMediatorResponse { Data = data };
        }
    }
}
