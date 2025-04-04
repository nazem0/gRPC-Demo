using Demo.Grpc.PaymentService.Models;
using System.Diagnostics;

namespace Demo.Grpc.PaymentService
{
    public class AppStorage
    {
        public List<User> Users { get; set; } = [];
        public AppStorage()
        {
            Users =
            [
                new()
                {
                    Id = 1,
                    Balance = 100,
                    Name = "Ahmed"
                },
                new()
                {
                    Id = 2,
                    Balance = 200,
                    Name = "Mohamed"
                },
                new()
                {
                    Id = 3,
                    Balance = 300,
                    Name = "Mahmoud"
                },
            ];

        }
    }
}
