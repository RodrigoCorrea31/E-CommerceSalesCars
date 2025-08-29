using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.Servicios;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace E_CommerceSalesCars.Tests
{
    public class ServicioPublicacionTests
    {
        private readonly Mock<IRepositorioPublicacion> _repositorioPublicacionMock;
        private readonly Mock<IRepositorioGenerico<Publicacion>> _repositorioGenericoPublicacionMock;
        private readonly Mock<IRepositorioGenerico<Usuario>> _repositorioGenericoUsuarioMock;
        private readonly ServicioPublicacion _servicio;

        public ServicioPublicacionTests()
        {
            _repositorioPublicacionMock = new Mock<IRepositorioPublicacion>();
            _repositorioGenericoPublicacionMock = new Mock<IRepositorioGenerico<Publicacion>>();
            _repositorioGenericoUsuarioMock = new Mock<IRepositorioGenerico<Usuario>>();

            _servicio = new ServicioPublicacion(
                _repositorioPublicacionMock.Object,
                _repositorioGenericoUsuarioMock.Object,
                _repositorioGenericoPublicacionMock.Object
            );
        }


        [Fact]
        public async Task CrearPublicacionAsync_DeberiaLanzarExcepcion_SiTituloEsVacio()
        {
            var publicacion = new Publicacion { Titulo = "", UsuarioId = 1 };

            await Assert.ThrowsAsync<ArgumentException>(() => _servicio.CrearPublicacionAsync(publicacion));
        }

        [Fact]
        public async Task CrearPublicacionAsync_DeberiaLanzarExcepcion_SiUsuarioNoExiste()
        {
            var publicacion = new Publicacion { Titulo = "Auto", UsuarioId = 99 };

            _repositorioGenericoUsuarioMock
                .Setup(r => r.ObtenerPorIdAsync(99))
                .ReturnsAsync((Usuario)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _servicio.CrearPublicacionAsync(publicacion));
        }

        [Fact]
        public async Task CrearPublicacionAsync_DeberiaLanzarExcepcion_SiYaExistePublicacionConMismoTitulo()
        {
            var publicacion = new Publicacion { Titulo = "Auto", UsuarioId = 1 };

            _repositorioGenericoUsuarioMock
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(new Persona { Id = 1, Name = "Juan", ContrasenaHash = "hash" });

            _repositorioGenericoPublicacionMock
                .Setup(r => r.ListarTodosAsync())
                .ReturnsAsync(new List<Publicacion> { new Publicacion { Titulo = "Auto", UsuarioId = 1 } });

            await Assert.ThrowsAsync<InvalidOperationException>(() => _servicio.CrearPublicacionAsync(publicacion));
        }

        [Fact]
        public async Task CrearPublicacionAsync_DeberiaAgregarPublicacion_SiEsValida()
        {
            var publicacion = new Publicacion { Titulo = "Auto Nuevo", UsuarioId = 1 };

            _repositorioGenericoUsuarioMock
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(new Persona { Id = 1, Name = "Juan", ContrasenaHash = "hash" });

            _repositorioGenericoPublicacionMock
                .Setup(r => r.ListarTodosAsync())
                .ReturnsAsync(new List<Publicacion>());

            await _servicio.CrearPublicacionAsync(publicacion);

            _repositorioGenericoPublicacionMock.Verify(r => r.AgregarAsync(publicacion), Times.Once);
        }


        [Fact]
        public async Task EditarPublicacionAsync_DeberiaLanzarExcepcion_SiIdNoCoincide()
        {
            var publicacion = new Publicacion { Id = 2, Titulo = "Auto", UsuarioId = 1 };

            await Assert.ThrowsAsync<ArgumentException>(() => _servicio.EditarPublicacionAsync(1, publicacion));
        }

        [Fact]
        public async Task EditarPublicacionAsync_DeberiaLanzarExcepcion_SiPublicacionNoExiste()
        {
            var publicacion = new Publicacion { Id = 1, Titulo = "Auto", UsuarioId = 1 };

            _repositorioGenericoPublicacionMock
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync((Publicacion)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _servicio.EditarPublicacionAsync(1, publicacion));
        }

        [Fact]
        public async Task EditarPublicacionAsync_DeberiaModificar_SiEsValida()
        {
            var publicacion = new Publicacion { Id = 1, Titulo = "Auto", UsuarioId = 1 };

            _repositorioGenericoPublicacionMock
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(publicacion);

            _repositorioGenericoPublicacionMock
                .Setup(r => r.ListarTodosAsync())
                .ReturnsAsync(new List<Publicacion> { publicacion });

            await _servicio.EditarPublicacionAsync(1, publicacion);

            _repositorioGenericoPublicacionMock.Verify(r => r.ModificarAsync(publicacion), Times.Once);
        }


        [Fact]
        public async Task EliminarPublicacionAsync_DeberiaLanzarExcepcion_SiPublicacionNoExiste()
        {
            _repositorioGenericoPublicacionMock
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync((Publicacion)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _servicio.EliminarPublicacionAsync(1));
        }

        [Fact]
        public async Task EliminarPublicacionAsync_DeberiaEliminar_SiExiste()
        {
            var publicacion = new Publicacion { Id = 1, Titulo = "Auto", UsuarioId = 1 };

            _repositorioGenericoPublicacionMock
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(publicacion);

            await _servicio.EliminarPublicacionAsync(1);

            _repositorioGenericoPublicacionMock.Verify(r => r.EliminarAsync(publicacion), Times.Once);
        }


        [Fact]
        public async Task FiltrarPublicacionesAsync_DeberiaRetornarCoincidencias()
        {
            var publicaciones = new List<Publicacion>
            {
                new Publicacion { Titulo = "Ford Fiesta", Precio = 5000, Vehiculo = new Vehiculo { Marca = "Ford", Modelo = "Fiesta", Anio = 2020 } },
                new Publicacion { Titulo = "Chevrolet Onix", Precio = 7000, Vehiculo = new Vehiculo { Marca = "Chevrolet", Modelo = "Onix", Anio = 2021 } }
            };

            _repositorioGenericoPublicacionMock
                .Setup(r => r.ObtenerPorFiltroAsync(It.IsAny<Expression<Func<Publicacion, bool>>>()))
                .ReturnsAsync((Expression<Func<Publicacion, bool>> pred) =>
                    publicaciones.Where(pred.Compile()).ToList());

            var result = await _servicio.FiltrarPublicacionesAsync("Ford", null, null, null, null, null);

            Assert.Single(result);
            Assert.Equal("Ford Fiesta", result.First().Titulo);
        }
    }
}
