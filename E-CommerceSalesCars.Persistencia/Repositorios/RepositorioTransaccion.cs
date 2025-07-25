using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Persistencia.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Persistencia.Repositorios
{
    public class RepositorioTransaccion : IRepositorioTransaccion
    {
        private readonly MyDbContext _context;
        private readonly DbSet<Transaccion> _dbset;

        public RepositorioTransaccion (MyDbContext context)
        {
            _context = context;
            _dbset = _context.Set<Transaccion>();
        }

        public async Task FinalizarTransaccionAsync(int transaccionId)
        {
            await _dbset.Where(t => t.Id == transaccionId).ExecuteUpdateAsync(setters => setters.SetProperty(t => t.Estado, EstadoTransaccion.Finalizada));
        }
    }
}
