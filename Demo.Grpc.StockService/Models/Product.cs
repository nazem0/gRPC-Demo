namespace Demo.Grpc.StockService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double UnitCost { get; set; }
    }
}
