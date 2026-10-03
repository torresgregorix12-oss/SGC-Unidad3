using System;
using System.Collections.Generic;
using System.Linq;

namespace SGC_Unidad3
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public int Duracion { get; set; }
        public string UnidadDuracion { get; set; }
        public string FrecuenciaCuota { get; set; }
        public bool Activo { get; set; }
        public HorarioCurso Horario { get; set; }
        public ICollection<Estudiante> Estudiantes { get; set; }

        public int NumeroCuotas
        {
            get
            {
                if (Duracion <= 0 || Costo <= 0)
                    return 0;

                return FrecuenciaCuota != null &&
                       FrecuenciaCuota.ToLowerInvariant().Contains("mens")
                    ? (int)Math.Ceiling(Duracion / 4.0)
                    : 1;
            }
        }

        public decimal CostoTotal
        {
            get { return Costo; }
        }

        public Curso()
        {
            Estudiantes = new List<Estudiante>();
        }

        public Curso(int id, string nombre, string descripcion, decimal costo,
            int duracion, string unidadDuracion, string frecuenciaCuota,
            bool activo, HorarioCurso horario) : this()
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Costo = costo;
            Duracion = duracion;
            UnidadDuracion = unidadDuracion;
            FrecuenciaCuota = frecuenciaCuota;
            Activo = activo;
            Horario = horario;
        }
    }
}
