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

    public class ProductGetDetailsMediatorResponse : BaseResponse
    {
        public List<ProductDetails> Data { get; set; } = new List<ProductDetails>();
    }
    public class ProductGetDetailsMediatorRequest : IRequest<ProductGetDetailsMediatorResponse>
    {
        public List<uint> ProductID { get; set; } = new List<uint>();
    }
    public class ProductGetDetailsMediatorHandler : IRequestHandler<ProductGetDetailsMediatorRequest, ProductGetDetailsMediatorResponse>
    {
        private readonly ILogger<ProductGetDetailsMediatorHandler> _logger;
        private readonly ProductsContext _productsContext;

        public ProductGetDetailsMediatorHandler(ILogger<ProductGetDetailsMediatorHandler> logger, ProductsContext productsContext)
        {
            _logger = logger;
            _productsContext = productsContext;
        }

        //private readonly IValidator<GetProductDetailsMediatorFluentValidatorDto> _validator;

        public async Task<ProductGetDetailsMediatorResponse> Handle(ProductGetDetailsMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new GetProductDetailsMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            var data = (await _productsContext.Products
                .Include(e => e.Category)
                .Include(e => e.ProductAttributes)
                .ThenInclude(e => e.Attribute)
                .Where(e => request.ProductID.Contains(e.ID))
                .ToListAsync())
                .Select(e => new ProductDetails
                {
                    ID = e.ID,
                    Title = e.Title,
                    Description = e.Description,
                    CategoryID = e.CategoryID,
                    Price = e.Price,
                    Image = e.Image,
                    Attributes = e.ProductAttributes.Select(pa =>
                        new ProductDetails.Attribute { ID = pa.AttributeID, Title = pa.Attribute.Title, Value = pa.Value }).ToList(),
                })
                .ToList();


            return new ProductGetDetailsMediatorResponse()
            {
                Data = data
            };
        }
    }
}
