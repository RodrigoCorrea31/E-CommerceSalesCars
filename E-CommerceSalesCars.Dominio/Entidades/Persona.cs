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
        public string CI { get; set; }
        [Required]
        public string Direccion { get; set; }

    }
}
