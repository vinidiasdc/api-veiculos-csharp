namespace apiVeiculos.Entidades.Autenticacao
{
    public class UsuarioToken
    {
        public string? Token { get; set; }
        public DateTime Expiracao { get; set; }
    }
}
