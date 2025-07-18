using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Interfaces
{
    public interface IRepositorioGenerico<E> where E : class
    {
        Task<E> ObtenerPorIdAsync(int id);
        Task<IEnumerable<E>> ListarTodosAsync();
        Task AgregarAsync(E Entidad);
        void ModificarAsync(E Entidad);
        void EliminarAsync(E Entidad);
    }
}
