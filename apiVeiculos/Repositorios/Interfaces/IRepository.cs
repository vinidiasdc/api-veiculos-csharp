using apiVeiculos.Entidades;
using System.Linq.Expressions;

namespace apiVeiculos.Repositorios.Interfaces
{
    public interface IRepository<T> : IDisposable where T : EntidadeComId
    {
        Task AddAsync(T entidade);
        Task<List<T>> ConsulteTodosRegistrosAsync(T entidade);
        Task<T> ConsultePorIdAsync(int id);
        Task AtualizeAsync(T entidade);
        Task RemovaAsync(int id);
        Task<IEnumerable<T>> ConsulteParcialAsync(Expression<Func<T, bool>> expression);
    }
}
