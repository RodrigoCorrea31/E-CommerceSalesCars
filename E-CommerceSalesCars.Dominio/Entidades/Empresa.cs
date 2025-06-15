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
        [StringLength(12, MinimumLength = 8, ErrorMessage = "El RUT debe tener entre 12 y 8 caracteres.")]
        public string RUT { get; set; }
        [Required]
        public string RazonSocial { get; set; }
    }
}
