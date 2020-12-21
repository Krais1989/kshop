namespace KShop.Communications.Contracts.ValueObjects
{
    public interface IProductStack
    {
        int ProductID { get; set; }
        int Quantity { get; set; }
    }
}
