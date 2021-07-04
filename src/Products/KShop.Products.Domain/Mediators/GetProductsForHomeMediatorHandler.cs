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

namespace KShop.Products.Domain.Mediators
{

    public class GetProductsForHomeMediatorResponse : BaseResponse
    {
        public List<ProductPresentation> Data { get; set; } = new List<ProductPresentation>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
    public class GetProductsForHomeMediatorRequest : IRequest<GetProductsForHomeMediatorResponse>
    {
        public int PageIndex { get; set; }
    }
    public class GetProductsForHomeMediatorHandler : IRequestHandler<GetProductsForHomeMediatorRequest, GetProductsForHomeMediatorResponse>
    {
        private readonly ILogger<GetProductsForHomeMediatorHandler> _logger;
        private readonly ProductsContext _productsContext;

        public GetProductsForHomeMediatorHandler(ILogger<GetProductsForHomeMediatorHandler> logger, ProductsContext productsContext)
        {
            _logger = logger;
            _productsContext = productsContext;
        }

        //private readonly IValidator<GetProductsForHomeFluentValidatorDto> _validator;

        public async Task<GetProductsForHomeMediatorResponse> Handle(GetProductsForHomeMediatorRequest request, CancellationToken cancellationToken)
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
                .OrderByDescending(e => e.ID)
                .Skip(request.PageIndex * pagesize)
                .Take(pagesize)
                .ToListAsync();

            //var categories = await _productsContext.Cate

            // Расчитать сколько осталось продуктов

            return new GetProductsForHomeMediatorResponse() { Data = data };
        }
    }
}
