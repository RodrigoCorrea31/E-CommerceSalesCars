using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Entidades
{
    public enum EstadoTransaccion { 
        Pendiente,
        Finalizada,
        Cancelada
    }
    public class Transaccion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public DateTime FechaFinalizacion { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal PrecioVenta { get; set; }
        [StringLength(250)]
        public string Observacion { get; set; }
        [Required]
        public EstadoTransaccion Estado { get; set; }
        [Required]
        [StringLength(50)]
        public string MetodoDePago { get; set; }

        public int CompradorId { get; set; }

        [ForeignKey("CompradorId")]
        [InverseProperty("Compras")]
        public Usuario CompradorTransaccion { get; set; }

        public int VendedorId { get; set; }

        [ForeignKey("VendedorId")]
        [InverseProperty("Ventas")]
        public Usuario Vendedor { get; set; }

        public int PublicacionId { get; set; }

        [ForeignKey("PublicacionId")]
        public Publicacion Publicacion { get; set; }
    }
}
