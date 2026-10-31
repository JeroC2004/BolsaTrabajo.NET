// Guía de estudio: Agrupa el texto usado para buscar alumnos, separando los criterios de consulta de la entidad Alumno.
namespace Domain.Model
{
    public class AlumnoCriteria
    {
        public string Texto { get; private set; }

        // Construye la entidad a partir de los datos recibidos y aplica sus reglas de inicialización.
        public AlumnoCriteria(string texto)
        {
            Texto = texto.Trim();
        }
    }
}
