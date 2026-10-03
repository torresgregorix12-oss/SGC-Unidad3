using System;
using System.Collections.Generic;

namespace SGC_Unidad3
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public ICollection<Curso> Cursos { get; set; }
        public ICollection<Cobro> Cobros { get; set; }

        public int Edad
        {
            get
            {
                DateTime hoy = DateTime.Today;
                int edad = hoy.Year - FechaNacimiento.Year;
                if (FechaNacimiento.Date > hoy.AddYears(-edad))
                    edad--;
                return edad;
            }
        }

        public Estudiante()
        {
            Cursos = new List<Curso>();
            Cobros = new List<Cobro>();
        }

        public Estudiante(int id, string nombreCompleto, DateTime fechaNacimiento,
            DateTime fechaRegistro, string telefono) : this()
        {
            Id = id;
            NombreCompleto = nombreCompleto;
            FechaNacimiento = fechaNacimiento;
            FechaRegistro = fechaRegistro;
            Telefono = telefono;
        }
    }
}
