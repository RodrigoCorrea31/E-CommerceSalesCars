using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
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
        private readonly IRepositorioGenerico<Usuario> _repositorioGenericoUsuario;

        public ServicioTransaccion (IRepositorioTransaccion repositorioTransaccion, IRepositorioGenerico<Transaccion> repositorioGenericoTransaccion, IRepositorioGenerico<Usuario> repositorioGenericoUsuario)
        {
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioGenericoTransaccion = repositorioGenericoTransaccion;
            _repositorioGenericoUsuario = repositorioGenericoUsuario;
        }

        public async Task FinalizarTransaccionAsync (int transaccionId)
        {
            if (transaccionId <= 0)
            {
                throw new ArgumentException("El id ingresado no  es válido.", nameof(transaccionId));
            }

            var transaccion = await _repositorioGenericoTransaccion.ObtenerPorIdAsync(transaccionId);
            if (transaccion == null)
            {
                throw new InvalidOperationException("La transaccion que se intenta finalizar no existe.");
            }

            if (transaccion.Estado == EstadoTransaccion.Finalizada)
            {
                throw new InvalidOperationException("Esta transaccion ya fue finalizada.");
            }


            transaccion.Estado = EstadoTransaccion.Finalizada;
            transaccion.FechaFinalizacion = DateTime.UtcNow;

            var comprador = await _repositorioGenericoUsuario.ObtenerPorIdAsync(transaccion.CompradorId);
            var vendedor = await _repositorioGenericoUsuario.ObtenerPorIdAsync(transaccion.VendedorId);

            if (comprador == null || vendedor == null)
            {
                throw new InvalidOperationException("No se encontró el comprador o el vendedor.");
            }

            comprador.Compras.Add(transaccion);
            vendedor.Ventas.Add(transaccion);

            await _repositorioGenericoTransaccion.ActualizarAsync(transaccion);
            await _repositorioGenericoUsuario.ActualizarAsync(comprador);
            await _repositorioGenericoUsuario.ActualizarAsync(vendedor);
        }
    }
}
