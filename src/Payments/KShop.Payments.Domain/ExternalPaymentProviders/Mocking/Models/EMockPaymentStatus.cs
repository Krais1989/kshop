using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Mocking.Models
{
    public enum EMockPaymentStatus
    {
        None,
        Processing,
        Successed,
        Faulted,
        Cancelled
    }
}
