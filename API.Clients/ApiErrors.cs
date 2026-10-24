using System.Net;
using System.Text.Json;

namespace API.Clients
{
    
    internal static class ApiErrors
    {
        public static async Task<Exception> CrearExcepcionAsync(HttpResponseMessage response, string accion)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                return new Exception("No tiene permisos para realizar esta operación.");

            string contenido = await response.Content.ReadAsStringAsync();
            string mensaje = ExtraerMensaje(contenido);

            return new Exception(string.IsNullOrWhiteSpace(mensaje)
                ? $"{accion}. Status: {response.StatusCode}"
                : mensaje);
        }

        private static string ExtraerMensaje(string contenido)
        {
            try
            {
                using JsonDocument documento = JsonDocument.Parse(contenido);

                if (documento.RootElement.ValueKind == JsonValueKind.Object
                    && documento.RootElement.TryGetProperty("error", out JsonElement error)
                    && error.ValueKind == JsonValueKind.String)
                {
                    return error.GetString() ?? string.Empty;
                }
            }
            catch (JsonException)
            {
            }

            return string.Empty;
        }
    }
}
