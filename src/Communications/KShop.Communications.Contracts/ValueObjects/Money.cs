namespace KShop.Communications.Contracts.ValueObjects
{
    public class Money 
    {
        public static class CurrencySign
        {
            public const string RUB = "RUB";
            public const string USD = "USD";
            public const string EUR = "EUR";
        }

        public Money(decimal amount, string currency = CurrencySign.RUB)
        {
            Currency = currency;
            Amount = amount;
        }

        public string Currency { get; private set; }
        public decimal Amount { get; private set; }

        public override string ToString()
        {
            return $"{Amount} {Currency}";
        }
    }
}
