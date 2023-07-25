using Blazored.LocalStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;

namespace BlazorVeiculos.Servicos.Autenticacao
{
    public class ApiAutenticacaoStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public ApiAutenticacaoStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string salvedToken = await _localStorage.GetItemAsync<string>("authToken");
            string expiration = await _localStorage.GetItemAsync<string>("expirationToken");

            if(string.IsNullOrWhiteSpace(salvedToken) || TokenExpirou(expiration))
            {
                DefinaUsuarioComoNaoLogado();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(salvedToken), "jwt")));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());
                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                } else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }
                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private bool TokenExpirou(string expiration)
        {
            DateTime dataAtual = DateTime.UtcNow;
            DateTime dataExpiracao = DateTime.ParseExact(expiration, "yyyy-MM-dd'T'HH:mm:ss.fffffff'Z", null, System.Globalization.DateTimeStyles.RoundtripKind);

            if(dataExpiracao < dataAtual)
            {
                return true;
            }

            return false;   
        }

        private void DefinaUsuarioComoNaoLogado()
        {
            var usuarioAnonimo = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(usuarioAnonimo));
            NotifyAuthenticationStateChanged(authState);
        }

        public void DefinaUsuarioComoLogado(string email)
        {
            var usuarioAutenticado = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email) },
                "apiauth"
            ));

            var authState = Task.FromResult(new AuthenticationState(usuarioAutenticado));

            NotifyAuthenticationStateChanged(authState);
        }
    }
}
