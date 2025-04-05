using Demo.Grpc.WebApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.Grpc.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(AppStorage storage, IConfiguration config) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto r, CancellationToken c = default)
        {
            var user = storage.Users.FirstOrDefault(x => x.Email == r.Email && x.Password == r.Password);
            if (user == null)
            {
                return Unauthorized("Invalid Credentials");
            }
            IEnumerable<string> rolesNameList = user.Roles;
            var RolesClaimsList = rolesNameList.Select(o => new Claim(ClaimTypes.Role, o));
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(config["Auth:SecretKey"]!));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
            }.Union(RolesClaimsList);

            var token = new JwtSecurityToken(
              expires: DateTime.Now.AddDays(30),
              signingCredentials: credentials,
              claims: claims);
            string generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(generatedToken);
        }
    }
}
