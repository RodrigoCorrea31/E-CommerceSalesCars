using E_CommerceSalesCars.Dominio.Interfaces;
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
    public class RepositorioGenerico<E> : IRepositorioGenerico<E> where E : class
    {
        private readonly MyDbContext _context;
        private readonly DbSet<E> _dbset;

        public RepositorioGenerico(MyDbContext context)
        {
            _context = context;
            _dbset = _context.Set<E>();
        }

        public async Task<E> ObtenerPorIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public async Task<ICollection<E>> ListarTodosAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task AgregarAsync(E entidad)
        {
            await _dbset.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public void EliminarAsync(E entidad)
        {
            _dbset.Remove(entidad);
        }

        public void ModificarAsync(E entidad)
        {
            _dbset.Update(entidad);
        }

        public async Task ActualizarAsync(E entidad)
        {
            _dbset.Update(entidad);
            await _context.SaveChangesAsync();
        }



        public async Task<ICollection<E>> ObtenerPorFiltroAsync(Expression<Func<E, bool>>filtro)
        {
            return await _dbset.Where(filtro).ToListAsync();
        }
    }
}
