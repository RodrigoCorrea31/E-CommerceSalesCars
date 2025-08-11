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
        Task RegistrarUsuarioAsync(string tipoUsuario, string nombre, string email, string telefono, string contrasena, string datoExtra1, string datoExtra2);
        Task<Usuario> LoginAsync(string email, string contraseña);
        Task<ICollection<Oferta>> ObtenerOfertasRealizadasAsync(int usuarioId);
        Task<ICollection<Transaccion>> ObtenerComprasAsync(int usuarioId);
        Task<ICollection<Transaccion>> ObtenerVentasAsync(int usuarioId);
        Task ComprarVehiculoAsync(int usuarioId, int publicacionId, decimal monto);
    }
}
