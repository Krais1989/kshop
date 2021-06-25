using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace KShop.Shared.Domain.Contracts
{
    public abstract class BaseResponse
    {
        public string ErrorMessage { get; set; }
        [JsonIgnore]
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }

}
