using Demo.Grpc.PaymentService.Protos;
using Demo.Grpc.StockService.Protos;
using Demo.Grpc.WebApi;
using Grpc.Net.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddGrpcClient<PaymentGrpcService.PaymentGrpcServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7103");
});

services.AddGrpcClient<StockGrpcService.StockGrpcServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7253");
});
services.AddGrpc().AddJsonTranscoding();
services.AddGrpcReflection();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddGrpcSwagger().AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new() { Title = "TestGRPC", Version = "v1" });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapGrpcService<AgeCalculatorService>();
//app.MapGrpcReflectionService();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "TestGRPC v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
