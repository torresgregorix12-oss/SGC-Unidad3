using System;
using System.Collections.Generic;

namespace SGC_Unidad3
{
    public class HorarioCurso
    {
        public int Id { get; set; }
        public DateTime FechaInicioFin { get; set; }
        public DateTime FechaFin { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public string DiasSemana { get; set; }
        public ICollection<Curso> Cursos { get; set; }

        public HorarioCurso()
        {
            Cursos = new List<Curso>();
        }

        public HorarioCurso(int id, DateTime fechaInicio, DateTime fechaFin,
            TimeSpan horaInicio, string diasSemana) : this()
        {
            Id = id;
            FechaInicioFin = fechaInicio;
            FechaFin = fechaFin;
            HoraInicio = horaInicio;
            DiasSemana = diasSemana;
        }

        public override string ToString()
        {
            return $"{DiasSemana} | {HoraInicio:hh\\:mm} | {FechaInicioFin:dd/MM/yyyy} - {FechaFin:dd/MM/yyyy}";
        }
    }
}
