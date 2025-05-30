﻿using Demo.Grpc.PaymentService.Protos;
using Demo.Grpc.StockService.Protos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GRPC.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class OrdersController(
        StockGrpcService.StockGrpcServiceClient stockService,
        PaymentGrpcService.PaymentGrpcServiceClient paymentService) : ControllerBase
    {
        [HttpPost, Authorize]
        public async Task<IActionResult> Order(OrdeRequsetDto input)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellResult = await stockService.SellAsync(new SellRequest
            {
                ProductId = input.ProductId,
                Quantity = input.Quantity
            });
            if (!sellResult.IsSuccess)
            {
                return StatusCode(sellResult.StatusCode, sellResult.Message);
            }
            double revenueMultiplier = 1.2;
            var withdrawalRequest = new WithdrawalRequest
            {
                PaymentAmount = sellResult.UnitCost * input.Quantity * revenueMultiplier,
                UserId = userId,
            };
            var withdrawalResult = await paymentService.WithdrawAsync(withdrawalRequest);
            if (!withdrawalResult.IsSuccess)
            {
                _ = await stockService.RestockAsync(new RestockRequest
                {
                    ProductId = input.ProductId,
                    Quantity = input.Quantity,
                    UnitCost = sellResult.UnitCost,
                });
                return StatusCode(withdrawalResult.StatusCode, withdrawalResult.Message);
            }
            return Ok($"Order completed successfully!, Paid ${withdrawalRequest.PaymentAmount}.");
        }
    }
}
