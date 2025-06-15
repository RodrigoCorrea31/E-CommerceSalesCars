using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Entidades
{
    public class ImagenVehiculo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(300, ErrorMessage = "La URL no debe tener mas de 300 caracteres.")]
        public string Url { get; set; }
        public int Orden { get; set; }
        public bool EsPrincipal { get; set; }
        [Required]
        public int VehiculoId { get; set; }

        [ForeignKey("VehiculoId")]
        public Vehiculo Vehiculo { get; set; }
    }

}
