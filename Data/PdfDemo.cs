using System.Text;

namespace Data
{
    /// <summary>
    /// Genera un PDF mínimo de una página (texto en Helvetica) para los CV de los datos de prueba,
    /// sin depender de ninguna librería externa.
    /// </summary>
    internal static class PdfDemo
    {
        public static byte[] Crear(string titulo, IEnumerable<string> lineas)
        {
            var texto = new StringBuilder();
            texto.Append("BT /F1 18 Tf 56 780 Td (").Append(Escapar(titulo)).Append(") Tj ET\n");
            texto.Append("BT /F1 11 Tf 15 TL 56 750 Td\n");
            foreach (string linea in lineas)
                texto.Append('(').Append(Escapar(linea)).Append(") Tj T*\n");
            texto.Append("ET");

            string contenido = texto.ToString();

            string[] objetos =
            {
                "<< /Type /Catalog /Pages 2 0 R >>",
                "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
                "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>",
                "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>",
                $"<< /Length {contenido.Length} >>\nstream\n{contenido}\nendstream"
            };

            // Todo el documento usa caracteres de 1 byte (Latin1), así que la posición en el
            // texto coincide con el desplazamiento en bytes que exige la tabla xref.
            var pdf = new StringBuilder("%PDF-1.4\n");
            var posiciones = new List<int>();

            for (int i = 0; i < objetos.Length; i++)
            {
                posiciones.Add(pdf.Length);
                pdf.Append($"{i + 1} 0 obj\n{objetos[i]}\nendobj\n");
            }

            int inicioXref = pdf.Length;
            pdf.Append($"xref\n0 {objetos.Length + 1}\n");
            pdf.Append("0000000000 65535 f \n");
            foreach (int posicion in posiciones)
                pdf.Append($"{posicion:D10} 00000 n \n");

            pdf.Append($"trailer\n<< /Size {objetos.Length + 1} /Root 1 0 R >>\nstartxref\n{inicioXref}\n%%EOF\n");

            return Encoding.Latin1.GetBytes(pdf.ToString());
        }

        private static string Escapar(string texto)
        {
            var sb = new StringBuilder(texto.Length);
            foreach (char c in texto)
            {
                if (c > 255) sb.Append('?');
                else if (c == '(' || c == ')' || c == '\\') sb.Append('\\').Append(c);
                else sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
