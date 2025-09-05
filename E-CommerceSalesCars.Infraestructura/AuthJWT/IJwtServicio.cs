using E_CommerceSalesCars.Infraestructura.DTOs.UsuarioDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.AuthJWT
{
    public interface IJwtServicio
    {
        string GenerarTokenJwt(UsuarioJwtDTO usuario);
    }
}
