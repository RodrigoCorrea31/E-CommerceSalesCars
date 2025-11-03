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

        [HttpPost("{ofertaId}/finalizar")]
        public async Task<ActionResult<TransaccionRespuestaDTO>> FinalizarTransaccion(int ofertaId)
        {
            try
            {
                var transaccion = await _servicioTransaccion.FinalizarTransaccionAsync(ofertaId);

                transaccion.MetodoDePago ??= "No especificado";

                var dto = new TransaccionRespuestaDTO
                {
                    Id = transaccion.Id,
                    Fecha = transaccion.Fecha,
                    FechaFinalizacion = transaccion.FechaFinalizacion,
                    PrecioVenta = transaccion.PrecioVenta,
                    Estado = transaccion.Estado.ToString(),
                    MetodoDePago = transaccion.MetodoDePago,
                    Observacion = transaccion.Observacion,
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
