using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Entidades
{
    public enum Combustible { 
        Nafta,
        Diesel,
        Electrico
    }
    public class Vehiculo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Marca { get; set; }
        [Required]
        [StringLength(50)]
        public string Modelo { get; set; }
        [Required]
        [Range(1900, 2100, ErrorMessage = "Año fuera del rango válido.")]
        public int Anio { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El kilometraje debe ser igual o mayor a 0.")]
        public int Kilometraje { get; set; }
        [Required]
        public Combustible Combustible { get; set; }
        [Required]
        [StringLength(30)]
        public string Color { get; set; }

        [Required]
        public ICollection<ImagenVehiculo> Imagenes { get; set; } = new List<ImagenVehiculo>();
    }
}
