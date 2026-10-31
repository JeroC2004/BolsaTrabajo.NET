// Guía de estudio: DTO de filtros de AlumnoCriteria: transporta criterios de búsqueda desde las interfaces hasta la API sin representar una entidad persistida.
namespace DTOs
{
    public class AlumnoCriteriaDTO
    {
        public string Texto { get; set; } = string.Empty;
    }
}
