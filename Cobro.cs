using System;
using System.Collections.Generic;
using System.Linq;

namespace SGC_Unidad3
{
    public class Cobro
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public Estudiante Estudiante { get; set; }
        public bool EsInscripcion { get; set; }
        public ICollection<Curso> Cursos { get; set; }

        public decimal Total
        {
            get { return Cursos == null ? 0m : Cursos.Sum(c => c.Costo); }
        }

        public Cobro()
        {
            Cursos = new List<Curso>();
        }

        public Cobro(int id, DateTime fecha, Estudiante estudiante,
            bool esInscripcion, IEnumerable<Curso> cursos) : this()
        {
            Id = id;
            Fecha = fecha;
            Estudiante = estudiante;
            EsInscripcion = esInscripcion;
            foreach (var curso in cursos)
                Cursos.Add(curso);
        }

        public override string ToString()
        {
            string tipo = EsInscripcion ? "Inscripción" : "Cuota";
            string nombres = Cursos == null ? "" : string.Join(", ", Cursos.Select(c => c.Nombre));
            return $"Cobro #{Id} | {Fecha:dd/MM/yyyy} | {tipo} | Cursos: {nombres} | Total: {Total:C2}";
        }
    }
}
