using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Dominio.Interfaces;
using E_CommerceSalesCars.Infraestructura.AuthJWT;
using E_CommerceSalesCars.Infraestructura.Servicios;
using E_CommerceSalesCars.Persistencia.Data;
using E_CommerceSalesCars.Persistencia.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_CommerceSalesCars
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // === Servicios base ===
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // === Configuración de base de datos ===
            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("StrConn")));

            // === Repositorios ===
            builder.Services.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
            builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            builder.Services.AddScoped<IRepositorioTransaccion, RepositorioTransaccion>();
            builder.Services.AddScoped<IRepositorioPublicacion, RepositorioPublicacion>();
            builder.Services.AddScoped<IRepositorioOferta, RepositorioOferta>();

            // === Servicios de aplicación ===
            builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
            builder.Services.AddScoped<IJwtServicio, JwtServicio>();
            builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();
            builder.Services.AddScoped<IServicioTransaccion, ServicioTransaccion>();
            builder.Services.AddScoped<IServicioPublicacion, ServicioPublicacion>();
            builder.Services.AddScoped<IServicioOferta, ServicioOferta>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            // === Configuración de JWT ===
            var claveSecreta = builder.Configuration["JWT_SECRET_KEY"]
                               ?? Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            var issuer = builder.Configuration["JWT_ISSUER"]
                         ?? Environment.GetEnvironmentVariable("JWT_ISSUER");

            var audience = builder.Configuration["JWT_AUDIENCE"]
                           ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            if (string.IsNullOrEmpty(claveSecreta))
                throw new Exception("No se encontró la clave JWT (ni en secrets.json ni en variables de entorno).");

            if (string.IsNullOrEmpty(issuer))
                throw new Exception("No se encontró el Issuer JWT (ni en secrets.json ni en variables de entorno).");

            if (string.IsNullOrEmpty(audience))
                throw new Exception("No se encontró el Audience JWT (ni en secrets.json ni en variables de entorno).");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(claveSecreta)
                        )
                    };
                });

            var app = builder.Build();

            // === Pipeline de la aplicación ===
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
