using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.DTOs
{
    public class FiltroPublicacionDTO
    {
        string? Marca {  get; set; }
        string? Modelo { get; set; }
        decimal? PrecioMinimo { get; set; }
        decimal? PrecioMaximo { get; set; }
        int? Anio { get; set; }
        string? TipoVehiculo { get; set; }
    }
}
