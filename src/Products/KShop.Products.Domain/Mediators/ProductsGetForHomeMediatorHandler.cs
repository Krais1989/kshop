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

    public class ProductsGetForHomeMediatorResponse : BaseResponse
    {
        public ProductsGetForHomeMediatorResponse(List<ProductPresentation> data, int pagesLeft)
        {
            Data = data;
            PagesLeft = pagesLeft;
        }

        public List<ProductPresentation> Data { get; private set; } = new List<ProductPresentation>();
        public int PagesLeft { get; private set; } = 0;

    }
    public class ProductsGetForHomeMediatorRequest : IRequest<ProductsGetForHomeMediatorResponse>
    {
        public ProductsGetForHomeMediatorRequest() { }
        public ProductsGetForHomeMediatorRequest(int pageIndex, uint categoryID, uint? userID)
        {
            UserID = userID;
            PageIndex = pageIndex;
            CategoryID = categoryID;
        }

        public uint? UserID { get; set; }
        public int PageIndex { get; set; }
        public uint CategoryID { get; set; } = 0;
    }
    public class ProductsGetForHomeMediatorHandler : IRequestHandler<ProductsGetForHomeMediatorRequest, ProductsGetForHomeMediatorResponse>
    {
        private readonly ILogger<ProductsGetForHomeMediatorHandler> _logger;
        private readonly ProductsContext _productsContext;

        public ProductsGetForHomeMediatorHandler(ILogger<ProductsGetForHomeMediatorHandler> logger, ProductsContext productsContext)
        {
            _logger = logger;
            _productsContext = productsContext;
        }

        //private readonly IValidator<GetProductsForHomeFluentValidatorDto> _validator;

        public async Task<ProductsGetForHomeMediatorResponse> Handle(ProductsGetForHomeMediatorRequest request, CancellationToken cancellationToken)
        {
            /* Запрашивать Product */

            //var validatorDto = new GetProductsForHomeFluentValidatorDto() { };
            //_validator.Validate(validatorDto);
            int pagesize = 10;
            var data = await _productsContext.Products.AsNoTracking()
                .Select(e => new ProductPresentation
                {
                    ID = e.ID,
                    Title = e.Title,
                    Description = e.Description,
                    CategoryID = e.CategoryID,
                    Price = e.Price,
                    Image = e.Image
                })
                .Where(e => request.CategoryID == 0 || e.CategoryID == request.CategoryID)
                .OrderByDescending(e => e.ID)
                .Skip(request.PageIndex * pagesize)
                .Take(pagesize)
                .ToListAsync();


            return new ProductsGetForHomeMediatorResponse(data, 0);
        }
    }
}
