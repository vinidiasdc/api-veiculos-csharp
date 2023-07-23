using apiVeiculos.Entidades.Autenticacao;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace apiVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<ApplicationUsuario> _userManager;
        private readonly SignInManager<ApplicationUsuario> _signInManager;
        private readonly IConfiguration _config;

        public UsuariosController(UserManager<ApplicationUsuario> userManager, SignInManager<ApplicationUsuario> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("Registro")]
        public async Task<ActionResult> CriarUsuario([FromBody] Usuario model)
        {
            ApplicationUsuario user = new() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await _userManager.CreateAsync(user, model.Senha);

            if(result.Succeeded)
            {
                return Ok(model);
            }
            else
            {
                return BadRequest("Usuário ou senha inválidos!");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UsuarioToken>> Login([FromBody] Usuario usuarioInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(usuarioInfo.Email, usuarioInfo.Senha, isPersistent: false, lockoutOnFailure: false);

            if(result.Succeeded)
            {
                return BuildToken(usuarioInfo);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login inválido");
                return BadRequest(ModelState);
            }
        }

        private UsuarioToken BuildToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email),
                new Claim("vinicius", "teste"),
                new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:Audience"]),
                new Claim(JwtRegisteredClaimNames.Iss, _config["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(2);

            JwtSecurityToken token = new(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );

            return new UsuarioToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracao = expiration
            };

        }
    }
}
