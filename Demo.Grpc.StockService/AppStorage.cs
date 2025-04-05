using Demo.Grpc.StockService.Models;

namespace Demo.Grpc.StockService
{
    public class AppStorage
    {
        public List<Product> Products { get; set; } = [];
        public AppStorage()
        {
            Products =
            [
                new()
                {
                    Id = 1,
                    UnitCost = 20,
                    Name = "Chair" ,
                    Quantity = 100
                },
                new()
                {
                    Id = 2,
                    UnitCost = 100,
                    Name = "Desk" ,
                    Quantity = 5
                },
                new()
                {
                    Id = 3,
                    UnitCost = 5,
                    Name = "Pen" ,
                    Quantity = 300
                }
            ];
        }
    }
}
