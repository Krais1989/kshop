using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Shared.Domain.Contracts
{
    public class BaseBadRequestException : Exception
    {
        public int StatusCode { get; }
        public BaseBadRequestException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }

    }
}
