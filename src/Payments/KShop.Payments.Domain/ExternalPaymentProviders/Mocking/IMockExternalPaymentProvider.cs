using KShop.Payments.Domain.ExternalPaymentProviders.Mocking.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Mocking
{
    /// <summary>
    /// Провайдер для тестирования внешних платежей
    /// </summary>
    public interface IMockExternalPaymentProvider
    {
        Task<MockExternalPaymentCreateResponse> CreateAsync(MockExternalPaymentCreateRequest request, CancellationToken cancellationToken = default);
        Task<MockExternalPaymentCancelResponse> CancelAsync(MockExternalPaymentCancelRequest request, CancellationToken cancellationToken = default); 
        Task<MockExternalPaymentGetStatusResponse> GetStatusAsync(MockExternalPaymentGetStatusRequest request, CancellationToken cancellationToken = default);
    }
}
