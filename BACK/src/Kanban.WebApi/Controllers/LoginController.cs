namespace webapi_kanban.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using webapi_kanban.dto;


    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        [HttpPost()]
        public IActionResult Login([FromBody] LoginDto request)
        {
            var login = Environment.GetEnvironmentVariable("LOGIN");
            var password = Environment.GetEnvironmentVariable("PASSWORD");

            if (request.Login == login && request.Senha == password)
            {
                var token = GenerateJwtToken();
                return Ok(token);
            }

            return Unauthorized();
        }

        private string GenerateJwtToken()
        {
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "user"),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
