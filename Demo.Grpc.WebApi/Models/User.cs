namespace Demo.Grpc.WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        //For demo purposes, currently not password hash :/
        public string Password { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
