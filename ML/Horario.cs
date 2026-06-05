using System;
using System.Collections.Generic;

namespace ML
{
    public class Horario
    {
        public int IdHorario { get; set; }
        public int IdGrupo { get; set; }
        public string? DocumentoRuta { get; set; }

        public ML.Grupo? Grupo { get; set; }

        // Useful for form submissions
        public List<object>? Grupos { get; set; }
        public List<object>? Horarios { get; set; }
    }
}
