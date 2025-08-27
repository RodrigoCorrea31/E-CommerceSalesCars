using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.Servicios;

namespace E_CommerceSalesCars.Tests.Servicios
{
    public class ServicioTransaccionTests
    {
        private readonly Mock<IRepositorioTransaccion> _mockRepoTransaccion;
        private readonly Mock<IRepositorioGenerico<Transaccion>> _mockRepoGenericoTransaccion;
        private readonly Mock<IRepositorioGenerico<Usuario>> _mockRepoGenericoUsuario;
        private readonly ServicioTransaccion _servicio;

        public ServicioTransaccionTests()
        {
            _mockRepoTransaccion = new Mock<IRepositorioTransaccion>();
            _mockRepoGenericoTransaccion = new Mock<IRepositorioGenerico<Transaccion>>();
            _mockRepoGenericoUsuario = new Mock<IRepositorioGenerico<Usuario>>();

            _servicio = new ServicioTransaccion(
                _mockRepoTransaccion.Object,
                _mockRepoGenericoTransaccion.Object,
                _mockRepoGenericoUsuario.Object
            );
        }

        [Fact]
        public async Task FinalizarTransaccionAsync_DeberiaLanzarExcepcion_SiIdInvalido()
        {
            var act = async () => await _servicio.FinalizarTransaccionAsync(0);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("El id ingresado no  es válido.*");
        }

        [Fact]
        public async Task FinalizarTransaccionAsync_DeberiaLanzarExcepcion_SiTransaccionNoExiste()
        {
            _mockRepoGenericoTransaccion
                .Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Transaccion)null);

            var act = async () => await _servicio.FinalizarTransaccionAsync(1);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("La transaccion que se intenta finalizar no existe.");
        }

        [Fact]
        public async Task FinalizarTransaccionAsync_DeberiaLanzarExcepcion_SiTransaccionYaFinalizada()
        {
            var transaccion = new Transaccion
            {
                Id = 1,
                Estado = EstadoTransaccion.Finalizada
            };

            _mockRepoGenericoTransaccion
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(transaccion);

            var act = async () => await _servicio.FinalizarTransaccionAsync(1);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Esta transaccion ya fue finalizada.");
        }

        [Fact]
        public async Task FinalizarTransaccionAsync_DeberiaLanzarExcepcion_SiCompradorOVendedorNoExisten()
        {
            var transaccion = new Transaccion
            {
                Id = 1,
                Estado = EstadoTransaccion.Pendiente,
                CompradorId = 10,
                VendedorId = 20
            };

            _mockRepoGenericoTransaccion
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(transaccion);

            _mockRepoGenericoUsuario
                .Setup(r => r.ObtenerPorIdAsync(10))
                .ReturnsAsync((Usuario)null);

            var act = async () => await _servicio.FinalizarTransaccionAsync(1);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("No se encontró el comprador o el vendedor.");
        }

        [Fact]
        public async Task FinalizarTransaccionAsync_DeberiaFinalizarTransaccionCorrectamente()
        {
            var comprador = new Persona { Id = 10, ContrasenaHash = "hash" };
            var vendedor = new Persona { Id = 20, ContrasenaHash = "hash" };
            var transaccion = new Transaccion
            {
                Id = 1,
                Estado = EstadoTransaccion.Pendiente,
                CompradorId = comprador.Id,
                VendedorId = vendedor.Id,
                Fecha = DateTime.UtcNow
            };

            _mockRepoGenericoTransaccion
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(transaccion);

            _mockRepoGenericoUsuario
                .Setup(r => r.ObtenerPorIdAsync(comprador.Id))
                .ReturnsAsync(comprador);

            _mockRepoGenericoUsuario
                .Setup(r => r.ObtenerPorIdAsync(vendedor.Id))
                .ReturnsAsync(vendedor);

            await _servicio.FinalizarTransaccionAsync(1);

            transaccion.Estado.Should().Be(EstadoTransaccion.Finalizada);
            transaccion.FechaFinalizacion.Should().NotBe(default);

            comprador.Compras.Should().Contain(transaccion);
            vendedor.Ventas.Should().Contain(transaccion);

            _mockRepoGenericoTransaccion.Verify(r => r.ActualizarAsync(transaccion), Times.Once);
            _mockRepoGenericoUsuario.Verify(r => r.ActualizarAsync(comprador), Times.Once);
            _mockRepoGenericoUsuario.Verify(r => r.ActualizarAsync(vendedor), Times.Once);
        }
    }
}
