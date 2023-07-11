using apiVeiculos.Entidades;
using apiVeiculos.Infraestrutura;
using apiVeiculos.Repositorios.Interfaces;
using System.Linq.Expressions;

namespace apiVeiculos.Repositorios
{
    public class CategoriaRepository : Repository<Categoria> , ICategoriaRepository
    {
        public CategoriaRepository(ConexaoContext db) : base(db) { }
    }
}
