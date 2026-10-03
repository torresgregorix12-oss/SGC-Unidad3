using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace SGC_Unidad3
{
    public class GestorAcademico
    {
        private const string RutaDatos = "datos.json";
        private DatosAcademicos datos;

        public GestorAcademico()
        {
            datos = new DatosAcademicos();
        }

        public List<Estudiante> CargarEstudiantes()
        {
            return datos.Estudiantes;
        }

        public void GuardarEstudiantes()
        {
            GuardarDatos();
        }

        public List<Curso> CargarCursos()
        {
            return datos.Cursos;
        }

        public void GuardarCursos()
        {
            GuardarDatos();
        }

        public List<Cobro> CargarCobros()
        {
            return datos.Cobros;
        }

        public void GuardarCobros()
        {
            GuardarDatos();
        }

        public DatosAcademicos CargarDatos()
        {
            if (!File.Exists(RutaDatos))
            {
                datos = new DatosAcademicos();
                return datos;
            }

            try
            {
                string json = File.ReadAllText(RutaDatos);
                datos = JsonConvert.DeserializeObject<DatosAcademicos>(json)
                        ?? new DatosAcademicos();
                ReconstruirRelaciones();
            }
            catch
            {
                datos = new DatosAcademicos();
            }

            return datos;
        }

        public void GuardarDatos(DatosAcademicos datosExternos = null)
        {
            if (datosExternos != null)
                datos = datosExternos;

            var configuracion = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(datos, configuracion);
            File.WriteAllText(RutaDatos, json);
        }

        public string RegistrarEstudiante(int id, string nombre, DateTime nacimiento,
            DateTime ingreso, string telefono)
        {
            if (BuscarEstudiante(id) != null)
                return "No se puede registrar: ya existe un estudiante con ese ID.";

            if (nacimiento.Date >= DateTime.Today)
                return "La fecha de nacimiento debe ser anterior a hoy.";

            datos.Estudiantes.Add(new Estudiante(id, nombre, nacimiento, ingreso, telefono));
            GuardarDatos();
            return "Estudiante registrado correctamente.";
        }

        public Estudiante BuscarEstudiante(int id)
        {
            return datos.Estudiantes.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Estudiante> ListarEstudiantes()
        {
            return datos.Estudiantes;
        }

        public string ActualizarEstudiante(int id, string nombre, DateTime nacimiento,
            DateTime ingreso, string telefono)
        {
            Estudiante e = BuscarEstudiante(id);
            if (e == null)
                return "No existe el estudiante indicado.";

            e.NombreCompleto = nombre;
            e.FechaNacimiento = nacimiento;
            e.FechaRegistro = ingreso;
            e.Telefono = telefono;
            GuardarDatos();
            return "Estudiante actualizado correctamente.";
        }

        public string EliminarEstudiante(int id)
        {
            Estudiante e = BuscarEstudiante(id);
            if (e == null)
                return "No existe el estudiante indicado.";

            foreach (var curso in e.Cursos.ToList())
                curso.Estudiantes.Remove(e);

            datos.Estudiantes.Remove(e);
            GuardarDatos();
            return "Estudiante eliminado correctamente.";
        }

        public string RegistrarCurso(int id, string nombre, string descripcion,
            decimal costo, int duracion, string unidadDuracion,
            string frecuenciaCuota, bool activo, HorarioCurso horario)
        {
            if (BuscarCurso(id) != null)
                return "No se puede registrar: ya existe un curso con ese ID.";

            if (horario.FechaFin < horario.FechaInicioFin)
                return "La fecha final del horario no puede ser anterior a la fecha inicial.";

            datos.Cursos.Add(new Curso(id, nombre, descripcion, costo, duracion,
                unidadDuracion, frecuenciaCuota, activo, horario));
            GuardarDatos();
            return "Curso registrado correctamente.";
        }

        public Curso BuscarCurso(int id)
        {
            return datos.Cursos.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Curso> ListarCursos()
        {
            return datos.Cursos;
        }

        public string ActualizarCurso(int id, string nombre, string descripcion,
            decimal costo, int duracion, string unidadDuracion,
            string frecuenciaCuota, bool activo, HorarioCurso horario)
        {
            Curso c = BuscarCurso(id);
            if (c == null)
                return "No existe el curso indicado.";

            c.Nombre = nombre;
            c.Descripcion = descripcion;
            c.Costo = costo;
            c.Duracion = duracion;
            c.UnidadDuracion = unidadDuracion;
            c.FrecuenciaCuota = frecuenciaCuota;
            c.Activo = activo;
            c.Horario = horario;
            GuardarDatos();
            return "Curso actualizado correctamente.";
        }

        public string EliminarCurso(int id)
        {
            Curso c = BuscarCurso(id);
            if (c == null)
                return "No existe el curso indicado.";

            foreach (var estudiante in c.Estudiantes.ToList())
                estudiante.Cursos.Remove(c);

            datos.Cursos.Remove(c);
            GuardarDatos();
            return "Curso eliminado correctamente.";
        }

        public bool EstaInscrito(int estudianteId, int cursoId)
        {
            Estudiante estudiante = BuscarEstudiante(estudianteId);
            Curso curso = BuscarCurso(cursoId);

            return estudiante != null &&
                   curso != null &&
                   estudiante.Cursos.Any(c => c.Id == cursoId);
        }

        public string Inscribir(int estudianteId, int cursoId)
        {
            Estudiante estudiante = BuscarEstudiante(estudianteId);
            Curso curso = BuscarCurso(cursoId);

            if (estudiante == null)
                return "No existe el estudiante indicado.";

            if (curso == null)
                return "No existe el curso indicado.";

            if (!curso.Activo)
                return "No se puede inscribir: el curso está inactivo.";

            if (EstaInscrito(estudianteId, cursoId))
                return "No se puede inscribir: el estudiante ya está inscrito en ese curso.";

            estudiante.Cursos.Add(curso);
            curso.Estudiantes.Add(estudiante);
            GuardarDatos();

            return "Inscripción realizada correctamente.";
        }

        public string RetirarInscripcion(int estudianteId, int cursoId)
        {
            Estudiante estudiante = BuscarEstudiante(estudianteId);
            Curso curso = BuscarCurso(cursoId);

            if (estudiante == null)
                return "No existe el estudiante indicado.";

            if (curso == null)
                return "No existe el curso indicado.";

            if (!EstaInscrito(estudianteId, cursoId))
                return "No existe esa inscripción.";

            estudiante.Cursos.Remove(curso);
            curso.Estudiantes.Remove(estudiante);
            GuardarDatos();

            return "Inscripción retirada correctamente.";
        }

        public IEnumerable<Estudiante> EstudiantesPorCurso(int cursoId)
        {
            Curso curso = BuscarCurso(cursoId);
            return curso == null ? Enumerable.Empty<Estudiante>() : curso.Estudiantes;
        }

        public IEnumerable<Curso> CursosPorEstudiante(int estudianteId)
        {
            Estudiante estudiante = BuscarEstudiante(estudianteId);
            return estudiante == null ? Enumerable.Empty<Curso>() : estudiante.Cursos;
        }

        public string RegistrarCobro(int estudianteId, DateTime fecha,
            bool esInscripcion, IEnumerable<Curso> cursos)
        {
            Estudiante estudiante = BuscarEstudiante(estudianteId);
            if (estudiante == null)
                return "No existe el estudiante indicado.";

            var listaCursos = cursos.Distinct().ToList();
            if (!listaCursos.Any())
                return "Debe incluir al menos un curso.";

            if (!esInscripcion && listaCursos.Any(c => !EstaInscrito(estudianteId, c.Id)))
                return "No se puede registrar la cuota: el estudiante no está inscrito en uno de los cursos.";

            int nuevoId = datos.Cobros.Any() ? datos.Cobros.Max(c => c.Id) + 1 : 1;
            Cobro cobro = new Cobro(nuevoId, fecha, estudiante, esInscripcion, listaCursos);

            datos.Cobros.Add(cobro);
            estudiante.Cobros.Add(cobro);
            GuardarDatos();

            return $"Cobro registrado correctamente. Total: {cobro.Total:C2}";
        }

        public IEnumerable<Cobro> CobrosPorEstudiante(int estudianteId)
        {
            return datos.Cobros.Where(c => c.Estudiante != null && c.Estudiante.Id == estudianteId);
        }

        public string GenerarResumen()
        {
            int totalEstudiantes = datos.Estudiantes.Count;
            int totalCursos = datos.Cursos.Count;
            int cursosActivos = datos.Cursos.Count(c => c.Activo);
            int totalInscripciones = datos.Estudiantes.Sum(e => e.Cursos.Count);
            decimal totalCobrado = datos.Cobros.Sum(c => c.Total);

            return
                "===== RESUMEN GENERAL =====\n" +
                $"Estudiantes: {totalEstudiantes}\n" +
                $"Cursos: {totalCursos}\n" +
                $"Cursos activos: {cursosActivos}\n" +
                $"Inscripciones: {totalInscripciones}\n" +
                $"Total cobrado: {totalCobrado:C2}";
        }

        private void ReconstruirRelaciones()
        {
            foreach (var estudiante in datos.Estudiantes)
            {
                estudiante.Cursos = new List<Curso>();
                estudiante.Cobros = new List<Cobro>();
            }

            foreach (var curso in datos.Cursos)
            {
                curso.Estudiantes = new List<Estudiante>();
            }

            // La persistencia conserva la información de relación dentro del JSON.
            // Aquí se reconstruyen las referencias compartidas por ID.
            foreach (var estudianteGuardado in datos.Estudiantes.ToList())
            {
                foreach (var cursoGuardado in estudianteGuardado.Cursos.ToList())
                {
                    Curso cursoReal = BuscarCurso(cursoGuardado.Id);
                    if (cursoReal != null && !estudianteGuardado.Cursos.Any(c => c.Id == cursoReal.Id))
                    {
                        estudianteGuardado.Cursos.Add(cursoReal);
                        if (!cursoReal.Estudiantes.Any(e => e.Id == estudianteGuardado.Id))
                            cursoReal.Estudiantes.Add(estudianteGuardado);
                    }
                }
            }

            foreach (var cobro in datos.Cobros)
            {
                if (cobro.Estudiante != null)
                {
                    Estudiante estudianteReal = BuscarEstudiante(cobro.Estudiante.Id);
                    cobro.Estudiante = estudianteReal;
                    if (estudianteReal != null && !estudianteReal.Cobros.Any(c => c.Id == cobro.Id))
                        estudianteReal.Cobros.Add(cobro);
                }

                var cursosReales = new List<Curso>();
                foreach (var curso in cobro.Cursos ?? new List<Curso>())
                {
                    Curso cursoReal = BuscarCurso(curso.Id);
                    if (cursoReal != null)
                        cursosReales.Add(cursoReal);
                }
                cobro.Cursos = cursosReales;
            }
        }
    }
}
