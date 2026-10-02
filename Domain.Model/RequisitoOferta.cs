using System;

namespace Domain.Model
{
    public class RequisitoOferta
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool EsExcluyente { get; set; }
        public int OfertaId { get; set; }
        public Oferta Oferta { get; set; }
    }
}