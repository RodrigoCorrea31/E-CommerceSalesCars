using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceSalesCars.Dominio.Entidades
{
    public abstract class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set;}
        [Required]
        public string Telefono { get; set;}

        [InverseProperty("Comprador")]
        public ICollection<Transaccion> Compras { get; set;} = new List<Transaccion>();

        [InverseProperty("Vendedor")]
        public ICollection<Transaccion> Ventas { get; set; } = new List<Transaccion>();
    }
}
