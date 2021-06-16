using KShop.Shared.Domain.Contracts;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain
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
