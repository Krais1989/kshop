using KShop.Payments.Persistence;
using KShop.Shared.Domain.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain
{
    public class CommonPaymentProvider : ICommonPaymentProvider
    {
        private readonly ILogger<CommonPaymentProvider> _logger;
        private readonly IMockExternalPaymentProvider _mockPaymentProvider;

        public CommonPaymentProvider(
            ILogger<CommonPaymentProvider> logger,
            IMockExternalPaymentProvider mockPaymentProvider)
        {
            _logger = logger;
            _mockPaymentProvider = mockPaymentProvider;
        }

        public async Task<CommonPaymentProviderCreateResponse> CreateAsync(CommonPaymentProviderCreateRequest request, CancellationToken cancellationToken = default)
        {
            var result = new CommonPaymentProviderCreateResponse();

            switch (request.Provider)
            {
                case EPaymentProvider.None:
                    break;
                case EPaymentProvider.Mock:
                    var mockResult = await _mockPaymentProvider.CreateAsync(
                        new MockExternalPaymentCreateRequest
                        {
                            
                        });

                    result.ExternalPaymentID = mockResult.ExternalPaymentID;

                    break;
                case EPaymentProvider.Yookassa:
                    break;
            }

            return result;
        }

        public async Task<CommonPaymentProviderCancelResponse> CancelAsync(CommonPaymentProviderCancelRequest request, CancellationToken cancellationToken = default)
        {
            var result = new CommonPaymentProviderCancelResponse();
            switch (request.Provider)
            {
                case EPaymentProvider.None:
                    break;
                case EPaymentProvider.Mock:
                    var mockResult = await _mockPaymentProvider.CancelAsync(
                        new MockExternalPaymentCancelRequest
                        {
                            ExternalPaymentID = request.ExternalPaymentID
                        });
                    break;
                case EPaymentProvider.Yookassa:
                    break;
            }
            return result;
        }

        public async Task<CommonPaymentProviderGetStatusResponse> GetStatusAsync(CommonPaymentProviderGetStatusRequest request, CancellationToken cancellationToken = default)
        {
            var result = new CommonPaymentProviderGetStatusResponse();
            switch (request.Provider)
            {
                case EPaymentProvider.None:
                    break;
                case EPaymentProvider.Mock:
                    var mockResult = await _mockPaymentProvider.GetStatusAsync(
                        new MockExternalPaymentGetStatusRequest
                        {
                            ExternalPaymentID = request.ExternalPaymentID
                        });

                    /* Маппинг внешнего статуса во внутренний */
                    switch (mockResult.PaymentStatus)
                    {
                        case EMockPaymentStatus.None:
                            break;
                        case EMockPaymentStatus.Processing:
                            result.PaymentStatus = EPaymentStatus.Pending;
                            break;
                        case EMockPaymentStatus.Successed:
                            result.PaymentStatus = EPaymentStatus.Paid;
                            break;
                        case EMockPaymentStatus.Faulted:
                            result.PaymentStatus = EPaymentStatus.Error;
                            break;
                        case EMockPaymentStatus.Cancelled:
                            result.PaymentStatus = EPaymentStatus.Canceled;
                            break;
                    }

                    break;
                case EPaymentProvider.Yookassa:
                    break;
            }
            return result;
        }
    }
}
