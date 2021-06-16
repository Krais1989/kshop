namespace KShop.Shared.Domain.Contracts
{
    public class Money
    {
        public static class CurrencySign
        {
            public const string RUB = "RUB";
            public const string USD = "USD";
            public const string EUR = "EUR";
        }

        public Money(decimal price, string currency = CurrencySign.RUB)
        {
            Currency = currency;
            Price = price;
        }

        public string Currency { get; private set; }
        public decimal Price { get; private set; }

        public override string ToString()
        {
            return $"{Price} {Currency}";
        }
    }
}
