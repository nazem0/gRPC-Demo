using Demo.Grpc.PaymentService;
using Demo.Grpc.PaymentService.Protos;
using Grpc.Core;

namespace Demo.Grpc.PaymentService.Services
{
    public class PaymentService(AppStorage storage) : PaymentGrpcService.PaymentGrpcServiceBase
    {

        public override Task<WithdrawalResponse> Withdraw(WithdrawalRequest r, ServerCallContext context)
        {
            var user = storage.Users.FirstOrDefault(x => x.Id == r.UserId);
            if (user == null)
            {
                return Task.FromResult(new WithdrawalResponse()
                {
                    Message = "User not found!",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            if (user.Balance < r.PaymentAmount)
            {
                return Task.FromResult(new WithdrawalResponse()
                {
                    Message = "Insufficient balance!",
                    IsSuccess = false,
                    Balance = user.Balance,
                    StatusCode = StatusCodes.Status409Conflict
                });
            }
            user.Balance -= r.PaymentAmount;
            return Task.FromResult(new WithdrawalResponse()
            {
                Balance = user.Balance,
                IsSuccess = true,
                Message = $"The amount of {r.PaymentAmount} has been withdrawn successfully.",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
