using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.DTOs.TransaccionDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSalesCars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionController : ControllerBase
    {
        private readonly IServicioTransaccion _servicioTransaccion;
        private readonly IRepositorioGenerico<Transaccion> _repositorioTransaccion;

        public TransaccionController(IServicioTransaccion servicioTransaccion,
                                     IRepositorioGenerico<Transaccion> repositorioTransaccion)
        {
            _servicioTransaccion = servicioTransaccion;
            _repositorioTransaccion = repositorioTransaccion;
        }

        // POST /api/transaccion/{id}/finalizar
        [HttpPost("{id}/finalizar")]
        public async Task<ActionResult<TransaccionRespuestaDTO>> FinalizarTransaccion(int id)
        {
            try
            {
                await _servicioTransaccion.FinalizarTransaccionAsync(id);

                var transaccion = await _repositorioTransaccion.ObtenerPorIdAsync(id);
                if (transaccion == null)
                    return NotFound("No se encontró la transacción.");

                var dto = new TransaccionRespuestaDTO
                {
                    Id = transaccion.Id,
                    Fecha = transaccion.Fecha,
                    FechaFinalizacion = transaccion.FechaFinalizacion,
                    PrecioVenta = transaccion.PrecioVenta,
                    Observacion = transaccion.Observacion,
                    Estado = transaccion.Estado.ToString(),
                    MetodoDePago = transaccion.MetodoDePago,
                    CompradorId = transaccion.CompradorId,
                    VendedorId = transaccion.VendedorId,
                    PublicacionId = transaccion.PublicacionId
                };

                return Ok(dto);
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
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}
