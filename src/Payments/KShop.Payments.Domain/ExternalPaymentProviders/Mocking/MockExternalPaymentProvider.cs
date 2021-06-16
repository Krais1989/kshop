
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain
{
    public class MockExternalPaymentProvider : IMockExternalPaymentProvider
    {
        private readonly ILogger<MockExternalPaymentProvider> _logger;

        public async Task<MockExternalPaymentCreateResponse> CreateAsync(MockExternalPaymentCreateRequest request, CancellationToken cancellationToken = default)
        {
            var result = new MockExternalPaymentCreateResponse
            {
                ExternalPaymentID = Guid.NewGuid().ToString()
            };
            return result;
        }


        public async Task<MockExternalPaymentCancelResponse> CancelAsync(MockExternalPaymentCancelRequest request, CancellationToken cancellationToken = default)
        {
            var result = new MockExternalPaymentCancelResponse { };
            return result;
        }

        public async Task<MockExternalPaymentGetStatusResponse> GetStatusAsync(MockExternalPaymentGetStatusRequest request, CancellationToken cancellationToken = default)
        {
            var result = new MockExternalPaymentGetStatusResponse
            { 
                PaymentStatus = EMockPaymentStatus.Successed
            };
            return result;
        }
    }
}
