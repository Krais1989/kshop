using System;

namespace KShop.Communications.Contracts
{
    public interface ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
    }
}
