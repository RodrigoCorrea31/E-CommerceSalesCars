using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.DTOs.TransaccionDTO
{
    public class TransaccionDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal PrecioVenta { get; set; }
        public string Estado { get; set; }
        public string MetodoDePago { get; set; }
        public string CompradorNombre { get; set; }
        public string VendedorNombre { get; set; }
        public string PublicacionTitulo { get; set; }
    }

}
