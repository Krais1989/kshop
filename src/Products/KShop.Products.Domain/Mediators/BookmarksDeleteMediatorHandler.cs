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

    public class BookmarksDeleteMediatorResponse
    {
    }
    public class BookmarksDeleteMediatorRequest : IRequest<BookmarksDeleteMediatorResponse>
    {
        public uint UserID { get; set; }
        public uint[] ProductsIDs { get; set; }
    }
    public class BookmarksDeleteMediatorHandler : IRequestHandler<BookmarksDeleteMediatorRequest, BookmarksDeleteMediatorResponse>
    {
        private readonly ILogger<BookmarksDeleteMediatorHandler> _logger;
        private readonly ProductsContext _productsContext;

        public BookmarksDeleteMediatorHandler(ProductsContext productsContext, ILogger<BookmarksDeleteMediatorHandler> logger)
        {
            _productsContext = productsContext;
            _logger = logger;
        }

        //private readonly IValidator<BookmarksDeleteMediatorFluentValidatorDto> _validator;

        public async Task<BookmarksDeleteMediatorResponse> Handle(BookmarksDeleteMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new BookmarksDeleteMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);
            var del_bookmarks = _productsContext.ProductBookmarks.Where(pb => request.ProductsIDs.Contains(pb.ProductID)).ToList();
            _productsContext.ProductBookmarks.RemoveRange(del_bookmarks);
            await _productsContext.SaveChangesAsync();

            return new BookmarksDeleteMediatorResponse();
        }
    }
}
