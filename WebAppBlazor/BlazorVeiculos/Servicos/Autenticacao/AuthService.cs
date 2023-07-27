using Blazored.LocalStorage;
using BlazorVeiculos.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BlazorVeiculos.Servicos.Autenticacao
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("ApiVeiculos");
                string loginAsJson = JsonSerializer.Serialize(loginModel);

                StringContent request = new StringContent(loginAsJson, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("api/usuarios/login", request);

                var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if(!response.IsSuccessStatusCode)
                {
                    return loginResult;
                }

                await _localStorage.SetItemAsync("authToken", loginResult.Token);
                await _localStorage.SetItemAsync("tokenExpiration", loginResult.Expiration);

                ((ApiAutenticacaoStateProvider)_authenticationStateProvider)
                                    .DefinaUsuarioComoLogado(loginModel.Email);

                httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("bearer",
                                                             loginResult.Token);

                return loginResult;
            } 
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Logout()
        {
            var httpClient = _httpClientFactory.CreateClient("ApiVeiculos");
            await _localStorage.RemoveItemAsync("authToken");

            ((ApiAutenticacaoStateProvider)_authenticationStateProvider).DefinaUsuarioComoNaoLogado();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
