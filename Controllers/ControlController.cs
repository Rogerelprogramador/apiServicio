using apiServicio.Models;
using apiServicio.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace apiServicio.Controllers
{
    [ApiController]
    [Route("api/ [controller]")]
    public class ControlController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        private readonly IUsuarioService _IUsuarioService;
        public ControlController(IConfiguration Configuration, IUsuarioService UsuarioService)
        {
            _IUsuarioService = UsuarioService;
            _Configuration = Configuration;
        }
        [HttpPost("CrearToken")]
        public async Task<TokenModel> PostToken(string login, string password)
        {
            TokenModel token = new TokenModel();

            try
            {
                var usuario = await _IUsuarioService.GetNombreUsuario(login);

                if (usuario != null)
                {
                    var hashedPassword = _IUsuarioService.CrearPasswordHash(password, usuario.Salt);

                    if (hashedPassword == usuario.Clave)
                    {
                        var currentDate = DateTime.UtcNow;
                        var expirationTime = TimeSpan.FromMinutes(10);
                        var expireDateTime = currentDate.Add(expirationTime);

                        // Obtén la configuración una vez.
                        var authSettings = _Configuration.GetSection("AuthentificationSettings");

                        string issuer = authSettings["Issuer"];
                        string audience = authSettings["Audence"];
                        string signingKey = authSettings["Signingkey"];

                        // Genera el token.
                        token.Token = _IUsuarioService.GenerarToken(currentDate, usuario, expirationTime, signingKey, audience, issuer);
                        token.tiempoExpira = expireDateTime;
                    }
                    else
                    {
                        // Manejar error de contraseña incorrecta.
                    }
                }
                else
                {
                    // Manejar error de usuario no existente.
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier otra excepción.
                Console.WriteLine($"Error en la generación del token: {ex.Message}");
                // Puedes registrar el error o manejarlo según tus necesidades.
            }

            return token;
        }
    }
}