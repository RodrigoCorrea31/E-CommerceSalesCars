using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Entidades
{
    public class Empresa : Usuario
    {
        [Required]
        public string RUT { get; set; }
        [Required]
        public string Nombre { get; set; }
    }
}
