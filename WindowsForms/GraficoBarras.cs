namespace WindowsForms
{
    
    internal sealed record SerieGrafico(string Nombre, Color Color, IReadOnlyList<double> Valores);


    internal sealed partial class GraficoBarras : Panel
    {
        private const int AnchoEtiquetas = 240;
        private const int AltoBarra = 22;
        private const int Separacion = 8;
        private const int MargenSuperior = 56;
        private const int MargenDerecho = 56;

        private string titulo = string.Empty;
        private List<string> etiquetas = new();
        private List<SerieGrafico> series = new();

        public GraficoBarras()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            AutoScroll = true;
            BackColor = Color.White;
        }

        public void SetDatos(string titulo, IEnumerable<string> etiquetas, IEnumerable<SerieGrafico> series)
        {
            this.titulo = titulo;
            this.etiquetas = etiquetas.ToList();
            this.series = series.ToList();

            AutoScrollMinSize = new Size(0, MargenSuperior + this.etiquetas.Count * (AltoBarra + Separacion) + 10);
            Invalidate();
        }

        private static double ValorEn(SerieGrafico serie, int indice) =>
            indice < serie.Valores.Count ? serie.Valores[indice] : 0;

        private double TotalEn(int indice) => series.Sum(serie => ValorEn(serie, indice));

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);

            using var fuenteTitulo = new Font(Font, FontStyle.Bold);
            g.DrawString(titulo, fuenteTitulo, Brushes.Black, 8, 6);

            double maximo = etiquetas.Count == 0 ? 0 : Enumerable.Range(0, etiquetas.Count).Max(indice => TotalEn(indice));

            if (maximo <= 0)
            {
                g.DrawString("No hay datos para graficar.", Font, Brushes.Gray, 8, 34);
                return;
            }

            
            int xLeyenda = 8;
            foreach (var serie in series)
            {
                using var pincel = new SolidBrush(serie.Color);
                g.FillRectangle(pincel, xLeyenda, 34, 12, 12);
                g.DrawString(serie.Nombre, Font, Brushes.Black, xLeyenda + 16, 32);
                xLeyenda += 16 + (int)g.MeasureString(serie.Nombre, Font).Width + 18;
            }

            int anchoDisponible = Math.Max(60, ClientSize.Width - AnchoEtiquetas - MargenDerecho - 20);

            using var formatoEtiqueta = new StringFormat
            {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };

            using var formatoValor = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            for (int i = 0; i < etiquetas.Count; i++)
            {
                int y = MargenSuperior + i * (AltoBarra + Separacion);

                var rectEtiqueta = new RectangleF(4, y, AnchoEtiquetas - 8, AltoBarra);
                g.DrawString(etiquetas[i], Font, Brushes.Black, rectEtiqueta, formatoEtiqueta);

                float x = AnchoEtiquetas + 8;
                foreach (var serie in series)
                {
                    double valor = ValorEn(serie, i);
                    if (valor <= 0)
                        continue;

                    float ancho = (float)(valor * anchoDisponible / maximo);
                    using var pincel = new SolidBrush(serie.Color);
                    g.FillRectangle(pincel, x, y, ancho, AltoBarra);

                    if (ancho >= 22)
                    {
                        g.DrawString(valor.ToString("0"), Font, Brushes.White, new RectangleF(x, y, ancho, AltoBarra), formatoValor);
                    }

                    x += ancho;
                }

                g.DrawString(TotalEn(i).ToString("0"), Font, Brushes.Black, x + 6, y + 3);
            }
        }
    }
}
