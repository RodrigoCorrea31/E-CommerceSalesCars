using E_CommerceSalesCars.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace E_CommerceSalesCars.Persistencia.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) 
        {
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<ImagenVehiculo> ImagenesVehiculos { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Publicacion> Publicaciones { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
    }
}
