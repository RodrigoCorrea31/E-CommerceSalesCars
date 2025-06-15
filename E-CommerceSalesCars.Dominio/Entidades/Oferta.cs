using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Entidades
{
    public enum EstadoOferta { 
        Pendiente,
        Aceptada,
        Rechazada,
        Cancelada
    }

    public class Oferta
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        [Required]
        public EstadoOferta Estado { get; set; }
        public int CompradorId { get; set; }

        [ForeignKey("CompradorId")]
        public Usuario CompradorOferta { get; set; }

        public int PublicacionId { get; set; }

        [ForeignKey("PublicacionId")]
        public Publicacion Publicacion { get; set; }
    }
}
