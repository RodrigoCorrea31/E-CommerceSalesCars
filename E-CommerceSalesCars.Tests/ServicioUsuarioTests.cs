using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.Servicios;

namespace E_CommerceSalesCars.Tests.Servicios
{
    public class ServicioUsuarioTests
    {
        private readonly Mock<IRepositorioGenerico<Usuario>> _mockRepoUsuarioGen;
        private readonly Mock<IRepositorioGenerico<Transaccion>> _mockRepoTransGen;
        private readonly Mock<IRepositorioGenerico<Publicacion>> _mockRepoPubGen;
        private readonly Mock<IRepositorioUsuario> _mockRepoUsuario;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly ServicioUsuario _servicio;

        public ServicioUsuarioTests()
        {
            _mockRepoUsuarioGen = new Mock<IRepositorioGenerico<Usuario>>();
            _mockRepoTransGen = new Mock<IRepositorioGenerico<Transaccion>>();
            _mockRepoPubGen = new Mock<IRepositorioGenerico<Publicacion>>();
            _mockRepoUsuario = new Mock<IRepositorioUsuario>();
            _passwordHasher = new PasswordHasher<Usuario>();

            _servicio = new ServicioUsuario(
                _mockRepoUsuarioGen.Object,
                _mockRepoTransGen.Object,
                _mockRepoPubGen.Object,
                _mockRepoUsuario.Object,
                _passwordHasher
            );
        }

        [Fact]
        public async Task RegistrarUsuarioAsync_DeberiaLanzar_SiNombreVacio()
        {
            var act = async () => await _servicio.RegistrarUsuarioAsync("persona", "", "test@mail.com", "123", "1234", "ci", "dir");
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("El nombre es requerido.");
        }

        [Fact]
        public async Task RegistrarUsuarioAsync_DeberiaLanzar_SiEmailYaExiste()
        {
            _mockRepoUsuario.Setup(r => r.ExisteUsuarioConEmailAsync("test@mail.com"))
                .ReturnsAsync(true);

            var act = async () => await _servicio.RegistrarUsuarioAsync("persona", "juan", "test@mail.com", "123", "1234", "ci", "dir");
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Ya existe un usuario registrado con ese email.");
        }

        [Fact]
        public async Task RegistrarUsuarioAsync_DeberiaRegistrarPersonaCorrectamente()
        {
            _mockRepoUsuario.Setup(r => r.ExisteUsuarioConEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            await _servicio.RegistrarUsuarioAsync("persona", "juan", "test@mail.com", "123", "pass", "12345678", "calle");

            _mockRepoUsuarioGen.Verify(r => r.AgregarAsync(It.Is<Persona>(p =>
                p.Email == "test@mail.com" &&
                !string.IsNullOrEmpty(p.ContrasenaHash)
            )), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_DeberiaLanzar_SiUsuarioNoExiste()
        {
            _mockRepoUsuario.Setup(r => r.ObtenerPorEmailAsync("notfound@mail.com"))
                .ReturnsAsync((Usuario)null);

            var act = async () => await _servicio.LoginAsync("notfound@mail.com", "pass");
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Email o contraseña inválidos.");
        }

        [Fact]
        public async Task LoginAsync_DeberiaLoguearCorrectamente()
        {
            var persona = new Persona { Id = 1, Email = "test@mail.com", Name = "Juan" };
            persona.ContrasenaHash = _passwordHasher.HashPassword(persona, "pass");

            _mockRepoUsuario.Setup(r => r.ObtenerPorEmailAsync("test@mail.com"))
                .ReturnsAsync(persona);

            var result = await _servicio.LoginAsync("test@mail.com", "pass");

            result.Should().Be(persona);
        }

        [Fact]
        public async Task ObtenerComprasAsync_DeberiaLanzar_SiUsuarioNoExiste()
        {
            _mockRepoUsuarioGen.Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync((Usuario)null);

            var act = async () => await _servicio.ObtenerComprasAsync(1);
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("No se encontró el usuario con ID: 1");
        }

        [Fact]
        public async Task ComprarVehiculoAsync_DeberiaLanzar_SiMontoMenorPrecio()
        {
            var comprador = new Persona { Id = 1, Email = "comprador@mail.com", ContrasenaHash = "hash" };
            var publicacion = new Publicacion { Id = 10, Precio = 1000, Estado = EstadoPublicacion.Activo, UsuarioId = 2 };

            _mockRepoUsuarioGen.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(comprador);
            _mockRepoPubGen.Setup(r => r.ObtenerPorIdAsync(10)).ReturnsAsync(publicacion);

            var act = async () => await _servicio.ComprarVehiculoAsync(1, 10, 500);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("El monto ofrecido es inferior al precio");
        }

        [Fact]
        public async Task ComprarVehiculoAsync_DeberiaComprarCorrectamente()
        {
            var comprador = new Persona { Id = 1, Email = "comprador@mail.com", ContrasenaHash = "hash" };
            var vendedor = new Persona { Id = 2, Email = "vendedor@mail.com", ContrasenaHash = "hash" };
            var publicacion = new Publicacion { Id = 10, Precio = 1000, Estado = EstadoPublicacion.Activo, UsuarioId = 2 };

            _mockRepoUsuarioGen.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(comprador);
            _mockRepoUsuarioGen.Setup(r => r.ObtenerPorIdAsync(2)).ReturnsAsync(vendedor);
            _mockRepoPubGen.Setup(r => r.ObtenerPorIdAsync(10)).ReturnsAsync(publicacion);

            await _servicio.ComprarVehiculoAsync(1, 10, 1000);

            _mockRepoTransGen.Verify(r => r.AgregarAsync(It.IsAny<Transaccion>()), Times.Once);
            _mockRepoPubGen.Verify(r => r.ActualizarAsync(It.Is<Publicacion>(p => p.Estado == EstadoPublicacion.Pausado)), Times.Once);
        }
    }
}
