namespace GRPC.WebApi.Controllers
{
    public partial class OrdersController
    {
        public class OrdeRequsetDto
        {
            public int ProductId { get; set; }
            public double Quantity { get; set; }
            public int UserId { get; set; }
        }
    }
}
