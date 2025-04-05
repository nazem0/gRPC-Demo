using Demo.Grpc.StockService;
using Demo.Grpc.StockService.Protos;
using Grpc.Core;

namespace Demo.Grpc.StockService.Services
{
    public class StockService(AppStorage storage) : StockGrpcService.StockGrpcServiceBase
    {
        public override Task<SellResponse> Sell(SellRequest r, ServerCallContext context)
        {
            var product = storage.Products.FirstOrDefault(x => x.Id == r.ProductId);
            if (product == null)
            {
                return Task.FromResult(new SellResponse()
                {
                    Message = "Product not found!",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            if (product.Quantity < r.Quantity)
            {
                return Task.FromResult(new SellResponse()
                {
                    Message = "Insufficient quantity!",
                    IsSuccess = false,
                    Quantity = product.Quantity,
                    UnitCost = product.UnitCost,
                    StatusCode = StatusCodes.Status409Conflict
                });
            }
            product.Quantity -= r.Quantity;
            return Task.FromResult(new SellResponse()
            {
                IsSuccess = true,
                Message = $"The quantity of {r.Quantity} has been sold successfully.",
                UnitCost = product.UnitCost,
                Quantity = product.Quantity,
                StatusCode = StatusCodes.Status200OK
            });
        }
        public override Task<RestockResponse> Restock(RestockRequest r, ServerCallContext context)
        {
            var product = storage.Products.FirstOrDefault(x => x.Id == r.ProductId);
            if (product == null)
            {
                return Task.FromResult(new RestockResponse()
                {
                    Message = "Product not found!",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            product.Quantity += r.Quantity;
            product.UnitCost = (product.UnitCost + r.UnitCost) / 2;
            return Task.FromResult(new RestockResponse()
            {
                IsSuccess = true,
                Message = $"The quantity of {r.Quantity} has been added to stock successfully.",
                UnitCost = product.UnitCost,
                Quantity = product.Quantity,
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
