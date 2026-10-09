namespace API.Clients
{
    public static class ApiConfiguration
    {
        private const string EnvVarName = "BOLSATRABAJO_API_BASE_URL";
        private const string DefaultBaseUrl = "http://localhost:5183/";

        public static string BaseUrl
        {
            get
            {
                string? envUrl = Environment.GetEnvironmentVariable(EnvVarName);
                string url = string.IsNullOrWhiteSpace(envUrl) ? DefaultBaseUrl : envUrl;
                return url.EndsWith('/') ? url : url + "/";
            }
        }
    }
}