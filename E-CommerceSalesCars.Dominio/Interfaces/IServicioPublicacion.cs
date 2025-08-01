using E_CommerceSalesCars.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Interfaces
{
    public interface IServicioPublicacion
    {
        Task CrearPublicacionAsync(Publicacion publicacion);
        Task EditarPublicacionAsync(int id, Publicacion publicacion);
        Task EliminarPublicacionAsync(int id);
        Task<ICollection<Publicacion>> ObtenerPublicacionesAsync();
        Task<ICollection<Publicacion>> FiltrarPublicacionesAsync(string marca, string modelo, int? anioDesde, int? anioHasta, decimal? precioDesde, decimal? precioHasta);
    }
}
