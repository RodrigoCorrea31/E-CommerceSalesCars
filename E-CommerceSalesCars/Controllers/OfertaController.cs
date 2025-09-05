using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.DTOs.OfertaDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSalesCars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaController : ControllerBase
    {
        private readonly IServicioOferta _servicioOferta;

        public OfertaController(IServicioOferta servicioOferta)
        {
            _servicioOferta = servicioOferta;
        }

        // POST /api/ofertas
        [HttpPost]
        public async Task<IActionResult> CrearOferta([FromBody] CrearOfertaDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevaOferta = new Oferta
            {
                Monto = dto.Monto,
                Fecha = DateTime.UtcNow,
                Estado = EstadoOferta.Pendiente,
                CompradorId = dto.CompradorId,
                PublicacionId = dto.PublicacionId
            };

            await _servicioOferta.RealizarOfertaAsync(nuevaOferta);

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
