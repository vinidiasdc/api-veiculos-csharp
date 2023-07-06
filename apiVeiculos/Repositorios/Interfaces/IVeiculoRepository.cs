using apiVeiculos.Entidades;

namespace apiVeiculos.Repositorios.Interfaces
{
    public interface IVeiculoRepository : IRepository<Veiculo>
    {
        Task<IEnumerable<Veiculo>> ConsulteVeiculoPorCategoriaAsync(int categoriaId);

        Task<IEnumerable<Veiculo>> ConsulteVeiculoPorTermoAsync(string termoPesquisa);
    }
}
