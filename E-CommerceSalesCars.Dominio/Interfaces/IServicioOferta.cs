using E_CommerceSalesCars.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Interfaces
{
    public interface IServicioOferta
    {
        Task<Oferta> RealizarOfertaAsync(decimal monto, int compradorId, int publicacionId);
        Task EliminarOfertaAsync(int id);
        Task<Oferta> ObtenerPorIdAsync(int id);
        Task AceptarOfertaAsync(int ofertaId);
        Task RechazarOfertaAsync(int ofertaId);
    }
}
