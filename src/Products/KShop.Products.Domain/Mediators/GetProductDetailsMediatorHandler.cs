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

    public class GetProductDetailsMediatorResponse : BaseResponse
    {
        public ProductDetails Data { get; set; }
    }
    public class GetProductDetailsMediatorRequest : IRequest<GetProductDetailsMediatorResponse>
    {
        public uint ProductID { get; set; }
    }
    public class GetProductDetailsMediatorHandler : IRequestHandler<GetProductDetailsMediatorRequest, GetProductDetailsMediatorResponse>
    {
        private readonly ILogger<GetProductDetailsMediatorHandler> _logger;
        private readonly ProductsContext _productsContext;

        public GetProductDetailsMediatorHandler(ILogger<GetProductDetailsMediatorHandler> logger, ProductsContext productsContext)
        {
            _logger = logger;
            _productsContext = productsContext;
        }

        //private readonly IValidator<GetProductDetailsMediatorFluentValidatorDto> _validator;

        public async Task<GetProductDetailsMediatorResponse> Handle(GetProductDetailsMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new GetProductDetailsMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            var data = (await _productsContext.Products
                .Include(e => e.Category)
                .Include(e => e.ProductAttributes)
                .ThenInclude(e => e.Attribute)
                .Include(e => e.Positions)
                .Where(e => e.ID == request.ProductID)
                .ToListAsync())
                .Select(e => new ProductDetails
                {
                    ID = e.ID,
                    Title = e.Title,
                    Description = e.Description,
                    CategoryID = e.CategoryID,
                    Price = e.Money,
                    Image = e.Image,
                    Attributes = e.ProductAttributes.Select(pa => 
                        new ProductDetailsAttribute { ID = pa.AttributeID, Title = pa.Attribute.Title, Value = pa.Value }).ToList(),
                })
                .FirstOrDefault();

            return new GetProductDetailsMediatorResponse()
            {
                Data = data
            };
        }
    }
}
