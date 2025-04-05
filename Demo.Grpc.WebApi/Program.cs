using Demo.Grpc.PaymentService.Protos;
using Demo.Grpc.StockService.Protos;
using Demo.Grpc.WebApi;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddAuthentication(Option =>
{
    Option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    Option.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    Option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(Options =>
{
    Options.TokenValidationParameters =
    new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:SecretKey"]!)),
    };
});
services.AddAuthorization();
services.AddSingleton<AppStorage>();
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
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyToken\"",
    });
    o.AddSecurityRequirement(
        new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                    new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme,
                    }
                }, []
            }
        });
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
