using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.DTOs.UsuarioDTO
{
    public class RegistrarUsuarioDto
    {
        public string TipoUsuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Contrasena { get; set; }
        public string DatoExtra1 { get; set; }
        public string DatoExtra2 { get; set; }
    }
}
