using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required, StringLength(150)]
        public string Titulo { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public bool EsUsado { get; set; }

        [Required]
        public EstadoPublicacion Estado { get; set; }

        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        public int VehiculoId { get; set; }
        [ForeignKey("VehiculoId")]
        public Vehiculo Vehiculo { get; set; }

        public ICollection<Oferta> Ofertas { get; set; } = new List<Oferta>();
    }

}
