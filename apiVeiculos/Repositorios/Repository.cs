using apiVeiculos.Entidades;
using apiVeiculos.Infraestrutura;
using apiVeiculos.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace apiVeiculos.Repositorios
{
    public class Repository<TEntidade> : IRepository<TEntidade> where TEntidade : EntidadeComId
    {
        protected readonly ConexaoContext _db;
        protected readonly DbSet<TEntidade> DbSet;

        public Repository(ConexaoContext db)
        {
            _db = db;
            DbSet = db.Set<TEntidade>();
        }

        public async Task AddAsync(TEntidade entidade)
        {
            DbSet.AddAsync(entidade);
            await _db.SaveChangesAsync();
        }

        public async Task RemovaAsync(int id)
        {
            var entidade = await DbSet.FindAsync(id);
            DbSet.Remove(entidade);
            await _db.SaveChangesAsync();
        }

        public async Task AtualizeAsync(TEntidade entidade)
        {
            DbSet.Update(entidade);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntidade>> ConsulteParcialAsync(Expression<Func<TEntidade, bool>> expression) =>
            await DbSet.AsNoTracking().Where(expression).ToListAsync();

        public async Task<TEntidade> ConsultePorIdAsync(int id) =>
            await DbSet.FindAsync(id);

        public async Task<List<TEntidade>> ConsulteTodosRegistrosAsync(TEntidade entidade) =>
            await DbSet.ToListAsync();

        public void Dispose()
        {
            _db?.Dispose();
        }

    }
}
