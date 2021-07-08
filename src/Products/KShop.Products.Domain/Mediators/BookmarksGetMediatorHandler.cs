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

    public class BookmarksGetMediatorResponse
    {
        public uint[] ProductsIDs { get; set; }
    }
    public class BookmarksGetMediatorRequest : IRequest<BookmarksGetMediatorResponse>
    {
        public uint UsedID { get; set; }
    }
    public class BookmarksGetMediatorHandler : IRequestHandler<BookmarksGetMediatorRequest, BookmarksGetMediatorResponse>
    {
        //private readonly IValidator<BookmarksGetMediatorFluentValidatorDto> _validator;
        private readonly ILogger<BookmarksGetMediatorHandler> _logger;
        private readonly ProductsContext _productsContext;

        public BookmarksGetMediatorHandler(ProductsContext productsContext, ILogger<BookmarksGetMediatorHandler> logger)
        {
            _productsContext = productsContext;
            _logger = logger;
        }

        public async Task<BookmarksGetMediatorResponse> Handle(BookmarksGetMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new BookmarksGetMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            var prodIds = await _productsContext.ProductBookmarks.AsNoTracking()
                .Where(e => e.CustomerID == request.UsedID)
                .Select(e => e.ProductID)
                .ToArrayAsync();

            return new BookmarksGetMediatorResponse() { ProductsIDs = prodIds };
        }
    }
}
