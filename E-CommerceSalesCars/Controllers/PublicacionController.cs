using Microsoft.AspNetCore.Mvc;
using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO;

namespace E_CommerceSalesCars.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacionController : ControllerBase
    {
        private readonly IServicioPublicacion _servicio;

        public PublicacionController(IServicioPublicacion servicio)
        {
            _servicio = servicio;
        }

        // GET: api/publicacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicacionDetalleDto>>> GetAll()
        {
            var publicaciones = await _servicio.ObtenerPublicacionesAsync();

            var dtoList = publicaciones.Select(p => new PublicacionDetalleDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Precio = p.Precio,
                EsUsado = p.EsUsado,
                Estado = p.Estado,
                Marca = p.Vehiculo?.Marca,
                Modelo = p.Vehiculo?.Modelo,
                Anio = p.Vehiculo?.Anio ?? 0
            });

            return Ok(dtoList);
        }

        // GET: api/publicacion/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PublicacionDetalleDto>> GetById(int id)
        {
            var publicaciones = await _servicio.ObtenerPublicacionesAsync();
            var pub = publicaciones.FirstOrDefault(p => p.Id == id);

            if (pub == null)
            {
                return NotFound();
            }

            var dto = new PublicacionDetalleDto
            {
                Id = pub.Id,
                Titulo = pub.Titulo,
                Precio = pub.Precio,
                EsUsado = pub.EsUsado,
                Estado = pub.Estado,
                Marca = pub.Vehiculo?.Marca,
                Modelo = pub.Vehiculo?.Modelo,
                Anio = pub.Vehiculo?.Anio ?? 0
            };

            return Ok(dto);
        }

        // POST: api/publicacion
        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] PublicacionCrearDto dto)
        {
            var pub = new Publicacion
            {
                Titulo = dto.Titulo,
                Precio = dto.Precio,
                EsUsado = dto.EsUsado,
                Estado = EstadoPublicacion.Activo,
                Fecha = DateTime.UtcNow,
                VehiculoId = dto.VehiculoId,
                UsuarioId = dto.UsuarioId
            };

            await _servicio.CrearPublicacionAsync(pub);

            return CreatedAtAction(nameof(GetById), new { id = pub.Id }, dto);
        }

        // PUT: api/publicacion/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Editar(int id, [FromBody] PublicacionEditarDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("El id no coincide");
            }

            var pub = new Publicacion
            {
                Id = dto.Id,
                Titulo = dto.Titulo,
                Precio = dto.Precio,
                EsUsado = dto.EsUsado,
                Estado = dto.Estado,
                VehiculoId = dto.VehiculoId,
                UsuarioId = dto.UsuarioId,
                Fecha = DateTime.UtcNow
            };

            await _servicio.EditarPublicacionAsync(id, pub);

            return NoContent();
        }

        // DELETE: api/publicacion/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _servicio.EliminarPublicacionAsync(id);
            return NoContent();
        }

        // GET: api/publicacion/filtrar
        [HttpGet("filtrar")]
        public async Task<ActionResult<IEnumerable<PublicacionDetalleDto>>> Filtrar(
            [FromQuery] string? marca,
            [FromQuery] string? modelo,
            [FromQuery] int? anioDesde,
            [FromQuery] int? anioHasta,
            [FromQuery] decimal? precioDesde,
            [FromQuery] decimal? precioHasta)
        {
            var publicaciones = await _servicio.FiltrarPublicacionesAsync(marca, modelo, anioDesde, anioHasta, precioDesde, precioHasta);

            var dtoList = publicaciones.Select(p => new PublicacionDetalleDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Precio = p.Precio,
                EsUsado = p.EsUsado,
                Estado = p.Estado,
                Marca = p.Vehiculo?.Marca,
                Modelo = p.Vehiculo?.Modelo,
                Anio = p.Vehiculo?.Anio ?? 0
            });

            return Ok(dtoList);
        }
    }
}
