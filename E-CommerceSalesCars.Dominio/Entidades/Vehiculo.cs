using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        public int Anio { get; set; }
        [Required]
        public int Kilometraje { get; set; }
        [Required]
        public Combustible Combustible { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public ICollection<ImagenVehiculo> imagenes { get; set; } = new List<ImagenVehiculo>();
    }
}
