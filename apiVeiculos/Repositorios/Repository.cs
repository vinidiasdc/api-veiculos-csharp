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

        public Task AddAsync(TEntidade entidade)
        {
            throw new NotImplementedException();
        }

        public Task AtualizeAsync(TEntidade entidade)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntidade>> ConsulteParcialAsync(Expression<Func<TEntidade, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<TEntidade> ConsultePorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntidade>> ConsulteTodosRegistrosAsync(TEntidade entidade)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task RemovaAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
