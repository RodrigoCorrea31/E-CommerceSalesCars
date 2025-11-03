using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.Servicios
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly IRepositorioGenerico<Usuario> _repositorioGenericoUsuario;
        private readonly IRepositorioGenerico<Transaccion> _repositorioGenericoTransaccion;
        private readonly IRepositorioGenerico<Publicacion> _repositorioGenericoPublicacion;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public ServicioUsuario (IRepositorioGenerico<Usuario> repositorioGenericoUsuario, IRepositorioGenerico<Transaccion> repositorioGenericoTransaccion, IRepositorioGenerico<Publicacion> repositorioGenericoPublicacion, IRepositorioUsuario repositorioUsuario, IPasswordHasher<Usuario> passwordHasher)
        {
            _repositorioGenericoUsuario = repositorioGenericoUsuario;
            _repositorioGenericoTransaccion = repositorioGenericoTransaccion;
            _repositorioGenericoPublicacion = repositorioGenericoPublicacion;
            _repositorioUsuario = repositorioUsuario;
            _passwordHasher = passwordHasher;
        }

        public async Task RegistrarUsuarioAsync(string tipoUsuario, string nombre, string email, string telefono, string contrasena, string datoExtra1, string datoExtra2)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es requerido.");
            if (string.IsNullOrWhiteSpace(contrasena))
                throw new ArgumentException("La contraseña es requerida.");

            bool existe = await _repositorioUsuario.ExisteUsuarioConEmailAsync(email);
            if (existe)
                throw new InvalidOperationException("Ya existe un usuario registrado con ese email.");

            Usuario nuevoUsuario;

            if (tipoUsuario.ToLower() == "persona")
            {
                if (string.IsNullOrWhiteSpace(datoExtra1))
                    throw new ArgumentException("La cédula es requerida.");
                if (string.IsNullOrWhiteSpace(datoExtra2))
                    throw new ArgumentException("La dirección es requerida.");

                nuevoUsuario = new Persona
                {
                    Name = nombre,
                    Email = email,
                    Telefono = telefono,
                    CI = datoExtra1,
                    Direccion = datoExtra2
                };
            }
            else if (tipoUsuario.ToLower() == "empresa")
            {
                if (string.IsNullOrWhiteSpace(datoExtra1))
                    throw new ArgumentException("El RUT es requerido.");
                if (string.IsNullOrWhiteSpace(datoExtra2))
                    throw new ArgumentException("La razón social es requerida.");

                nuevoUsuario = new Empresa
                {
                    Name = nombre,
                    Email = email,
                    Telefono = telefono,
                    RUT = datoExtra1,
                    RazonSocial = datoExtra2
                };
            }
            else
            {
                throw new ArgumentException("Tipo de usuario no válido. Debe ser 'persona' o 'empresa'.");
            }

            var hasher = new PasswordHasher<Usuario>();
            nuevoUsuario.ContrasenaHash = hasher.HashPassword(nuevoUsuario, contrasena);

            await _repositorioGenericoUsuario.AgregarAsync(nuevoUsuario);
        }


        public async Task<Usuario> LoginAsync(string email, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email), "El email no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(contrasena))
            {
                throw new ArgumentNullException(nameof(contrasena), "La contraseña no puede estar vacía.");
            }

            var usuario = await _repositorioUsuario.ObtenerPorEmailAsync(email);
            if (usuario == null)
            {
                throw new InvalidOperationException("Email o contraseña inválidos.");
            }

            var hasher = new PasswordHasher<Usuario>();
            var resultado = hasher.VerifyHashedPassword(usuario, usuario.ContrasenaHash, contrasena);

            if (resultado == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Email o contraseña inválidos.");
            }

            return usuario;
        }
        public async Task<Usuario> ObtenerPorIdAsync(int id)
        {
            var usuario = await _repositorioGenericoUsuario.ObtenerPorIdAsync(id);
            return usuario;
        }

        public async Task<ICollection<Oferta>> ObtenerOfertasRealizadasAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentOutOfRangeException("El id del usuario debe ser mayor a 0.", nameof(usuarioId));
            }

            var usuario = await _repositorioGenericoUsuario.ObtenerPorIdAsync(usuarioId);
            if (usuario == null)
            {
                throw new InvalidOperationException($"No se encontró ningun usuario con el ID: {usuarioId}.");
            }

            var ofertas = await _repositorioUsuario.ObtenerOfertasRealizadasAsync(usuarioId);
            return ofertas ?? new List<Oferta>();
        }

        public async Task<ICollection<Transaccion>> ObtenerComprasAsync(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentOutOfRangeException(nameof(usuarioId));

            var usuario = await _repositorioGenericoUsuario.ObtenerPorIdAsync(usuarioId);
            if (usuario == null)
                throw new InvalidOperationException($"No se encontró el usuario con ID {usuarioId}");

            return await _repositorioUsuario.ObtenerComprasAsync(usuarioId);
        }

        public async Task<ICollection<Transaccion>> ObtenerVentasAsync(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentOutOfRangeException(nameof(usuarioId));

            var usuario = await _repositorioGenericoUsuario.ObtenerPorIdAsync(usuarioId);
            if (usuario == null)
                throw new InvalidOperationException($"No se encontró el usuario con ID {usuarioId}");

            return await _repositorioUsuario.ObtenerVentasAsync(usuarioId);
        }


        public async Task ComprarVehiculoAsync(int usuarioId, int publicacionId, decimal monto)
        {
            if (usuarioId <= 0 || publicacionId <= 0)
            {
                throw new ArgumentException("IDs inválidos.");
            }

            var comprador = await _repositorioGenericoUsuario.ObtenerPorIdAsync(usuarioId);
            if (comprador == null)
            {
                throw new InvalidOperationException("Comprador no encontrado");
            }

            var publicacion = await _repositorioGenericoPublicacion.ObtenerPorIdAsync(publicacionId);
            if (publicacion == null)
            {
                throw new InvalidOperationException("Publicación no encontrada");
            }

            if (publicacion.Estado != EstadoPublicacion.Activo)
            {
                throw new InvalidOperationException("La publicación no está disponible");
            }

            if (monto < publicacion.Precio)
            {
                throw new InvalidOperationException("El monto ofrecido es inferior al precio");
            }

            var vendedor = await _repositorioGenericoUsuario.ObtenerPorIdAsync(publicacion.UsuarioId);
            if (vendedor == null)
            {
                throw new InvalidOperationException("Vendedor no encontrado");
            }               

            var transaccion = new Transaccion
            {
                Fecha = DateTime.UtcNow,
                FechaFinalizacion = DateTime.MinValue,
                PrecioVenta = monto,
                Observacion = "Compra iniciada",
                Estado = EstadoTransaccion.Pendiente,
                MetodoDePago = "A convenir",
                CompradorId = comprador.Id,
                VendedorId = vendedor.Id,
                PublicacionId = publicacion.Id
            };

            publicacion.Estado = EstadoPublicacion.Pausado;

            await _repositorioGenericoTransaccion.AgregarAsync(transaccion);
            await _repositorioGenericoPublicacion.ActualizarAsync(publicacion);
        }

    }
}
