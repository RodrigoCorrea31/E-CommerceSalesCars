using E_CommerceSalesCars.Infraestructura.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.AuthJWT
{
    public interface IJwtServicio
    {
        string GenerarTokenJwt(UsuarioDTO user);
    }
}
