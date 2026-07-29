namespace Domain.Model
{
    public class OfertaCriteria
    {
        public string Texto { get; private set; }

        public OfertaCriteria(string texto)
        {
            Texto = texto.Trim();
        }
    }
}
