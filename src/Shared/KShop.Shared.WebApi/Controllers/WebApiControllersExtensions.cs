using KShop.Shared.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KShop.Shared.WebApi
{
    public static class WebApiControllersExtensions
    {
        public static IActionResult WrapResponse(this ControllerBase controller, BaseResponse response)
        {
            if (response.IsSuccess)
                return controller.Ok(response);
            else
                return controller.BadRequest(response);
        }
    }
}
