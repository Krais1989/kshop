using KShop.Communications.Contracts.Payments;
using KShop.Payments.Domain.ExternalPaymentProviders.Common.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Common
{
    /// <summary>
    /// Интерфейс общего провайдера платежных систем
    /// </summary>
    public interface ICommonPaymentProvider
    {
        Task<CommonPaymentProviderCreateResponse> CreateAsync(CommonPaymentProviderCreateRequest request, CancellationToken cancellationToken = default);
        Task<CommonPaymentProviderCancelResponse> CancelAsync(CommonPaymentProviderCancelRequest request, CancellationToken cancellationToken = default);
        Task<CommonPaymentProviderGetStatusResponse> GetStatusAsync(CommonPaymentProviderGetStatusRequest request, CancellationToken cancellationToken = default);
    }
}
