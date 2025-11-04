using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO;
using E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO.E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO;
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
        private readonly IRepositorioGenerico<Vehiculo> _repositorioGenericoVehiculo;

        public ServicioPublicacion(IRepositorioPublicacion repositorioPublicacion, IRepositorioGenerico<Usuario> repositorioGenericoUsuario, IRepositorioGenerico<Publicacion> repositorioGenericoPublicacion, IRepositorioGenerico<Vehiculo> repositorioGenericoVehiculo)
        {
            _repositorioPublicacion = repositorioPublicacion;
            _repositorioGenericoPublicacion = repositorioGenericoPublicacion;
            _repositorioGenericoUsuario = repositorioGenericoUsuario;
            _repositorioGenericoVehiculo = repositorioGenericoVehiculo;
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
                throw new ArgumentException("Id inválido para editar publicación.");

            if (publicacion == null)
                throw new ArgumentNullException(nameof(publicacion));

            if (id != publicacion.Id)
                throw new ArgumentException("El Id proporcionado no coincide con el de la publicación.");

            var publicaciones = await _repositorioPublicacion.ObtenerPublicacionesConVehiculoAsync();
            var existente = publicaciones.FirstOrDefault(p => p.Id == id);

            if (existente == null)
                throw new InvalidOperationException($"No existe una publicación con el id: {id}");

            var duplicado = publicaciones.Any(p =>
                p.Id != id &&
                p.Titulo == publicacion.Titulo &&
                p.UsuarioId == existente.UsuarioId);

            if (duplicado)
                throw new InvalidOperationException("Ya existe una publicación con este título para este usuario.");

            existente.Titulo = publicacion.Titulo;
            existente.Precio = publicacion.Precio;
            existente.EsUsado = publicacion.EsUsado;
            existente.Estado = publicacion.Estado;

            var vehiculo = existente.Vehiculo;
            vehiculo.Marca = publicacion.Vehiculo.Marca;
            vehiculo.Modelo = publicacion.Vehiculo.Modelo;
            vehiculo.Anio = publicacion.Vehiculo.Anio;
            vehiculo.Kilometraje = publicacion.Vehiculo.Kilometraje;
            vehiculo.Combustible = publicacion.Vehiculo.Combustible;
            vehiculo.Color = publicacion.Vehiculo.Color;

            if (publicacion.Vehiculo.Imagenes?.Any() == true)
            {
                foreach (var img in publicacion.Vehiculo.Imagenes)
                {
                    vehiculo.Imagenes.Add(img);
                }
            }

            await _repositorioGenericoVehiculo.ActualizarAsync(vehiculo);
            await _repositorioGenericoPublicacion.ActualizarAsync(existente);
        }

        public async Task EliminarPublicacionAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El id es inválido");

            var publicacion = await _repositorioGenericoPublicacion.ObtenerPorIdAsync(id);

            if (publicacion == null)
                throw new InvalidOperationException($"No existe una publicación con el id: {id}");

            var vehiculo = await _repositorioGenericoVehiculo.ObtenerPorIdAsync(publicacion.VehiculoId);
            if (vehiculo != null)
            {
                await _repositorioGenericoVehiculo.EliminarAsync(vehiculo);
            }

        }


        public async Task<ICollection<Publicacion>> ObtenerPublicacionesAsync()
        {
            var publicaciones = await _repositorioPublicacion.ObtenerPublicacionesConVehiculoAsync();
            return publicaciones ?? new List<Publicacion>();
        }

        public async Task<Publicacion?> ObtenerDetallePorIdAsync(int id)
        {
            return await _repositorioPublicacion.ObtenerPublicacionConDetallesPorIdAsync(id);
        }

        public async Task<ICollection<Publicacion>> ObtenerPublicacionesDelUsuarioAsync(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("Id de usuario inválido.");

            var publicaciones = await _repositorioPublicacion.ObtenerPublicacionesPorUsuarioAsync(usuarioId);
            return publicaciones ?? new List<Publicacion>();
        }

        public async Task<IEnumerable<Oferta>> ObtenerOfertasPorPublicacionAsync(int publicacionId)
        {
            if (publicacionId <= 0)
                throw new ArgumentException("El ID de la publicación no es válido.");

            var ofertas = await _repositorioPublicacion.ObtenerOfertasPorPublicacionAsync(publicacionId);

            return ofertas ?? new List<Oferta>();
        }

        public async Task<ICollection<Publicacion>> FiltrarPublicacionesAsync(string marca, string modelo, int? anioDesde, int? anioHasta, decimal? precioDesde, decimal? precioHasta)
        {
            marca = marca?.ToLower();
            modelo = modelo?.ToLower();

            var publicaciones = await _repositorioPublicacion.FiltrarPublicacionesAsync(p =>
                (string.IsNullOrWhiteSpace(marca) || p.Vehiculo.Marca.ToLower() == marca) &&
                (string.IsNullOrWhiteSpace(modelo) || p.Vehiculo.Modelo.ToLower() == modelo) &&
                (!anioDesde.HasValue || p.Vehiculo.Anio >= anioDesde) &&
                (!anioHasta.HasValue || p.Vehiculo.Anio <= anioHasta) &&
                (!precioDesde.HasValue || p.Precio >= precioDesde) &&
                (!precioHasta.HasValue || p.Precio <= precioHasta));

            return publicaciones ?? new List<Publicacion>();
        }

    }
}
