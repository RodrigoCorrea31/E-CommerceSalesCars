using Microsoft.AspNetCore.Mvc;
using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO.E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO;
using E_CommerceSalesCars.Infraestructura.DTOs.OfertaDTO;
using E_CommerceSalesCars.Infraestructura.DTOs.UsuarioDTO;

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
            var publicacion = await _servicio.ObtenerDetallePorIdAsync(id);

            if (publicacion == null)
                return NotFound();

            var dto = new PublicacionDetalleDto
            {
                Id = publicacion.Id,
                Titulo = publicacion.Titulo,
                Precio = publicacion.Precio,
                EsUsado = publicacion.EsUsado,
                Estado = publicacion.Estado,
                Vehiculo = new VehiculoDto
                {
                    Marca = publicacion.Vehiculo?.Marca,
                    Modelo = publicacion.Vehiculo?.Modelo,
                    Anio = publicacion.Vehiculo?.Anio ?? 0,
                    Kilometraje = publicacion.Vehiculo?.Kilometraje ?? 0,
                    Combustible = publicacion.Vehiculo?.Combustible ?? 0,
                    Color = publicacion.Vehiculo?.Color,
                    Imagenes = publicacion.Vehiculo?.Imagenes?.Select(i => new ImagenDto
                    {
                        Id = i.Id,
                        Url = i.Url
                    }).ToList() ?? new List<ImagenDto>()
                },
                Vendedor = new UsuarioDto
                {
                    Id = publicacion.Usuario.Id,
                    Name = publicacion.Usuario.Name,
                    Email = publicacion.Usuario.Email,
                    Telefono = publicacion.Usuario.Telefono,
                    TipoUsuario = publicacion.Usuario is Persona ? "Persona" : "Empresa"
                }
            };

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
            try
            {
                if (id != dto.Id)
                    return BadRequest("El id no coincide.");

                var publicacion = new Publicacion
                {
                    Id = dto.Id,
                    Titulo = dto.Titulo,
                    Precio = dto.Precio,
                    EsUsado = dto.EsUsado,
                    Estado = dto.Estado,
                    Vehiculo = new Vehiculo
                    {
                        Marca = dto.Marca,
                        Modelo = dto.Modelo,
                        Anio = dto.Anio,
                        Kilometraje = dto.Kilometraje,
                        Combustible = dto.Combustible,
                        Color = dto.Color
                    }
                };

                if (dto.NuevasImagenesBase64 != null && dto.NuevasImagenesBase64.Any())
                {
                    foreach (var imgBase64 in dto.NuevasImagenesBase64)
                    {
                        publicacion.Vehiculo.Imagenes.Add(new ImagenVehiculo { Url = imgBase64 });
                    }
                }

                await _servicio.EditarPublicacionAsync(id, publicacion);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado al editar la publicación.");
            }
        }


        // DELETE: api/publicacion/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _servicio.EliminarPublicacionAsync(id);
            return NoContent();
        }

        // GET: api/publicacion/mis-publicaciones
        [HttpGet("mis-publicaciones")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PublicacionDetalleDto>>> ObtenerMisPublicaciones()
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (userIdClaim == null)
                    return Unauthorized("Token inválido o faltante.");

                int usuarioId = int.Parse(userIdClaim);

                var publicaciones = await _servicio.ObtenerPublicacionesDelUsuarioAsync(usuarioId);

                var dtoList = publicaciones.Select(p => new PublicacionDetalleDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Precio = p.Precio,
                    EsUsado = p.EsUsado,
                    Estado = p.Estado,
                    Fecha = p.Fecha,
                    Vehiculo = new VehiculoDto
                    {
                        Marca = p.Vehiculo?.Marca,
                        Modelo = p.Vehiculo?.Modelo,
                        Anio = p.Vehiculo?.Anio ?? 0,
                        Kilometraje = p.Vehiculo?.Kilometraje ?? 0,
                        Combustible = p.Vehiculo?.Combustible ?? 0,
                        Color = p.Vehiculo?.Color,
                        Imagenes = p.Vehiculo?.Imagenes.Select(i => new ImagenDto
                        {
                            Id = i.Id,
                            Url = i.Url
                        }).ToList() ?? new List<ImagenDto>()
                    },
                    Vendedor = new UsuarioDto
                    {
                        Id = p.Usuario.Id,
                        Name = p.Usuario.Name,
                        Email = p.Usuario.Email,
                        Telefono = p.Usuario.Telefono,
                        TipoUsuario = p.Usuario is Persona ? "Persona" : "Empresa"
                    }
                });

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener tus publicaciones: {ex.Message}");
            }
        }

        // GET: api/publicacion/{publicacionId}/ofertas
        [HttpGet("{publicacionId}/ofertas")]
        [Authorize] 
        public async Task<ActionResult<IEnumerable<OfertaDto>>> ObtenerOfertasDePublicacion(int publicacionId)
        {
            try
            {
                var ofertas = await _servicio.ObtenerOfertasPorPublicacionAsync(publicacionId);

                var dtoList = ofertas.Select(o => new OfertasDePublicacionDTO
                {
                    Id = o.Id,
                    Monto = o.Monto,
                    Fecha = o.Fecha,
                    Estado = o.Estado,
                    Comprador = new UsuarioRespuestaDto
                    {
                        Id = o.CompradorOferta.Id,
                        Nombre = o.CompradorOferta.Name,
                        Email = o.CompradorOferta.Email,
                        Telefono = o.CompradorOferta.Telefono,
                        TipoUsuario = o.CompradorOferta is Persona ? "Persona" : "Empresa"
                    }
                });

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las ofertas: {ex.Message}");
            }
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
                    Imagenes = p.Vehiculo?.Imagenes.Select(i => new ImagenDto
                    {
                        Id = i.Id,
                        Url = i.Url
                    }).ToList() ?? new List<ImagenDto>()
                },
            });



            return Ok(dtoList);
        }
    }
}
