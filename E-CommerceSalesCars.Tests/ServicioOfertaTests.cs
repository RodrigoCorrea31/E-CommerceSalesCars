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
            var oferta = new Oferta { CompradorId = 1, PublicacionId = 2 };
            var usuario = new Persona { Id = 1, OfertasRealizadadas = new List<Oferta>() };
            var publicacion = new Publicacion { Id = 2, Ofertas = new List<Oferta>() };

            _repoGenericoUsuarioMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);
            _repoGenericoPublicacionMock.Setup(r => r.ObtenerPorIdAsync(2)).ReturnsAsync(publicacion);

            await _servicio.RealizarOfertaAsync(oferta);

            usuario.OfertasRealizadadas.Should().Contain(oferta);
            publicacion.Ofertas.Should().Contain(oferta);
            _repoGenericoOfertaMock.Verify(r => r.AgregarAsync(oferta), Times.Once);
        }

        [Fact]
        public async Task RealizarOfertaAsync_CuandoUsuarioNoExiste_LanzaExcepcion()
        {
            var oferta = new Oferta { CompradorId = 99, PublicacionId = 2 };
            _repoGenericoUsuarioMock.Setup(r => r.ObtenerPorIdAsync(99))
                                    .ReturnsAsync((Usuario)null);

            Func<Task> act = async () => await _servicio.RealizarOfertaAsync(oferta);

            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("El usuario que intenta hacer la oferta no existe.");
        }

        [Fact]
        public async Task RealizarOfertaAsync_CuandoPublicacionNoExiste_LanzaExcepcion()
        {
            var oferta = new Oferta { CompradorId = 1, PublicacionId = 99 };
            var usuario = new Persona { Id = 1, OfertasRealizadadas = new List<Oferta>() };

            _repoGenericoUsuarioMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);
            _repoGenericoPublicacionMock.Setup(r => r.ObtenerPorIdAsync(99))
                                        .ReturnsAsync((Publicacion)null);

            Func<Task> act = async () => await _servicio.RealizarOfertaAsync(oferta);

            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("La publicacion ya no existe");
        }

        [Fact]
        public async Task AceptarOfertaAsync_CuandoOfertaExiste_LlamaRepositorio()
        {
            var oferta = new Oferta { Id = 10 };
            _repoGenericoOfertaMock.Setup(r => r.ObtenerPorIdAsync(10)).ReturnsAsync(oferta);

            await _servicio.AceptarOfertaAsync(10);

            _repoOfertaMock.Verify(r => r.AceptarOfertaAsync(10), Times.Once);
        }

        [Fact]
        public async Task RechazarOfertaAsync_CuandoNoExiste_LanzaExcepcion()
        {
            _repoGenericoOfertaMock.Setup(r => r.ObtenerPorIdAsync(999))
                                   .ReturnsAsync((Oferta)null);

            Func<Task> act = async () => await _servicio.RechazarOfertaAsync(999);

            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("No se encontró ninguna oferta con el ID 999.");
        }
    }
}
