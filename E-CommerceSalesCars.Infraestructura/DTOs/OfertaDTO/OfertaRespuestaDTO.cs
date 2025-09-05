using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.DTOs.OfertaDTO
{
    public class OfertaRespuestaDTO
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string Estado { get; set; }
        public int CompradorId { get; set; }
        public int PublicacionId { get; set; }
    }

}
