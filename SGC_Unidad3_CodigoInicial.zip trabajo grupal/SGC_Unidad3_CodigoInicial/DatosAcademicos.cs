using System.Collections.Generic;

namespace SGC_Unidad3
{
    public class DatosAcademicos
    {
        public List<Estudiante> Estudiantes { get; set; }
        public List<Curso> Cursos { get; set; }
        public List<Cobro> Cobros { get; set; }

        public DatosAcademicos()
        {
            Estudiantes = new List<Estudiante>();
            Cursos = new List<Curso>();
            Cobros = new List<Cobro>();
        }
    }
}
