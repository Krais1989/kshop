using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Domain.Contracts
{
    public abstract class BaseResponse
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }
}
