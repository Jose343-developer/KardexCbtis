using System;

namespace ML
{
    public class Noticia
    {
        public int IdNoticia { get; set; }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public string? ImagenBase64 { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public bool? Estatus { get; set; }
    }
}
