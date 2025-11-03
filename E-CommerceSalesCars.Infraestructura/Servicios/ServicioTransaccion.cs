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
        private readonly IRepositorioGenerico<Oferta> _repositorioGenericoOferta;
        private readonly IRepositorioGenerico<Publicacion> _repositorioGenericoPublicacion;

        public ServicioTransaccion (IRepositorioTransaccion repositorioTransaccion, IRepositorioGenerico<Transaccion> repositorioGenericoTransaccion, IRepositorioGenerico<Usuario> repositorioGenericoUsuario, IRepositorioGenerico<Oferta> repositorioGenericoOferta, IRepositorioGenerico<Publicacion> repositorioGenericoPublicacion)
        {
            _repositorioTransaccion = repositorioTransaccion;
            _repositorioGenericoTransaccion = repositorioGenericoTransaccion;
            _repositorioGenericoUsuario = repositorioGenericoUsuario;
            _repositorioGenericoOferta = repositorioGenericoOferta;
            _repositorioGenericoPublicacion = repositorioGenericoPublicacion;
        }

        public async Task<Transaccion> FinalizarTransaccionAsync(int ofertaId)
        {
            if (ofertaId <= 0)
                throw new ArgumentException("El id de la oferta no es válido.", nameof(ofertaId));

            var oferta = await _repositorioGenericoOferta.ObtenerPorIdAsync(ofertaId);
            if (oferta == null)
                throw new InvalidOperationException("La oferta no existe.");

            var publicacion = await _repositorioGenericoPublicacion.ObtenerPorIdAsync(oferta.PublicacionId);
            if (publicacion == null)
                throw new InvalidOperationException("La publicación asociada a la oferta no existe.");

            if (publicacion.Estado != EstadoPublicacion.Activo)
                throw new InvalidOperationException("La publicación ya no está disponible.");

            oferta.Estado = EstadoOferta.Aceptada;
            publicacion.Estado = EstadoPublicacion.Vendido;

            var otrasOfertas = await _repositorioGenericoOferta.ObtenerPorFiltroAsync(o =>
                o.PublicacionId == publicacion.Id && o.Id != oferta.Id);
            foreach (var o in otrasOfertas)
                o.Estado = EstadoOferta.Rechazada;

            var transaccion = new Transaccion
            {
                PublicacionId = publicacion.Id,
                CompradorId = oferta.CompradorId,
                VendedorId = publicacion.UsuarioId,
                PrecioVenta = oferta.Monto,
                Fecha = DateTime.UtcNow,
                Estado = EstadoTransaccion.Finalizada,
                MetodoDePago = "No especificado",
                Observacion = "Sin observaciones"
            };

            await _repositorioGenericoTransaccion.AgregarAsync(transaccion);

            var comprador = await _repositorioGenericoUsuario.ObtenerPorIdAsync(transaccion.CompradorId);
            var vendedor = await _repositorioGenericoUsuario.ObtenerPorIdAsync(transaccion.VendedorId);

            await _repositorioGenericoUsuario.ActualizarAsync(comprador);
            await _repositorioGenericoUsuario.ActualizarAsync(vendedor);
            await _repositorioGenericoPublicacion.ActualizarAsync(publicacion);

            return transaccion;
        }


    }
}
