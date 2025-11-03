using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Persistencia.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Persistencia.Repositorios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly MyDbContext _context;
        private readonly DbSet<Usuario> _dbset;

        public RepositorioUsuario(MyDbContext context)
        {
            _context = context;
            _dbset = _context.Set<Usuario>();
        }
        public async Task<Usuario> ObtenerPorEmailAsync(string email)
        {
            return await _dbset.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExisteUsuarioConEmailAsync(string email)
        {
            return await _dbset.AnyAsync(u=> u.Email == email);
        }

        public async Task<ICollection<Oferta>> ObtenerOfertasRealizadasAsync(int usuarioId)
        {
            var usuario = await _dbset
                .Include(u => u.OfertasRealizadadas)
                    .ThenInclude(o => o.Publicacion)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            return usuario?.OfertasRealizadadas ?? new List<Oferta>();
        }

        public async Task<ICollection<Transaccion>> ObtenerComprasAsync(int usuarioId)
        {
            return await _context.Transacciones
                .Include(t => t.Vendedor)
                .Include(t => t.Publicacion)
                .Where(t => t.CompradorId == usuarioId)
                .ToListAsync();
        }

        public async Task<ICollection<Transaccion>> ObtenerVentasAsync(int usuarioId)
        {
            return await _context.Transacciones
                .Include(t => t.CompradorTransaccion)
                .Include(t => t.Publicacion)
                .Where(t => t.VendedorId == usuarioId)
                .ToListAsync();
        }

    }
}
