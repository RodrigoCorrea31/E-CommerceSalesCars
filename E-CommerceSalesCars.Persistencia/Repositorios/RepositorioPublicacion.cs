using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.DTOs;
using E_CommerceSalesCars.Persistencia.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Persistencia.Repositorios
{
    public class RepositorioPublicacion : IRepositorioPublicacion
    {
        private readonly MyDbContext _context;
        private readonly DbSet<Publicacion> _dbset;

        public RepositorioPublicacion(MyDbContext context) 
        {
            _context = context;
            _dbset = _context.Set<Publicacion>();
        }
        public async Task<ICollection<Publicacion>> ObtenerPublicacionesConVehiculoAsync()
        {
            return await _dbset
                .Include(p => p.Vehiculo)
                    .ThenInclude(v => v.Imagenes)
                .ToListAsync();
        }

        public async Task<ICollection<Publicacion>> FiltrarPublicacionesAsync(Expression<Func<Publicacion, bool>> filtro)
        {
            return await _dbset.Include(p => p.Vehiculo).Where(filtro).ToListAsync();
        }
    }
}
