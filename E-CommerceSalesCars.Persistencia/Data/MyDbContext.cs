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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Oferta>()
                .HasOne(o => o.CompradorOferta)
                .WithMany(u => u.OfertasRealizadadas)
                .HasForeignKey(o => o.CompradorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Oferta>()
                .HasOne(o => o.Publicacion)
                .WithMany(p => p.Ofertas)
                .HasForeignKey(o => o.PublicacionId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.CompradorTransaccion)
                .WithMany(u => u.Compras)
                .HasForeignKey(t => t.CompradorId)
                .OnDelete(DeleteBehavior.NoAction); // ❗ Quitamos cascada

            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.Vendedor)
                .WithMany(u => u.Ventas)
                .HasForeignKey(t => t.VendedorId)
                .OnDelete(DeleteBehavior.NoAction); // ❗ También quitamos cascada (opcional pero recomendable)

            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.Publicacion)
                .WithMany()
                .HasForeignKey(t => t.PublicacionId)
                .OnDelete(DeleteBehavior.Cascade); // ✅ Mantenemos cascada solo acá si querés
        }

    }

}
