namespace apiVeiculos.Entidades
{
    public abstract class Veiculo : EntidadeComId
    {
        public string? Nome { get; set; }
        public string? Marca { get; set; }
        public int Ano { get; set; }
        public double Velocidade { get; set; }

        //chave estrangeira
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
