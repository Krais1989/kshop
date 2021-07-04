using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace KShop.Shared.Domain.Contracts
{
    public abstract class BaseResponse
    {
        //[JsonPropertyName("errorMessage")]
        //[JsonInclude]
        private string _errorMessage = null;
        //[JsonIgnore]
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    IsSuccess = false;
                }
                _errorMessage = value;
            }
        }

        public bool IsSuccess { get; private set; } = true;

        public BaseResponse(bool isSuccess = true)
        {
            IsSuccess = isSuccess;
        }

        public BaseResponse(string error)
        {
            IsSuccess = false;
            ErrorMessage = error;
        }
    }

}
