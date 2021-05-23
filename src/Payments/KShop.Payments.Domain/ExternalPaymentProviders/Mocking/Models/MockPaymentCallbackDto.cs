using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Mocking.Models
{
    public enum EMockPaymentExternalStatus
    {
        Paid,
        Canceled,
        Error
    }

    public class MockPaymentCallbackDto
    {
        public string ExternalPaymentID { get; set; }
        public EMockPaymentExternalStatus Status { get; set; }
    }
}
