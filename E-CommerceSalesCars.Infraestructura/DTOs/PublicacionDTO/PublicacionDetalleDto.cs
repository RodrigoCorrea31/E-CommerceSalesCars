using E_CommerceSalesCars.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO
{
    namespace E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO
    {
        public class VehiculoDto
        {
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public int Anio { get; set; }
            public int Kilometraje { get; set; }
            public Combustible Combustible { get; set; }
            public string Color { get; set; }

            public List<ImagenDto> Imagenes { get; set; } = new List<ImagenDto>();

        }

        public class ImagenDto
        {
            public int Id { get; set; }
            public string Url { get; set; }  
        }

        public class PublicacionDetalleDto
        {
            public int Id { get; set; }
            public string Titulo { get; set; }
            public decimal Precio { get; set; }
            public bool EsUsado { get; set; }
            public EstadoPublicacion Estado { get; set; }
            public VehiculoDto Vehiculo { get; set; }

            public UsuarioDto Vendedor { get; set; }
            public DateTime Fecha { get; set; }
        }

        public class UsuarioDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Telefono { get; set; }
            public string TipoUsuario { get; set; }
        }

    }
}
