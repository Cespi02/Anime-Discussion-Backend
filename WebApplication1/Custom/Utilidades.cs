using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Custom
{
    public class Utilidades
    {
        private readonly IConfiguration _configuration;
        public Utilidades(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string encriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {  
                //Computar hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));
                //Convertir bytes a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public string generarJWT(Cliente modelo)
        {
            // crear informacion de cliente para el token
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, modelo.IdCliente.ToString()),
                new Claim(ClaimTypes.Email, modelo.Email!),
                new Claim(ClaimTypes.UserData, modelo.NombreUsuario!)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

        public string CalcularHorarioComentario(DateTime fecha)
        {
            DateTime fechaActual = DateTime.Now;
            TimeSpan diferencia = fechaActual.Subtract(fecha).Duration();

            if (diferencia.Days >= 365)
            {
                int años = diferencia.Days / 365;
                return $"{años} año(s)";
            }
            else if (diferencia.Days >= 30)
            {
                int meses = diferencia.Days / 30;
                return $"{meses} mes(es)";
            }
            else if (diferencia.Days > 0)
            {
                return $"{diferencia.Days} día(s)";
            }
            else if (diferencia.Hours > 0)
            {
                return $"{diferencia.Hours} hora(s)";
            }
            else
            {
                return $"{diferencia.Minutes} minuto(s)";
            }
        }

    }
}
