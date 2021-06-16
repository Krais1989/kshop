using System;

namespace KShop.Shared.Integration.Contracts
{
    public interface ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
    }
}
