using Demo.Grpc.WebApi.Models;

namespace Demo.Grpc.WebApi
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
                    Email = "user1@example.com",
                    Password = "user1@example.com",
                    Roles = ["admin"]
                },
                new()
                {
                    Id = 2,
                    Email = "user2@example.com",
                    Password = "user2@example.com"
                },
                new()
                {
                    Id = 3,
                    Email = "user3@example.com",
                    Password = "user3@example.com"
                }
            ];
        }
    }
}
