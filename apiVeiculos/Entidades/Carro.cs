using apiVeiculos.Entidades.Enumeradores;

namespace apiVeiculos.Entidades
{
    public class Carro : Veiculo
    {
        public int QtdPassageiros { get; set; }
        public int Portas { get; set; }

        public EnumeradorTipoFreio TipoDoFreio { get; set; }
    }
}
