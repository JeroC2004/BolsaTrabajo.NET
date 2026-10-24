namespace BlazorApp.Client.Shared
{
   
    public class SerieGrafico
    {
        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; } = "#6c757d";
        public List<double> Valores { get; set; } = new();
    }
}
