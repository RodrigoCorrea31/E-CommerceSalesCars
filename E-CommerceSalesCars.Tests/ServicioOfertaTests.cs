using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.Servicios;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace E_CommerceSalesCars.Tests
{
    public class ServicioOfertaTests
    {
        private readonly Mock<IRepositorioOferta> _repoOfertaMock;
        private readonly Mock<IRepositorioGenerico<Oferta>> _repoGenericoOfertaMock;
        private readonly Mock<IRepositorioGenerico<Publicacion>> _repoGenericoPublicacionMock;
        private readonly Mock<IRepositorioGenerico<Usuario>> _repoGenericoUsuarioMock;
        private readonly ServicioOferta _servicio;

        public ServicioOfertaTests()
        {
            _repoOfertaMock = new Mock<IRepositorioOferta>();
            _repoGenericoOfertaMock = new Mock<IRepositorioGenerico<Oferta>>();
            _repoGenericoPublicacionMock = new Mock<IRepositorioGenerico<Publicacion>>();
            _repoGenericoUsuarioMock = new Mock<IRepositorioGenerico<Usuario>>();

            _servicio = new ServicioOferta(
                _repoOfertaMock.Object,
                _repoGenericoOfertaMock.Object,
                _repoGenericoPublicacionMock.Object,
                _repoGenericoUsuarioMock.Object
            );
        }

        [Fact]
        public async Task RealizarOfertaAsync_CuandoDatosValidos_AgregaOferta()
        {
            // Arrange
            decimal monto = 5000m;
            int compradorId = 1;
            int publicacionId = 2;

            var usuario = new Persona { Id = compradorId, OfertasRealizadadas = new List<Oferta>() };
            var publicacion = new Publicacion { Id = publicacionId, Ofertas = new List<Oferta>(), UsuarioId = 99, Estado = EstadoPublicacion.Activo };

            _repoGenericoUsuarioMock.Setup(r => r.ObtenerPorIdAsync(compradorId)).ReturnsAsync(usuario);
            _repoGenericoPublicacionMock.Setup(r => r.ObtenerPorIdAsync(publicacionId)).ReturnsAsync(publicacion);

            // Act
            var resultado = await _servicio.RealizarOfertaAsync(monto, compradorId, publicacionId);

            // Assert
            resultado.Monto.Should().Be(monto);
            resultado.CompradorId.Should().Be(compradorId);
            resultado.PublicacionId.Should().Be(publicacionId);

            _repoGenericoOfertaMock.Verify(r => r.AgregarAsync(It.IsAny<Oferta>()), Times.Once);
        }

        [Fact]
        public async Task RealizarOfertaAsync_CuandoUsuarioNoExiste_LanzaExcepcion()
        {
            // Arrange
            decimal monto = 5000m;
            int compradorId = 99;
            int publicacionId = 2;

            _repoGenericoUsuarioMock.Setup(r => r.ObtenerPorIdAsync(compradorId))
                                    .ReturnsAsync((Usuario)null);

            // Act
            Func<Task> act = async () => await _servicio.RealizarOfertaAsync(monto, compradorId, publicacionId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("El usuario no existe.");
        }

        [Fact]
        public async Task RealizarOfertaAsync_CuandoPublicacionNoExiste_LanzaExcepcion()
        {
            // Arrange
            decimal monto = 5000m;
            int compradorId = 1;
            int publicacionId = 99;

            var usuario = new Persona { Id = compradorId, OfertasRealizadadas = new List<Oferta>() };

            _repoGenericoUsuarioMock.Setup(r => r.ObtenerPorIdAsync(compradorId)).ReturnsAsync(usuario);
            _repoGenericoPublicacionMock.Setup(r => r.ObtenerPorIdAsync(publicacionId))
                                        .ReturnsAsync((Publicacion)null);

            // Act
            Func<Task> act = async () => await _servicio.RealizarOfertaAsync(monto, compradorId, publicacionId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("La publicación no existe.");
        }

        [Fact]
        public async Task AceptarOfertaAsync_CuandoOfertaExiste_LlamaRepositorio()
        {
            // Arrange
            var ofertaId = 10;
            var oferta = new Oferta { Id = ofertaId };
            _repoGenericoOfertaMock.Setup(r => r.ObtenerPorIdAsync(ofertaId)).ReturnsAsync(oferta);

            // Act
            await _servicio.AceptarOfertaAsync(ofertaId);

            // Assert
            _repoOfertaMock.Verify(r => r.AceptarOfertaAsync(ofertaId), Times.Once);
        }

        [Fact]
        public async Task RechazarOfertaAsync_CuandoNoExiste_LanzaExcepcion()
        {
            // Arrange
            int ofertaId = 999;
            _repoGenericoOfertaMock.Setup(r => r.ObtenerPorIdAsync(ofertaId))
                                   .ReturnsAsync((Oferta)null);

            // Act
            Func<Task> act = async () => await _servicio.RechazarOfertaAsync(ofertaId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage($"No se encontró ninguna oferta con el ID {ofertaId}.");
        }
    }
}
