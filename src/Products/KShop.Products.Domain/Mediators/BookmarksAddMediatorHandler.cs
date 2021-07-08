using FluentValidation;
using KShop.Products.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Products.Domain
{
    public class BookmarksAddMediatorResponse
    {
    }
    public class BookmarksAddMediatorRequest : IRequest<BookmarksAddMediatorResponse>
    {
        public uint UserID { get; set; }
        public uint[] ProductsIDs { get; set; }
    }
    public class BookmarksAddMediatorHandler : IRequestHandler<BookmarksAddMediatorRequest, BookmarksAddMediatorResponse>
    {
        private readonly ILogger<BookmarksAddMediatorHandler> _logger;
        private readonly ProductsContext _productsContext;

        public BookmarksAddMediatorHandler(ProductsContext productsContext, ILogger<BookmarksAddMediatorHandler> logger)
        {
            _productsContext = productsContext;
            _logger = logger;
        }

        //private readonly IValidator<AddBookmarksMediatorFluentValidatorDto> _validator;

        public async Task<BookmarksAddMediatorResponse> Handle(BookmarksAddMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new AddBookmarksMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            var bookmarks = request.ProductsIDs.Select(pid => new ProductBookmark { CustomerID = request.UserID, ProductID = pid }).ToList();
            await _productsContext.ProductBookmarks.AddRangeAsync(bookmarks);
            await _productsContext.SaveChangesAsync();
            return new BookmarksAddMediatorResponse();
        }
    }
}
