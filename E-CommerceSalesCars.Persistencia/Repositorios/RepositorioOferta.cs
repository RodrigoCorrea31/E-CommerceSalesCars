using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Persistencia.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Persistencia.Repositorios
{
    public class RepositorioOferta : IRepositorioOferta
    {
        private readonly MyDbContext _context;
        private readonly DbSet<Oferta> _dbset;

        public RepositorioOferta (MyDbContext context)
        {
            _context = context;
            _dbset = _context.Set<Oferta>();
        }

        public async Task<ICollection<Oferta>> ObtenerOfertasPorPublicacionAsync(int publicacionId)
        {
            return await _dbset.Where(o => o.PublicacionId == publicacionId).ToListAsync();
        }

        public async Task AceptarOfertaAsync(int ofertaId)
        {
            await _dbset.Where(o => o.Id == ofertaId).ExecuteUpdateAsync(setters => setters.SetProperty(o => o.Estado, EstadoOferta.Aceptada));
        }

        public async Task RechazarOfertaAsync(int ofertaId)
        {
            await _dbset.Where(o => o.Id == ofertaId).ExecuteUpdateAsync(setters => setters.SetProperty(o => o.Estado, EstadoOferta.Rechazada));
        }
    }
}
