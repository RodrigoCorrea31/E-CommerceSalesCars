using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.Servicios
{
    public class ServicioTransaccion : IServicioTransaccion
    {
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        private readonly IRepositorioGenerico<Transaccion> _repositorioGenericoTransaccion;
        private readonly ILogger _logger;

        public ServicioTransaccion (IRepositorioTransaccion repositorioTransaccion, IRepositorioGenerico<Transaccion> repositorioGenericoTransaccion, ILogger logger)
        {
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioGenericoTransaccion = repositorioGenericoTransaccion;
            _logger = logger;
        }

        public async Task FinalizarTransaccionAsync (int transaccionId)
        {
            if (transaccionId <= 0)
            {
                throw new ArgumentException("El id ingresado no  es válido.", nameof(transaccionId));
            }

            var transaccionExiste = await _repositorioGenericoTransaccion.ObtenerPorIdAsync(transaccionId);
            if (transaccionExiste == null)
            {
                throw new InvalidOperationException("La transaccion que se intenta finalizar no existe.");
            }

            if (transaccionExiste.Estado == EstadoTransaccion.Finalizada)
            {
                throw new InvalidOperationException("Esta transaccion ya fue finalizada.");
            }

            await _repositorioTransaccion.FinalizarTransaccionAsync(transaccionId);

            _logger.LogInformation("Finalizada la transaccion.");
        }
    }
}
