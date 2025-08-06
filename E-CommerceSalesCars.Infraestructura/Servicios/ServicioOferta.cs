using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public ServicioOferta (IRepositorioOferta repositorioOferta, IRepositorioGenerico<Oferta> repositorioGenericoOferta, IRepositorioGenerico<Publicacion> repositorioGenericoPublicacion, IRepositorioGenerico<Usuario> repositorioGenericoUsuario, ILogger logger)
        {
            _repositorioOferta = repositorioOferta;
            _repositorioGenericoOferta = repositorioGenericoOferta;
            _repositorioGenericoPublicacion = repositorioGenericoPublicacion;
            _repositorioGenericoUsuario = repositorioGenericoUsuario;
            _logger = logger;
        }

        public async Task RealizarOfertaAsync(Oferta oferta)
        {
            if (oferta == null)
            {
                throw new ArgumentNullException (nameof(oferta));
            }

            var usuario = await _repositorioGenericoUsuario.ObtenerPorIdAsync(oferta.CompradorId);
            if(usuario == null)
            {
                throw new InvalidOperationException("El usuario que intenta hacer la oferta no existe.");
            }

            var publicacion = await _repositorioGenericoPublicacion.ObtenerPorIdAsync(oferta.PublicacionId);
            if (publicacion == null)
            {
                throw new InvalidOperationException("La publicacion ya no existe");
            }

            usuario.OfertasRealizadadas.Add(oferta);
            publicacion.Ofertas.Add(oferta);

            await _repositorioGenericoOferta.AgregarAsync(oferta);
            
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
