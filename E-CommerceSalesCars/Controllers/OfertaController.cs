using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.DTOs.OfertaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSalesCars.Controllers
{
    [Route("api/ofertas")]
    [ApiController]
    public class OfertaController : ControllerBase
    {
        private readonly IServicioOferta _servicioOferta;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OfertaController(IServicioOferta servicioOferta, IHttpContextAccessor httpContextAccessor)
        {
            _servicioOferta = servicioOferta;
            _httpContextAccessor = httpContextAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        // POST /api/ofertas
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CrearOferta([FromBody] CrearOfertaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("id");
            if (userIdClaim == null)
                return Unauthorized();

            int compradorId = int.Parse(userIdClaim.Value);

            try
            {
                var nuevaOferta = await _servicioOferta.RealizarOfertaAsync(dto.Monto, compradorId, dto.PublicacionId);

                var respuesta = new OfertaRespuestaDTO
                {
                    Id = nuevaOferta.Id,
                    Monto = nuevaOferta.Monto,
                    Fecha = nuevaOferta.Fecha,
                    Estado = nuevaOferta.Estado.ToString(),
                    CompradorId = nuevaOferta.CompradorId,
                    PublicacionId = nuevaOferta.PublicacionId
                };

                return CreatedAtAction(nameof(CrearOferta), new { id = respuesta.Id }, respuesta);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Oferta>> ObtenerPorId(int id)
        {
            try
            {
                var oferta = await _servicioOferta.ObtenerPorIdAsync(id);
                return Ok(oferta);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // POST /api/ofertas/{id}/aceptar
        [HttpPost("{id}/aceptar")]
        public async Task<IActionResult> AceptarOferta(int id)
        {
            try
            {
                await _servicioOferta.AceptarOfertaAsync(id);
                return Ok(new { mensaje = "Oferta aceptada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST /api/ofertas/{id}/rechazar
        [HttpPost("{id}/rechazar")]
        public async Task<IActionResult> RechazarOferta(int id)
        {
            try
            {
                await _servicioOferta.RechazarOfertaAsync(id);
                return Ok(new { mensaje = "Oferta rechazada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
