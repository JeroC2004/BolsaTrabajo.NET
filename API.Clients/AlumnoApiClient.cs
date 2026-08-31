using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class AlumnoApiClient : BaseApiClient
    {
        public static async Task<AlumnoDTO> GetAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync($"alumnos/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<AlumnoDTO>())!;

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener alumno con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public static async Task<IEnumerable<AlumnoDTO>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("alumnos");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<AlumnoDTO>>())!;

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de alumnos. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public static async Task<IEnumerable<AlumnoDTO>> GetByCriteriaAsync(string texto)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync($"alumnos/criteria?texto={Uri.EscapeDataString(texto)}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<AlumnoDTO>>()) ?? new List<AlumnoDTO>();

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al buscar alumnos. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public static async Task AddAsync(AlumnoDTO alumno)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PostAsJsonAsync("alumnos", alumno);

            if (!response.IsSuccessStatusCode)
            {
                await HandleUnauthorizedResponseAsync(response);
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear alumno. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public static async Task UpdateAsync(AlumnoDTO alumno)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PutAsJsonAsync("alumnos", alumno);

            if (!response.IsSuccessStatusCode)
            {
                await HandleUnauthorizedResponseAsync(response);
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar alumno con Id {alumno.Id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public static async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.DeleteAsync($"alumnos/{id}");

            if (!response.IsSuccessStatusCode)
            {
                await HandleUnauthorizedResponseAsync(response);
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar alumno con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }
    }
}
