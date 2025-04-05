using Grpc.Core;
using GRPC.WebApi.Protos;

namespace Demo.Grpc.WebApi
{
    public class AgeCalculatorService : AgeCalculatorGrpcService.AgeCalculatorGrpcServiceBase
    {
        public override Task<AgeCalculatorResponse> CalculateAge(AgeCalculatorRequest r, ServerCallContext context)
        {
            var bd = r.BirthDate;
            DateTime currentDate = DateTime.UtcNow;
            DateTime birthDate = new(bd.Year, bd.Month, bd.Day);
            var yearsDifference = (currentDate - birthDate).Days / 365;
            AgeCalculatorResponse response = new()
            {
                Years = yearsDifference,
            };
            return Task.FromResult(response);
        }
    }
}
