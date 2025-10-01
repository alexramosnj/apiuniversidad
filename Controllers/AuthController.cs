using ApiBase.DAL.Modelos_BD_Universidad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiUniversidad.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly BD_UniversidadContext _UniversidadContext;

        public AuthController(
            IConfiguration config, BD_UniversidadContext bD_UniversidadContext)
        {
            _config = config;
            _UniversidadContext = bD_UniversidadContext;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Aquí debería ir la validación contra la BD o servicio de usuarios
            // var usuario = new { UserName = "testuser", PasswordHash = "password" }; // Simulación
            var usuario = _UniversidadContext.Usuarios.FirstOrDefault(u => u.userName == model.userName && u.passwordHash == model.password);
            if (usuario != null)
            {
                var token = GenerateToken(usuario.userName);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        private string GenerateToken(string username)
        {
            var jwtConfig = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfig["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}
