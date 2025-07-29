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
        Task RealizarOfertaAsync(Oferta oferta);
        Task AceptarOfertaAsync(int ofertaId);
        Task RechazarOfertaAsync(int ofertaId);
    }
}
