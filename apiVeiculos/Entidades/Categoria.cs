using apiVeiculos.Validacoes;

namespace apiVeiculos.Entidades
{
    public sealed class Categoria : EntidadeComId
    {
        public string? Nome { get; set; }
        public IEnumerable<Veiculo> Veiculos { get; set; }

        public Categoria(string nome) => ValidarDominio(nome);

        public Categoria(int id, string nome)
        {
            DominioExceptionValidation.QuandoHouverErro(id < 0, "Id informado inválido");
            Id = id;
            ValidarDominio(nome);
        }

        public void Atualizar(string nome) => ValidarDominio(nome);

        private void ValidarDominio(string nome)
        {
            DominioExceptionValidation.QuandoHouverErro(string.IsNullOrEmpty(nome), "Nome é obrigatório");
            DominioExceptionValidation.QuandoHouverErro(nome.Length < 3, "Nome inválido");
            Nome = nome;
        }

    }
}
