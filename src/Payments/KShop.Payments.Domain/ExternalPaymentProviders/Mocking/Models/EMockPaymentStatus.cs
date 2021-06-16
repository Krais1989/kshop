using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain
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
