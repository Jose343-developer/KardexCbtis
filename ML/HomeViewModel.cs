using System.Collections.Generic;

namespace ML
{
    public class HomeViewModel
    {
        public int AlumnosCount { get; set; }
        public int PrimerSemestre { get; set; }
        public int SegundoSemestre { get; set; }
        public int TercerSemestre { get; set; }
        public int CuartoSemestre { get; set; }
        public int QuintoSemestre { get; set; }
        public int SextoSemestre { get; set; }
        public List<object>? Noticias { get; set; }
        public Noticia? NoticiaRelevante { get; set; }
    }
}
