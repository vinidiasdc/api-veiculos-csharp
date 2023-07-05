namespace apiVeiculos.Validacoes
{
    public class DominioExceptionValidation : Exception
    {
        public DominioExceptionValidation(string error) : base(error) { }

        public static void QuandoHouverErro(bool temErro, string error)
        {
            if (temErro)
            {
                throw new DominioExceptionValidation(error);
            }
        }
    }
}
