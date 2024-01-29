using apiServicio.Business.Contracts;
using apiServicio.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using apiServicio.Services.Contracts;
using System.Collections.Generic;

namespace apiServicio.Services.Clases
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _IUsuarioRepository;
        public UsuarioService(IUsuarioRepository tempI)
        {
            _IUsuarioRepository = tempI;
        }
        public Task<Usuario> GetNombreUsuario(string nombreusuario)
        {
            return _IUsuarioRepository.GetNombreUsuario(nombreusuario);
        }
        public string CrearPasswordHash(string password, string salt)
        {
            // Concatena la contraseña y la sal.
            string cadenaUnida = string.Concat(password, salt);
            // Utiliza SHA-256 en lugar de SHA-1, ya que SHA-1 se considera obsoleto y menos seguro.
            using (var sha256 = SHA256.Create())
            {
                // Calcula el hash de la cadena unida.
                var resultadoHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(cadenaUnida));
                // Convierte el resultado del hash a una cadena hexadecimal.
                var strResultadoHash = BitConverter.ToString(resultadoHash).Replace("-", "").ToUpper();
                return strResultadoHash;
            }
        }
        public string GenerarToken(DateTime fechaEmision, Usuario usuario, TimeSpan tiempoExpiracion,
                            string claveFirma, string audiencia, string emisor)
        {
            // Calcula la fecha de vencimiento del token.
            var fechaExpiracion = fechaEmision.Add(tiempoExpiracion);

            // Define las reclamaciones del token.
            var reclamaciones = new Claim[]
            {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Identificador único del token.
        new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(fechaEmision).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Fecha de emisión.
        new Claim("NombreUsuario", usuario.NombreUsuario),
        new Claim("IdUsuario", usuario.IdUsuario.ToString())
            };

            // Configura las credenciales de firma.
            var credencialesFirma = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(claveFirma)),
                SecurityAlgorithms.HmacSha256Signature
            );

            try
            {
                // Crea el token JWT.
                var tokenJwt = new JwtSecurityToken(
                    issuer: emisor,
                    audience: audiencia,
                    claims: reclamaciones,
                    notBefore: fechaEmision,
                    expires: fechaExpiracion,
                    signingCredentials: credencialesFirma
                );

                // Codifica el token.
                var tokenCodificado = new JwtSecurityTokenHandler().WriteToken(tokenJwt);

                return tokenCodificado;
            }
            catch (Exception ex)
            {
                // Manejar excepciones relacionadas con la generación del token.
                Console.WriteLine($"Error al generar el token: {ex.Message}");
                throw; // Puedes manejar la excepción de manera diferente según tus necesidades.
            }
        }
        
    }
}