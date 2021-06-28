namespace KShop.Shared.Domain.Contracts
{
    public enum EPaymentProvider : int
    {
        None = 0,
        Mock,
        Yookassa
    }

    public enum EShippingMethod : int
    {
        /// <summary>
        /// самовывоз
        /// </summary>
        Pickup,
        Default
    }
}
