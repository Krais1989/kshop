namespace KShop.Communications.Contracts.ValueObjects
{
    //public interface IProductStack
    //{
    //    int ProductID { get; set; }
    //    int Quantity { get; set; }
    //}

    public struct ProductStack // : IProductStack
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
