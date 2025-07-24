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
            var usuario = await _dbset.Include(u => u.OfertasRealizadadas).FirstOrDefaultAsync(u => u.Id == usuarioId);
            return usuario?.OfertasRealizadadas ?? new List<Oferta>();
        }

        public async Task<ICollection<Transaccion>> ObtenerComprasAsync(int usuarioId)
        {
            var usuario = await _dbset.Include(u => u.Compras).FirstOrDefaultAsync(u => u.Id == usuarioId);
            return usuario?.Compras ?? new List<Transaccion>();
        }

        public async Task<ICollection<Transaccion>> ObtenerVentasAsync(int usuarioId)
        {
            var usuario = await _dbset.Include(u => u.Ventas).FirstOrDefaultAsync(u => u.Id == usuarioId);
            return usuario?.Ventas ?? new List<Transaccion>();
        }
    }
}
