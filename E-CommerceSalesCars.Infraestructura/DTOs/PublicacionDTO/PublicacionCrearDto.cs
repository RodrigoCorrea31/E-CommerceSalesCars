using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO
{
    public class PublicacionCrearDto
    {
        public string Titulo { get; set; }
        public decimal Precio { get; set; }
        public bool EsUsado { get; set; }
        public int VehiculoId { get; set; }
        public int UsuarioId { get; set; }
    }
}
