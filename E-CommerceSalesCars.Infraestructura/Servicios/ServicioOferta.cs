using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.Servicios
{
    public class ServicioOferta : IServicioOferta
    {
        private readonly IRepositorioOferta _repositorioOferta;
        private readonly IRepositorioGenerico<Oferta> _repositorioGenericoOferta;
        private readonly IRepositorioGenerico<Publicacion> _repositorioGenericoPublicacion;
        private readonly IRepositorioGenerico<Usuario> _repositorioGenericoUsuario;

        public ServicioOferta (IRepositorioOferta repositorioOferta, IRepositorioGenerico<Oferta> repositorioGenericoOferta, IRepositorioGenerico<Publicacion> repositorioGenericoPublicacion, IRepositorioGenerico<Usuario> repositorioGenericoUsuario)
        {
            _repositorioOferta = repositorioOferta;
            _repositorioGenericoOferta = repositorioGenericoOferta;
            _repositorioGenericoPublicacion = repositorioGenericoPublicacion;
            _repositorioGenericoUsuario = repositorioGenericoUsuario;
        }

        public async Task<Oferta> RealizarOfertaAsync(decimal monto, int compradorId, int publicacionId)
        {
            var comprador = await _repositorioGenericoUsuario.ObtenerPorIdAsync(compradorId)
                ?? throw new InvalidOperationException("El usuario no existe.");

            var publicacion = await _repositorioGenericoPublicacion.ObtenerPorIdAsync(publicacionId)
                ?? throw new InvalidOperationException("La publicación no existe.");

            if (publicacion.UsuarioId == compradorId)
                throw new InvalidOperationException("No puedes ofertar en tu propia publicación.");

            if (publicacion.Estado != EstadoPublicacion.Activo)
                throw new InvalidOperationException("La publicación no está activa para recibir ofertas.");

            bool yaOferto = publicacion.Ofertas.Any(o => o.CompradorId == compradorId);
            if (yaOferto)
                throw new InvalidOperationException("Ya realizaste una oferta en esta publicación.");

            var oferta = new Oferta
            {
                Monto = monto,
                Fecha = DateTime.UtcNow,
                Estado = EstadoOferta.Pendiente,
                CompradorId = compradorId,
                PublicacionId = publicacionId
            };

            await _repositorioGenericoOferta.AgregarAsync(oferta);
            return oferta;
        }

        public async Task EliminarOfertaAsync(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("El id es inválido");
            }

            var oferta = await _repositorioGenericoOferta.ObtenerPorIdAsync(id);

            if (oferta == null)
                throw new InvalidOperationException($"No existe una oferta con el id: {id}");

            await _repositorioGenericoOferta.EliminarAsync(oferta);
        }

        public async Task<Oferta> ObtenerPorIdAsync(int id)
        {
            var oferta = await _repositorioGenericoOferta.ObtenerPorIdAsync(id);
            if (oferta == null)
                throw new KeyNotFoundException($"No se encontró una oferta con el ID {id}");

            return oferta;
        }

        public async Task AceptarOfertaAsync(int ofertaId)
        {

            if (ofertaId <= 0)
            {
                throw new ArgumentException("El id de la oferta no puede ser igual o menor a 0.", nameof(ofertaId));
            }

            var ofertaExiste = await _repositorioGenericoOferta.ObtenerPorIdAsync(ofertaId);
            if (ofertaExiste == null)
            {
                throw new InvalidOperationException("La oferta que se intenta aceptar no existe.");    
            }

            await _repositorioOferta.AceptarOfertaAsync(ofertaId);

        }

        public async Task RechazarOfertaAsync(int ofertaId)
        {
            if (ofertaId <= 0)
            {
                throw new ArgumentException("El id de la oferta no puede ser igual o menor a 0.", nameof(ofertaId));
            }

            var ofertaExiste = await _repositorioGenericoOferta.ObtenerPorIdAsync(ofertaId);
            if (ofertaExiste == null)
            {
                throw new InvalidOperationException($"No se encontró ninguna oferta con el ID {ofertaId}.");
            }

            await _repositorioOferta.RechazarOfertaAsync(ofertaId);
        }
    }
}
