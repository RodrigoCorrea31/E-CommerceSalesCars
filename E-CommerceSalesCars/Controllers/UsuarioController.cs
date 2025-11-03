using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.AuthJWT;
using E_CommerceSalesCars.Infraestructura.DTOs.OfertaDTO;
using E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO.E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO;
using E_CommerceSalesCars.Infraestructura.DTOs.TransaccionDTO;
using E_CommerceSalesCars.Infraestructura.DTOs.UsuarioDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSalesCars.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IServicioUsuario _servicioUsuario;
        private readonly IJwtServicio _jwtServicio;

        public UsuarioController(IServicioUsuario servicioUsuario, IJwtServicio jwtServicio)
        {
            _servicioUsuario = servicioUsuario;
            _jwtServicio = jwtServicio;
        }

        // POST: api/usuarios/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioDto dto)
        {
            await _servicioUsuario.RegistrarUsuarioAsync(
                dto.TipoUsuario, dto.Nombre, dto.Email, dto.Telefono, dto.Contrasena, dto.DatoExtra1, dto.DatoExtra2
            );

            return Ok(new { mensaje = "Usuario registrado con éxito" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioDto dto)
        {
            try
            {
                var usuario = await _servicioUsuario.LoginAsync(dto.Email, dto.Contrasena);

                var usuarioJwt = new UsuarioJwtDTO
                {
                    Id = usuario.Id,
                    NombreUsuario = usuario.Name,
                    Correo = usuario.Email
                };

                var token = _jwtServicio.GenerarTokenJwt(usuarioJwt);

                return Ok(new
                {
                    token,
                    usuario = new UsuarioRespuestaDto
                    {
                        Id = usuario.Id,
                        Nombre = usuario.Name,
                        Email = usuario.Email,
                        Telefono = usuario.Telefono,
                        TipoUsuario = usuario is Persona ? "Persona" : "Empresa"
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error inesperado", detalle = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuarioById(int id)
        {
            var usuario = await _servicioUsuario.ObtenerPorIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            var dto = new UsuarioDto
            {
                Id = usuario.Id,
                Name = usuario.Name,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                TipoUsuario = usuario is Empresa ? "Empresa" : "Persona"
            };

            return Ok(dto);
        }

        // GET: api/usuarios/{id}/ofertas
        [HttpGet("{id}/ofertas")]
        public async Task<ActionResult<IEnumerable<OfertaDto>>> ObtenerOfertas(int id)
        {
            var ofertas = await _servicioUsuario.ObtenerOfertasRealizadasAsync(id);

            var dto = ofertas.Select(o => new OfertaDto
            {
                Id = o.Id,
                Monto = o.Monto,
                Fecha = o.Fecha,
                Estado = o.Estado.ToString(),
                PublicacionId = o.Publicacion.Id,
                TituloPublicacion = o.Publicacion.Titulo
            });

            return Ok(dto);
        }


        // GET: api/usuarios/{id}/compras
        [HttpGet("{id}/compras")]
        public async Task<ActionResult<IEnumerable<TransaccionDto>>> ObtenerCompras(int id)
        {
            var compras = await _servicioUsuario.ObtenerComprasAsync(id);

            var dto = compras.Select(c => new TransaccionDto
            {
                Id = c.Id,
                Fecha = c.Fecha,
                PrecioVenta = c.PrecioVenta,
                Estado = c.Estado.ToString(),
                MetodoDePago = c.MetodoDePago,
                VendedorNombre = c.Vendedor?.Name,
                PublicacionTitulo = c.Publicacion?.Titulo
            });

            return Ok(dto);
        }

        // GET: api/usuarios/{id}/ventas
        [HttpGet("{id}/ventas")]
        public async Task<ActionResult<IEnumerable<TransaccionDto>>> ObtenerVentas(int id)
        {
            var ventas = await _servicioUsuario.ObtenerVentasAsync(id);

            var dto = ventas.Select(v => new TransaccionDto
            {
                Id = v.Id,
                Fecha = v.Fecha,
                PrecioVenta = v.PrecioVenta,
                Estado = v.Estado.ToString(),
                MetodoDePago = v.MetodoDePago,
                CompradorNombre = v.CompradorTransaccion?.Name,
                PublicacionTitulo = v.Publicacion?.Titulo
            });

            return Ok(dto);
        }

    }
}
