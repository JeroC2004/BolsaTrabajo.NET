namespace Domain.Model
{
    public class AlumnoCriteria
    {
        public string Texto { get; private set; }

        public AlumnoCriteria(string texto)
        {
            Texto = texto.Trim();
        }
    }
}
