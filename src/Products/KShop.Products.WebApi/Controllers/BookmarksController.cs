using KShop.Products.Domain;
using KShop.Shared.Authentication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.WebApi.Controllers
{
    public class AddBookmarksDto
    {
        public uint[] ProductsIDs { get; set; }
    }

    public class DelBookmarksDto
    {
        public uint[] ProductsIDs { get; set; }
    }

    [Route("api/bookmarks")]
    [ApiController]
    public class BookmarksController : ControllerBase
    {
        private readonly ILogger<BookmarksController> _logger;
        private readonly IMediator _mediator;

        public BookmarksController(ILogger<BookmarksController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetBookmarks()
        {
            var response = await _mediator.Send(new BookmarksGetMediatorRequest(usedID: this.GetCurrentUserID().Value));
            return Ok(response);
        }

        [HttpPost]
        public async ValueTask<IActionResult> AddBookmarks([FromBody] AddBookmarksDto dto)
        {
            var request = new BookmarksAddMediatorRequest(userID: this.GetCurrentUserID().Value, productsIDs: dto.ProductsIDs);
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete]
        public async ValueTask<IActionResult> DeleteBookmarks([FromBody] DelBookmarksDto dto)
        {
            var request = new BookmarksDeleteMediatorRequest ( userID: this.GetCurrentUserID().Value, productsIDs: dto.ProductsIDs );
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
