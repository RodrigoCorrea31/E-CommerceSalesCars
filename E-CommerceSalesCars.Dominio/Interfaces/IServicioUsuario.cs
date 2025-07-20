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
        void CrearUsuario();
        void ModificarUsuario(Usuario usuario);
        void EliminarUsuario(Usuario usuario);
        void ComprarVehiculo(Vehiculo vehiculo);
        void VenderVehiculo(Vehiculo vehiculo);
    }
}
