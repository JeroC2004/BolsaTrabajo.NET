// Guía de estudio: Estructura de una serie del gráfico: reúne su identificación visual y los valores numéricos que se dibujan.
namespace BlazorApp.Client.Shared
{
   
    public class SerieGrafico
    {
        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; } = "#6c757d";
        public List<double> Valores { get; set; } = new();
    }
}
