using Data;
using Domain.Model;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly BolsaTrabajoContext context;
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IConfiguration configuration;

        public AuthService(
            BolsaTrabajoContext context,
            IUsuarioRepository usuarioRepository,
            IConfiguration configuration)
        {
            this.context = context;
            this.usuarioRepository = usuarioRepository;
            this.configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }

            Usuario? usuario = await usuarioRepository.GetByUsernameAsync(request.Username);

            if (usuario == null || !usuario.ValidatePassword(request.Password))
                return null;

            var jwtSettings = configuration.GetSection("JwtSettings");
            string secretKey = jwtSettings["SecretKey"]!;
            string? issuer = jwtSettings["Issuer"];
            string? audience = jwtSettings["Audience"];

            int expirationMinutes =
                int.TryParse(jwtSettings["ExpirationMinutes"], out int minutes)
                    ? minutes
                    : 60;

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    usuario.Username),

                new Claim(
                    ClaimTypes.Email,
                    usuario.Email),

                new Claim(
                    "jti",
                    Guid.NewGuid().ToString())
            };

            DateTime expiresAt =
                DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt,
                Username = usuario.Username
            };
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            string username = request.Username.Trim().ToLowerInvariant();
            string email = request.Email.Trim().ToLowerInvariant();
            string legajo = request.Legajo.Trim();
            string dni = request.Dni.Trim();

            bool usernameExiste = await context.Usuarios
                .AnyAsync(u => u.Username.ToLower() == username);

            if (usernameExiste)
            {
                throw new InvalidOperationException(
                    "El nombre de usuario seleccionado ya está en uso.");
            }

            bool emailUsuarioExiste = await context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == email);

            if (emailUsuarioExiste)
            {
                throw new InvalidOperationException(
                    "Ya existe una cuenta registrada con ese correo electrónico.");
            }

            bool emailAlumnoExiste = await context.Alumnos
                .AnyAsync(a => a.Email.ToLower() == email);

            if (emailAlumnoExiste)
            {
                throw new InvalidOperationException(
                    "Ya existe un alumno registrado con ese correo electrónico.");
            }

            bool legajoExiste = await context.Alumnos
                .AnyAsync(a => a.Legajo.ToLower() == legajo.ToLower());

            if (legajoExiste)
            {
                throw new InvalidOperationException(
                    "Ya existe un alumno registrado con ese legajo.");
            }

            bool dniExiste = await context.Alumnos
                .AnyAsync(a => a.Dni == dni);

            if (dniExiste)
            {
                throw new InvalidOperationException(
                    "Ya existe un alumno registrado con ese DNI.");
            }

            bool carreraExiste = await context.Carreras
                .AnyAsync(c => c.Id == request.CarreraId);

            if (!carreraExiste)
            {
                throw new InvalidOperationException(
                    "La carrera seleccionada no existe.");
            }

            if (request.Password != request.ConfirmPassword)
            {
                throw new InvalidOperationException(
                    "Las contraseñas no coinciden.");
            }

            DateTime fechaAlta = DateTime.UtcNow;

            var usuario = new Usuario(
                id: 0,
                username: username,
                email: email,
                password: request.Password,
                fechaCreacion: fechaAlta);

            var alumno = new Alumno(
                id: 0,
                nomAlumno: request.NomAlumno.Trim(),
                apeAlumno: request.ApeAlumno.Trim(),
                email: email,
                legajo: legajo,
                dni: dni,
                plan: request.Plan.Trim(),
                anioCurso: request.AnioCurso,
                cantMatAp: request.CantMatAp,
                promedio: request.Promedio,
                carreraId: request.CarreraId,
                fechaAlta: fechaAlta);

            await using var transaction =
                await context.Database.BeginTransactionAsync();

            try
            {
                context.Usuarios.Add(usuario);
                context.Alumnos.Add(alumno);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
