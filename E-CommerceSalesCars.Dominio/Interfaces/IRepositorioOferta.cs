using E_CommerceSalesCars.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Interfaces
{
    public interface IRepositorioOferta
    {
        Task<ICollection<Oferta>> ObtenerOfertasPorPublicacionAsync(int publicacionId);
        Task AceptarOfertaAsync(int ofertaId);
        Task RechazarOfertaAsync(int ofertaId);
    }
}
