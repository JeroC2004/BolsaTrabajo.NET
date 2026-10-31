// Guía de estudio: Lee el contenido Base64Url del JWT y su vencimiento para las interfaces. Leer claims no verifica la firma: la validación de seguridad corresponde a la API.
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace API.Clients
{
 
    public static class JwtClaimsReader
    {
        // Decodifica los claims del JWT para consultarlos en el cliente; esta lectura no comprueba su firma.
        public static IReadOnlyList<Claim> ReadClaims(string? token)
        {
            var claims = new List<Claim>();

            if (string.IsNullOrWhiteSpace(token))
                return claims;

            string[] partes = token.Split('.');
            if (partes.Length < 2)
                return claims;

            try
            {
                string json = Encoding.UTF8.GetString(DecodeBase64Url(partes[1]));
                using JsonDocument documento = JsonDocument.Parse(json);

                foreach (JsonProperty propiedad in documento.RootElement.EnumerateObject())
                {
                    if (propiedad.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement item in propiedad.Value.EnumerateArray())
                            claims.Add(new Claim(propiedad.Name, item.ToString()));
                    }
                    else
                    {
                        claims.Add(new Claim(propiedad.Name, propiedad.Value.ToString()));
                    }
                }
            }
            catch (Exception ex) when (ex is FormatException || ex is JsonException)
            {
                claims.Clear();
            }

            return claims;
        }

        // Consulta el claim de expiración para determinar si el token ya venció.
        public static bool IsExpired(IEnumerable<Claim> claims)
        {
            string? exp = claims.FirstOrDefault(c => c.Type == "exp")?.Value;

            return long.TryParse(exp, out long segundos)
                && DateTimeOffset.FromUnixTimeSeconds(segundos) <= DateTimeOffset.UtcNow;
        }

        // Adapta Base64Url y su relleno para decodificar el contenido del token.
        private static byte[] DecodeBase64Url(string valor)
        {
            string base64 = valor.Replace('-', '+').Replace('_', '/');

            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
