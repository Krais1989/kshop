using System;

namespace KShop.Shared.Domain.Contracts
{
    public class MoneyException : Exception
    {
        public MoneyException(string message) : base($"Money operation error: {message}")
        {
        }
    }

    public class Money
    {
        public static class CurrencySign
        {
            public const string RUB = "RUB";
            public const string USD = "USD";
            public const string EUR = "EUR";
        }

        public static Money operator -(Money m1) => new Money(-m1.Price, m1.Currency);
        
        public static Money operator +(Money m1, Money m2)
        {
            if (m1.Currency != m2.Currency)
                throw new MoneyException($"Trying to operate money objects with a different currencies: {m1.Currency} and {m2.Currency}");
            return new Money(m1.Price + m2.Price, m1.Currency);
        }

        public static Money operator -(Money m1, Money m2)
        {
            if (m1.Currency != m2.Currency)
                throw new MoneyException($"Trying to operate money objects with a different currencies: {m1.Currency} and {m2.Currency}");
            return new Money(m1.Price - m2.Price, m1.Currency);
        }

        public static Money operator +(Money m1, decimal d) => new Money(m1.Price + d, m1.Currency);
        public static Money operator -(Money m1, decimal d) => new Money(m1.Price - d, m1.Currency);
        public static Money operator *(Money m1, decimal d) => new Money(m1.Price * d, m1.Currency);
        public static Money operator /(Money m1, decimal d) => new Money(m1.Price / d, m1.Currency);


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
