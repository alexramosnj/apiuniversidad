
using ApiBase.DAL.Modelos_BD_Universidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiBase.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspaciosController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly BD_UniversidadContext _UniversidadContext;

        public EspaciosController(IConfiguration config, BD_UniversidadContext bD_UniversidadContext)
        {
            _config = config;
            _UniversidadContext = bD_UniversidadContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            
            // return Ok(new { message = "Acceso autorizado a EspaciosController" });  

            var items = await _UniversidadContext.Espacios.ToListAsync();
            return Ok(items);
        }
    }
}
