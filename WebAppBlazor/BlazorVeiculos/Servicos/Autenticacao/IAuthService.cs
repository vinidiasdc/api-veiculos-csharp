using BlazorVeiculos.Models;

namespace BlazorVeiculos.Servicos.Autenticacao
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);

        Task Logout();
    }
}
