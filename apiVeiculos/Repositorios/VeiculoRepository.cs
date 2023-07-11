using apiVeiculos.Entidades;
using apiVeiculos.Infraestrutura;
using apiVeiculos.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace apiVeiculos.Repositorios
{
    public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(ConexaoContext db) : base(db) { }

        public async Task<IEnumerable<Veiculo>> ConsulteVeiculoPorCategoriaAsync(int categoriaId)
        {
            return await _db.Veiculos.Where(v => v.CategoriaId == categoriaId).ToListAsync();
        }

        public async Task<IEnumerable<Veiculo>> ConsulteVeiculoPorTermoAsync(string termoPesquisa)
        {
            return await _db.Veiculos.AsNoTracking()
                .Include(b => b.Categoria)
                .Where(b => b.Nome.Contains(termoPesquisa) ||
                            b.Marca.Contains(termoPesquisa))
                .ToListAsync();
        }
    }
}
