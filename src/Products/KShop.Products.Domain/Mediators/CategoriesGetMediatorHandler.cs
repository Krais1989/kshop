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
    public class CategoriesGetMediatorResponse
    {
        public List<CategoryPresentation> Categories { get; private set; }

        public CategoriesGetMediatorResponse(List<CategoryPresentation> categories)
        {
            Categories = categories;
        }
    }
    public class CategoriesGetMediatorRequest : IRequest<CategoriesGetMediatorResponse>
    {
    }
    public class CategoriesGetMediatorHandler : IRequestHandler<CategoriesGetMediatorRequest, CategoriesGetMediatorResponse>
    {
        private readonly ILogger<CategoriesGetMediatorHandler> _logger;
        private readonly ProductsContext _productsContext;

        public CategoriesGetMediatorHandler(ILogger<CategoriesGetMediatorHandler> logger, ProductsContext productsContext)
        {
            _logger = logger;
            _productsContext = productsContext;
        }

        public async Task<CategoriesGetMediatorResponse> Handle(CategoriesGetMediatorRequest request, CancellationToken cancellationToken)
        {
            var result = await _productsContext.Categories.AsNoTracking()
                .Select(e => new CategoryPresentation { ID = e.ID, Name = e.Name }).ToListAsync();

            return new CategoriesGetMediatorResponse(result);
        }
    }
}
