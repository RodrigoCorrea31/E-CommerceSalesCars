using Microsoft.AspNetCore.Mvc;
using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO.E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO;

namespace E_CommerceSalesCars.Presentacion.Controllers
{
    [Route("api/publicacion")]
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
                Vehiculo = new VehiculoDto
                {
                    Marca = p.Vehiculo?.Marca,
                    Modelo = p.Vehiculo?.Modelo,
                    Anio = p.Vehiculo?.Anio ?? 0,
                    Kilometraje = p.Vehiculo?.Kilometraje ?? 0,
                    Combustible = p.Vehiculo?.Combustible ?? 0,
                    Color = p.Vehiculo?.Color,
                    Imagenes = p.Vehiculo.Imagenes?.Select(img => new ImagenDto
                    {
                        Id = img.Id,
                        Url = img.Url
                    }).ToList()
                }
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

            var dto = publicaciones.Select(p => new PublicacionDetalleDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Precio = p.Precio,
                EsUsado = p.EsUsado,
                Estado = p.Estado,
                Vehiculo = new VehiculoDto
                {
                    Marca = p.Vehiculo?.Marca,
                    Modelo = p.Vehiculo?.Modelo,
                    Anio = p.Vehiculo?.Anio ?? 0,
                    Kilometraje = p.Vehiculo?.Kilometraje ?? 0,
                    Combustible = p.Vehiculo?.Combustible ?? 0,
                    Color = p.Vehiculo?.Color,
                }
            });



            return Ok(dto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Crear([FromBody] PublicacionCrearDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (userIdClaim == null) return Unauthorized("Token inválido");

                int usuarioId = int.Parse(userIdClaim);

                var vehiculo = new Vehiculo
                {
                    Marca = dto.Vehiculo.Marca,
                    Modelo = dto.Vehiculo.Modelo,
                    Anio = dto.Vehiculo.Anio,
                    Kilometraje = dto.Vehiculo.Kilometraje,
                    Combustible = dto.Vehiculo.Combustible,
                    Color = dto.Vehiculo.Color,
                    Imagenes = dto.Vehiculo.Imagenes
                        .Where(url => !string.IsNullOrWhiteSpace(url))
                        .Select((url, idx) => new ImagenVehiculo
                        {
                            Url = url,
                            Orden = idx,
                            EsPrincipal = idx == 0
                        }).ToList()
                };

                foreach (var img in vehiculo.Imagenes)
                {
                    img.Vehiculo = vehiculo;
                }

                var pub = new Publicacion
                {
                    Titulo = dto.Titulo,
                    Precio = dto.Precio,
                    EsUsado = dto.EsUsado,
                    Estado = EstadoPublicacion.Activo,
                    Fecha = DateTime.UtcNow,
                    UsuarioId = usuarioId,
                    Vehiculo = vehiculo
                };

                await _servicio.CrearPublicacionAsync(pub);

                return CreatedAtAction(nameof(GetById), new { id = pub.Id }, new
                {
                    pub.Id,
                    pub.Titulo,
                    pub.Precio,
                    pub.EsUsado,
                    pub.Estado,
                    pub.Fecha,
                    pub.UsuarioId,
                    Vehiculo = new
                    {
                        pub.Vehiculo.Marca,
                        pub.Vehiculo.Modelo,
                        pub.Vehiculo.Anio,
                        pub.Vehiculo.Kilometraje,
                        pub.Vehiculo.Combustible,
                        pub.Vehiculo.Color,
                        Imagenes = pub.Vehiculo.Imagenes.Select(i => new {
                            i.Url,
                            i.Orden,
                            i.EsPrincipal
                        })
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear publicación: {ex.Message}");
            }
        }


        [HttpPost("upload-imagen")]
        [Authorize]
        public async Task<ActionResult<string>> UploadImagen(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Archivo inválido");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"{Request.Scheme}://{Request.Host}/imagenes/{fileName}";
            return Ok(url);
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
                Vehiculo = new VehiculoDto
                {
                    Marca = p.Vehiculo?.Marca,
                    Modelo = p.Vehiculo?.Modelo,
                    Anio = p.Vehiculo?.Anio ?? 0,
                    Kilometraje = p.Vehiculo?.Kilometraje ?? 0,
                    Combustible = p.Vehiculo?.Combustible ?? 0,
                    Color = p.Vehiculo?.Color,
                }
            });


            return Ok(dtoList);
        }
    }
}
