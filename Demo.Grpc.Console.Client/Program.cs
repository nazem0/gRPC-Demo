using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GRPC.WebApi.Protos;
using static GRPC.WebApi.Protos.AgeCalculatorGrpcService;

namespace Demo.Grpc.ConsoleClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Invalid input is not handled :X
            var channel = GrpcChannel.ForAddress("https://localhost:7110");
            var ageClacService = new AgeCalculatorGrpcServiceClient(channel);
            Console.WriteLine("Enter your birthday year, month, day :-");
            Console.Write("Year: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Month: ");
            int month = int.Parse(Console.ReadLine());
            Console.Write("Day: ");
            int day = int.Parse(Console.ReadLine());
            Date birthDate = new()
            {
                Year = year,
                Month = month,
                Day = day
            };
            var response = await ageClacService.CalculateAgeAsync(new AgeCalculatorRequest() { BirthDate = birthDate });
            Console.WriteLine($"You are {response.Years} years old!");
        }
    }
}
