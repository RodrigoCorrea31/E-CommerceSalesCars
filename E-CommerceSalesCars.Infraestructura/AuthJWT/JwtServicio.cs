using E_CommerceSalesCars.Infraestructura.AuthJWT;
using E_CommerceSalesCars.Infraestructura.DTOs.UsuarioDTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtServicio : IJwtServicio
{
    private readonly string _claveSecreta;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtServicio(IConfiguration configuracion)
    {
        _claveSecreta = configuracion["JWT_SECRET_KEY"]
                        ?? Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

        _issuer = configuracion["JWT_ISSUER"]
                  ?? Environment.GetEnvironmentVariable("JWT_ISSUER");

        _audience = configuracion["JWT_AUDIENCE"]
                    ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE");

        if (string.IsNullOrEmpty(_claveSecreta))
            throw new Exception("No se encontró la clave JWT.");
        if (string.IsNullOrEmpty(_issuer))
            throw new Exception("No se encontró el Issuer JWT.");
        if (string.IsNullOrEmpty(_audience))
            throw new Exception("No se encontró el Audience JWT.");
    }

    public string GenerarTokenJwt(UsuarioJwtDTO usuario)
    {
        var manejadorToken = new JwtSecurityTokenHandler();
        var clave = Encoding.UTF8.GetBytes(_claveSecreta);

        var descriptorToken = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Correo)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(clave),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        Console.WriteLine($"Clave secreta (len): {_claveSecreta?.Length}");
        Console.WriteLine($"Issuer: {_issuer}");
        Console.WriteLine($"Audience: {_audience}");
        Console.WriteLine($"Usuario Id: {usuario.Id}, Nombre: {usuario.NombreUsuario}, Email: {usuario.Correo}");


        var token = manejadorToken.CreateToken(descriptorToken);
        return manejadorToken.WriteToken(token);
    }
}
