using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.Servicios
{
    public class ServicioPublicacion : IServicioPublicacion
    {
        private readonly IRepositorioPublicacion _repositorioPublicacion;
        private readonly IRepositorioGenerico<Publicacion> _repositorioGenericoPublicacion;
        private readonly IRepositorioGenerico<Usuario> _repositorioGenericoUsuario;

        public ServicioPublicacion(IRepositorioPublicacion repositorioPublicacion, IRepositorioGenerico<Usuario> repositorioGenericoUsuario, IRepositorioGenerico<Publicacion> repositorioGenericoPublicacion)
        {
            _repositorioPublicacion = repositorioPublicacion;
            _repositorioGenericoPublicacion = repositorioGenericoPublicacion;
            _repositorioGenericoUsuario = repositorioGenericoUsuario;
        }
        public async Task CrearPublicacionAsync(Publicacion publicacion)
        {
            if (string.IsNullOrWhiteSpace(publicacion.Titulo))
            {
                throw new ArgumentException("La publicacion debe tener título.");
            }

            if (publicacion.UsuarioId <= 0)
            {
                throw new ArgumentException("Debe especificarse un ID de usuario válido.");
            }

            var usuario = await _repositorioGenericoUsuario.ObtenerPorIdAsync(publicacion.UsuarioId);
            if (usuario == null)
            {
                throw new InvalidOperationException("No existe un usuario con ese ID");
            }

            var publicaciones = await _repositorioGenericoPublicacion.ListarTodosAsync();
            if (publicaciones.Any(p => p.Titulo == publicacion.Titulo && p.UsuarioId == publicacion.UsuarioId))
            {
                throw new InvalidOperationException("Ya existe una publicación con ese título para este usuario.");
            }

            await _repositorioGenericoPublicacion.AgregarAsync(publicacion);

        }

        public async Task EditarPublicacionAsync(int id, Publicacion publicacion)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id invalido para editar publicacion");
            }

            if (publicacion == null)
            {
                throw new ArgumentNullException(nameof(publicacion));
            }

            if (id != publicacion.Id)
            {
                throw new ArgumentException("El Id proporcionado no coincide con el de la publicacion");
            }

            if (string.IsNullOrWhiteSpace(publicacion.Titulo))
            {
                throw new ArgumentNullException("La publicacion debe tener titulo");
            }

            if (publicacion.UsuarioId <= 0)
            {
                throw new ArgumentException("Usuario invalido");
            }

            var publicacionExiste = await _repositorioGenericoPublicacion.ObtenerPorIdAsync(id);
            if(publicacionExiste == null)
            {
                throw new InvalidOperationException($"No existe una publicación con el id: {id}");
            }

            var publicaciones = await _repositorioGenericoPublicacion.ListarTodosAsync();
            if (publicaciones.Any(p => p.Id != publicacion.Id && p.Titulo == publicacion.Titulo && p.UsuarioId == publicacion.UsuarioId))
            {
                throw new InvalidOperationException("Ya existe una publicacion con este titulo para este usuario");
            }

             _repositorioGenericoPublicacion.ModificarAsync(publicacion);
        }

        public async Task EliminarPublicacionAsync(int id)
        {

            if (id <= 0)
            {
                throw new ArgumentException("El id es invalido");
            }

            var publicacion = await _repositorioGenericoPublicacion.ObtenerPorIdAsync(id);

            if (publicacion == null)
            {
                throw new InvalidOperationException($"No existe una aplicacion con el id: {id}");
            }

            _repositorioGenericoPublicacion.EliminarAsync(publicacion);
        }

        public async Task<ICollection<Publicacion>> ObtenerPublicacionesAsync()
        {
            var publicaciones = await _repositorioGenericoPublicacion.ListarTodosAsync();
            return publicaciones ?? new List<Publicacion>();
        }

        public async Task<ICollection<Publicacion>> FiltrarPublicacionesAsync(string marca, string modelo, int? anioDesde, int? anioHasta, decimal? precioDesde, decimal? precioHasta)
        {
            var publicaciones = await _repositorioGenericoPublicacion.ObtenerPorFiltroAsync(p =>
            (string.IsNullOrWhiteSpace(marca) || p.Vehiculo.Marca.Equals(marca, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrWhiteSpace(modelo) || p.Vehiculo.Modelo.Equals(modelo, StringComparison.OrdinalIgnoreCase)) &&
            (!anioDesde.HasValue || p.Vehiculo.Anio >= anioDesde) &&
            (!anioHasta.HasValue || p.Vehiculo.Anio <= anioHasta) &&
            (!precioDesde.HasValue || p.Precio >= precioDesde) &&
            (!precioHasta.HasValue || p.Precio <= precioHasta));

            return publicaciones ?? new List<Publicacion>();
        }
    }
}
