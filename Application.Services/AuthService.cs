
using Data;
using DTOs;
using Domain.Model;
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
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IConfiguration configuration;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            IConfiguration configuration)
        {
            this.usuarioRepository = usuarioRepository;
            this.configuration = configuration;
        }


        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
                return null;

            var usuario = await usuarioRepository
                .GetByUsernameAsync(request.Username);

            if (usuario == null ||
                !usuario.ValidatePassword(request.Password))
                return null;

            var jwtSettings = configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["SecretKey"]!;
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var expirationMinutes =
                int.TryParse(jwtSettings["ExpirationMinutes"], out var m)
                    ? m
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
                    ClaimTypes.Role,
                    usuario.Rol.ToString()),

                new Claim(
                    "jti",
                    Guid.NewGuid().ToString())
            };

            if (usuario.AlumnoId.HasValue)
            {
                claims.Add(
                    new Claim(
                        AuthClaims.AlumnoId,
                        usuario.AlumnoId.Value.ToString()));
            }

            if (usuario.EmpresaId.HasValue)
            {
                claims.Add(
                    new Claim(
                        AuthClaims.EmpresaId,
                        usuario.EmpresaId.Value.ToString()));
            }

            var expiresAt = DateTime.UtcNow
                .AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler()
                    .WriteToken(token),

                ExpiresAt = expiresAt,

                Username = usuario.Username
            };
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
                request, new System.ComponentModel.DataAnnotations.ValidationContext(request), validationResults, true))
                throw new ArgumentException(string.Join(" ", validationResults.Select(v => v.ErrorMessage)));

            var username = request.Username?.Trim();
            var email = request.Email?.Trim();

            if (string.IsNullOrWhiteSpace(username) || username.Length < 3 || username.Length > 50 ||
                !System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9._-]+$"))
                throw new ArgumentException("El usuario debe tener entre 3 y 50 caracteres y usar solo letras, números, puntos, guiones o guiones bajos.");
            if (string.IsNullOrWhiteSpace(request.NomAlumno) || request.NomAlumno.Trim().Length > 100 ||
                string.IsNullOrWhiteSpace(request.ApeAlumno) || request.ApeAlumno.Trim().Length > 100)
                throw new ArgumentException("El nombre y el apellido son obligatorios y no pueden superar 100 caracteres.");
            if (string.IsNullOrWhiteSpace(email) || email.Length > 150)
                throw new ArgumentException("El email es obligatorio y no puede superar 150 caracteres.");
            if (string.IsNullOrWhiteSpace(request.Legajo) || request.Legajo.Trim().Length > 50 ||
                string.IsNullOrWhiteSpace(request.Dni) ||
                !System.Text.RegularExpressions.Regex.IsMatch(request.Dni.Trim(), @"^\d{8,9}$") ||
                string.IsNullOrWhiteSpace(request.Plan) || request.Plan.Trim().Length > 20)
                throw new ArgumentException("Verificá el legajo, el DNI (8 o 9 dígitos) y el plan.");
            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");
            if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
                throw new ArgumentException("Las contraseñas no coinciden.");
            if (request.AnioCurso is < 1 or > 5)
                throw new ArgumentException("El año de curso debe estar entre 1 y 5.");
            if (request.CantMatAp is < 0 or > 99)
                throw new ArgumentException("La cantidad de materias aprobadas debe estar entre 0 y 99.");
            if (request.Promedio is < 0 or > 10)
                throw new ArgumentException("El promedio debe estar entre 0 y 10.");

            var alumno = new Alumno(
                0,
                request.NomAlumno.Trim(),
                request.ApeAlumno.Trim(),
                email,
                request.Legajo.Trim(),
                request.Dni.Trim(),
                request.Plan.Trim(),
                request.AnioCurso,
                request.CantMatAp,
                request.Promedio,
                request.CarreraId,
                DateTime.UtcNow);

            await usuarioRepository.RegisterAlumnoAsync(alumno, username, request.Password);
        }
    }
}
