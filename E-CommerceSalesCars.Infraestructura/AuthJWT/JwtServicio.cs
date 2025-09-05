using E_CommerceSalesCars.Infraestructura.DTOs.UsuarioDTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Infraestructura.AuthJWT
{
    public class JwtServicio : IJwtServicio
    {
        private readonly string _claveSecreta;

        public JwtServicio(IConfiguration configuracion)
        {
            _claveSecreta = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            if (string.IsNullOrEmpty(_claveSecreta))
                throw new Exception("No se encontró la variable de entorno JWT_SECRET_KEY.");
        }

        public string GenerarTokenJwt(UsuarioJwtDTO usuario)
        {
            var manejadorToken = new JwtSecurityTokenHandler();
            var clave = Encoding.ASCII.GetBytes(_claveSecreta);

            var descriptorToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("id", usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Correo)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(clave),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = manejadorToken.CreateToken(descriptorToken);
            return manejadorToken.WriteToken(token);
        }
    }
}
