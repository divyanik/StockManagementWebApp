namespace StockManagementWebApp.ViewModels
{
    public class OrderViewModel
    {
        public List<OrderItemViewModel> OrderItems { get; set; }
    }
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
