namespace StockManagementWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public int Demand { get; set; }
        public int QuantityPurchased { get; set; }
    }
}
