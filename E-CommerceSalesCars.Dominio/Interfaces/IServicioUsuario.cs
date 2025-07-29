using E_CommerceSalesCars.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Interfaces
{
    public interface IServicioUsuario
    {
        Task RegistrarUsuarioAsync(string nombreUsuario, string email, string contraseña);
        Task<Usuario> LoginAsync(string email, string contraseña);
        Task<List<Oferta>> ObtenerOfertasRealizadasAsync(int usuarioId);
        Task<List<Transaccion>> ObtenerComprasAsync(int usuarioId);
        Task<List<Transaccion>> ObtenerVentasAsync(int usuarioId);
        Task ComprarVehiculoAsync(int usuarioId, int publicacionId, decimal monto);
    }
}
