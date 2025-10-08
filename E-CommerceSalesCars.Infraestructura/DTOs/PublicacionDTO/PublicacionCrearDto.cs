using E_CommerceSalesCars.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO
{
    public class VehiculoCrearDto
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public int Kilometraje { get; set; }
        public Combustible Combustible { get; set; }
        public string Color { get; set; }

        public List<string> Imagenes { get; set; } = new List<string>();
    }

    public class PublicacionCrearDto
    {
        public string Titulo { get; set; }
        public decimal Precio { get; set; }
        public bool EsUsado { get; set; }
        public VehiculoCrearDto Vehiculo { get; set; }
    }


}
