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
        Task<List<Publicacion>> ObtenerPublicacionesAsync();
        Task<List<Publicacion>> FiltrarPublicacionesAsync(Publicacion publicacionFiltro);
    }
}
