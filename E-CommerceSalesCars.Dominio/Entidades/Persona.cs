using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Entidades
{
    public class Persona : Usuario
    {
        [Required]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "La CI debe tener entre 8 y 7 dígitos.")]
        public string CI { get; set; }
        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }

    }
}
