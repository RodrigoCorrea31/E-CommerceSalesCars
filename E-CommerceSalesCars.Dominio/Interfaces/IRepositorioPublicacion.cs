using E_CommerceSalesCars.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Interfaces
{
    public interface IRepositorioPublicacion
    {
        Task<ICollection<Publicacion>> ObtenerPublicacionesConVehiculoAsync();
        Task<ICollection<Publicacion>> FiltrarPublicacionesAsync(Expression<Func<Publicacion, bool>> filtro);
    }
}
