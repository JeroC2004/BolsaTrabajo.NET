// Guía de estudio: Enumeración de estados posibles de una oferta; se usa para controlar su disponibilidad y mostrar su situación.
namespace Domain.Model
{
    public enum EstadoOferta
    {
        Activa = 1,
        Pausada = 2,
        Cerrada = 3,
        Finalizada = 4
    }
}
