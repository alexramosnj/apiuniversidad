using ApiBase.DAL.Modelos_BD_Universidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiUniversidad.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EspaciosController : ControllerBase
    {
        private readonly BD_UniversidadContext _db;

        public EspaciosController(BD_UniversidadContext db)
        {
            _db = db;
        }

        // GET: api/Espacios/Leer
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Leer(CancellationToken ct)
        {
            var items = await _db.Espacios
                .OrderByDescending(x => x.fechaCreacion)
                .ToListAsync(ct);

            return Ok(items);
        }

        // POST: api/Espacios/Crear
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] EspacioCrearDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.nombre))
                return BadRequest(new { message = "El nombre es obligatorio." });

            var entity = new Espacio
            {
                nombre = dto.nombre.Trim(),
                descripcion = dto.descripcion?.Trim(),
                clasificacion = dto.clasificacion?.Trim(),
                observaciones = dto.observaciones?.Trim(),
                activo = dto.activo,
                fechaCreacion = DateTime.UtcNow,
                fechaActualizacion = DateTime.UtcNow
            };

            _db.Espacios.Add(entity);
            await _db.SaveChangesAsync(ct);

            return Ok(entity);
        }

        // PUT: api/Espacios/Actualizar/{id}
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar([FromRoute] int id, [FromBody] EspacioActualizarDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.nombre))
                return BadRequest(new { message = "El nombre es obligatorio." });

            var entity = await _db.Espacios.FirstOrDefaultAsync(x => x.idEspacio == id, ct);
            if (entity == null)
                return NotFound(new { message = "No se encontró el espacio." });

            entity.nombre = dto.nombre.Trim();
            entity.descripcion = dto.descripcion?.Trim();
            entity.clasificacion = dto.clasificacion?.Trim();
            entity.observaciones = dto.observaciones?.Trim();
            entity.activo = dto.activo;
            entity.fechaActualizacion = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
            return Ok(entity);
        }

        // PUT: api/Espacios/Desactivar/{id}
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id, CancellationToken ct)
        {
            var entity = await _db.Espacios.FirstOrDefaultAsync(x => x.idEspacio == id, ct);
            if (entity == null)
                return NotFound(new { message = "No se encontró el espacio." });

            if (!entity.activo)
                return Ok(entity); // ya está inactivo

            entity.activo = false;
            entity.fechaActualizacion = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);

            return Ok(entity);
        }

        // PUT: api/Espacios/Activar/{id}
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Activar([FromRoute] int id, CancellationToken ct)
        {
            var entity = await _db.Espacios.FirstOrDefaultAsync(x => x.idEspacio == id, ct);
            if (entity == null)
                return NotFound(new { message = "No se encontró el espacio." });

            if (entity.activo)
                return Ok(entity); // ya está activo

            entity.activo = true;
            entity.fechaActualizacion = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);

            return Ok(entity);
        }
    }
}
