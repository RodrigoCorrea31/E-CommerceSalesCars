using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.DTOs.TransaccionDTO
{
    public class TransaccionRespuestaDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public decimal PrecioVenta { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
        public string MetodoDePago { get; set; }
        public int CompradorId { get; set; }
        public int VendedorId { get; set; }
        public int PublicacionId { get; set; }
    }
}
