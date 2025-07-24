using E_CommerceSalesCars.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Interfaces
{
    public interface IRepositorioUsuario
    {
        Task<Usuario> ObtenerPorEmailAsync(string email);
        Task<bool> ExisteUsuarioConEmailAsync(string email);
        Task<ICollection<Oferta>> ObtenerOfertasRealizadasAsync(int usuarioId);
        Task<ICollection<Transaccion>> ObtenerComprasAsync(int usuarioId);
        Task<ICollection<Transaccion>> ObtenerVentasAsync(int usuarioId);
    }
}
