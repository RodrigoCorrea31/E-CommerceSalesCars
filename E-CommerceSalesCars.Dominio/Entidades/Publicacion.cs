using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Entidades
{
    public enum EstadoPublicacion { 
        Activo,
        Vendido,
        Pausado
    }
    public class Publicacion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public bool EsUsado { get; set; }
        [Required]
        public EstadoPublicacion Estado { get; set; }

        public Transaccion Transaccion { get; set; }

        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

    }
}
