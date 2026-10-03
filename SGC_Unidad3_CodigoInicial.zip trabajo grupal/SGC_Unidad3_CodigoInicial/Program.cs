using System;
using System.Globalization;
using System.Linq;

namespace SGC_Unidad3
{
    internal static class Program
    {
        private static GestorAcademico gestor;

        private static void Main()
        {
            Console.Title = "Sistema de Gestión de Cursos (SGC)";
            gestor = new GestorAcademico();
            gestor.CargarDatos();

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("   SISTEMA DE GESTIÓN DE CURSOS (SGC)");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Estudiantes");
                Console.WriteLine("2. Cursos");
                Console.WriteLine("3. Inscripciones");
                Console.WriteLine("4. Reportes");
                Console.WriteLine("5. Cobros");
                Console.WriteLine("0. Salir");

                int opcion = LeerEntero("Seleccione una opción: ");

                switch (opcion)
                {
                    case 1: MenuEstudiantes(); break;
                    case 2: MenuCursos(); break;
                    case 3: MenuInscripciones(); break;
                    case 4: MenuReportes(); break;
                    case 5: MenuCobros(); break;
                    case 0:
                        gestor.GuardarDatos();
                        salir = true;
                        Console.WriteLine("Datos guardados. Hasta luego.");
                        break;
                    default:
                        Mensaje("Opción inválida. Seleccione una opción del menú.");
                        break;
                }
            }
        }

        private static void MenuEstudiantes()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== ESTUDIANTES ===");
                Console.WriteLine("1. Registrar");
                Console.WriteLine("2. Buscar");
                Console.WriteLine("3. Actualizar");
                Console.WriteLine("4. Eliminar");
                Console.WriteLine("5. Listar");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Opción: ");

                switch (opcion)
                {
                    case 1:
                        int id = LeerEnteroPositivo("ID del estudiante: ");
                        string nombre = LeerTexto("Nombre completo: ");
                        DateTime nacimiento = LeerFecha("Fecha de nacimiento (dd/MM/yyyy): ");
                        DateTime ingreso = LeerFecha("Fecha de ingreso (dd/MM/yyyy): ");
                        string telefono = LeerTexto("Teléfono: ");
                        MostrarResultado(gestor.RegistrarEstudiante(id, nombre, nacimiento, ingreso, telefono));
                        break;

                    case 2:
                        id = LeerEnteroPositivo("ID a buscar: ");
                        Estudiante estudiante = gestor.BuscarEstudiante(id);
                        if (estudiante == null)
                            Mensaje("No se encontró un estudiante con ese ID.");
                        else
                            MostrarEstudiante(estudiante);
                        break;

                    case 3:
                        id = LeerEnteroPositivo("ID del estudiante: ");
                        nombre = LeerTexto("Nuevo nombre: ");
                        nacimiento = LeerFecha("Nueva fecha de nacimiento (dd/MM/yyyy): ");
                        ingreso = LeerFecha("Nueva fecha de ingreso (dd/MM/yyyy): ");
                        telefono = LeerTexto("Nuevo teléfono: ");
                        MostrarResultado(gestor.ActualizarEstudiante(id, nombre, nacimiento, ingreso, telefono));
                        break;

                    case 4:
                        id = LeerEnteroPositivo("ID a eliminar: ");
                        MostrarResultado(gestor.EliminarEstudiante(id));
                        break;

                    case 5:
                        var estudiantes = gestor.ListarEstudiantes();
                        if (!estudiantes.Any())
                            Mensaje("No hay estudiantes registrados.");
                        else
                            foreach (var e in estudiantes) MostrarEstudiante(e);
                        break;

                    case 0: break;
                    default: Mensaje("Opción inválida."); break;
                }
            } while (opcion != 0);
        }

        private static void MenuCursos()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== CURSOS ===");
                Console.WriteLine("1. Registrar");
                Console.WriteLine("2. Actualizar");
                Console.WriteLine("3. Eliminar");
                Console.WriteLine("4. Listar");
                Console.WriteLine("5. Buscar");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Opción: ");

                switch (opcion)
                {
                    case 1:
                        int id = LeerEnteroPositivo("ID del curso: ");
                        string nombre = LeerTexto("Nombre: ");
                        string descripcion = LeerTexto("Descripción: ");
                        decimal costo = LeerDecimalNoNegativo("Costo: ");
                        int duracion = LeerEnteroPositivo("Duración en semanas: ");
                        string unidad = LeerTexto("Unidad de duración (ej. semanas): ");
                        string frecuencia = LeerTexto("Frecuencia de cuota (ej. mensual): ");
                        bool activo = LeerSiNo("¿Curso activo? (S/N): ");
                        var horario = CrearHorario();
                        MostrarResultado(gestor.RegistrarCurso(id, nombre, descripcion, costo, duracion, unidad, frecuencia, activo, horario));
                        break;

                    case 2:
                        id = LeerEnteroPositivo("ID del curso: ");
                        nombre = LeerTexto("Nuevo nombre: ");
                        descripcion = LeerTexto("Nueva descripción: ");
                        costo = LeerDecimalNoNegativo("Nuevo costo: ");
                        duracion = LeerEnteroPositivo("Nueva duración: ");
                        unidad = LeerTexto("Nueva unidad de duración: ");
                        frecuencia = LeerTexto("Nueva frecuencia de cuota: ");
                        activo = LeerSiNo("¿Curso activo? (S/N): ");
                        horario = CrearHorario();
                        MostrarResultado(gestor.ActualizarCurso(id, nombre, descripcion, costo, duracion, unidad, frecuencia, activo, horario));
                        break;

                    case 3:
                        id = LeerEnteroPositivo("ID del curso: ");
                        MostrarResultado(gestor.EliminarCurso(id));
                        break;

                    case 4:
                        var cursos = gestor.ListarCursos();
                        if (!cursos.Any())
                            Mensaje("No hay cursos registrados.");
                        else
                            foreach (var c in cursos) MostrarCurso(c);
                        break;

                    case 5:
                        id = LeerEnteroPositivo("ID del curso: ");
                        Curso curso = gestor.BuscarCurso(id);
                        if (curso == null)
                            Mensaje("No se encontró un curso con ese ID.");
                        else
                            MostrarCurso(curso);
                        break;

                    case 0: break;
                    default: Mensaje("Opción inválida."); break;
                }
            } while (opcion != 0);
        }

        private static HorarioCurso CrearHorario()
        {
            Console.WriteLine("--- Horario del curso ---");
            int id = LeerEnteroPositivo("ID del horario: ");
            DateTime inicio = LeerFecha("Fecha inicio (dd/MM/yyyy): ");
            DateTime fin = LeerFecha("Fecha fin (dd/MM/yyyy): ");
            TimeSpan hora = LeerHora("Hora de inicio (HH:mm): ");
            string dias = LeerTexto("Días (ej. Lunes y Miércoles): ");
            return new HorarioCurso(id, inicio, fin, hora, dias);
        }

        private static void MenuInscripciones()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== INSCRIPCIONES ===");
                Console.WriteLine("1. Inscribir estudiante");
                Console.WriteLine("2. Consultar estudiantes por curso");
                Console.WriteLine("3. Consultar cursos por estudiante");
                Console.WriteLine("4. Retirar inscripción");
                Console.WriteLine("5. Comprobar inscripción");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Opción: ");

                switch (opcion)
                {
                    case 1:
                        int estudianteId = LeerEnteroPositivo("ID del estudiante: ");
                        int cursoId = LeerEnteroPositivo("ID del curso: ");
                        MostrarResultado(gestor.Inscribir(estudianteId, cursoId));
                        break;

                    case 2:
                        cursoId = LeerEnteroPositivo("ID del curso: ");
                        var inscritos = gestor.EstudiantesPorCurso(cursoId);
                        if (!inscritos.Any())
                            Mensaje("No hay estudiantes inscritos o el curso no existe.");
                        else
                            foreach (var e in inscritos) MostrarEstudiante(e);
                        break;

                    case 3:
                        estudianteId = LeerEnteroPositivo("ID del estudiante: ");
                        var cursos = gestor.CursosPorEstudiante(estudianteId);
                        if (!cursos.Any())
                            Mensaje("El estudiante no tiene cursos inscritos o no existe.");
                        else
                            foreach (var c in cursos) MostrarCurso(c);
                        break;

                    case 4:
                        estudianteId = LeerEnteroPositivo("ID del estudiante: ");
                        cursoId = LeerEnteroPositivo("ID del curso: ");
                        MostrarResultado(gestor.RetirarInscripcion(estudianteId, cursoId));
                        break;

                    case 5:
                        estudianteId = LeerEnteroPositivo("ID del estudiante: ");
                        cursoId = LeerEnteroPositivo("ID del curso: ");
                        Mensaje(EstaInscrito(estudianteId, cursoId)
                            ? "El estudiante está inscrito en el curso."
                            : "El estudiante NO está inscrito en el curso.");
                        break;

                    case 0: break;
                    default: Mensaje("Opción inválida."); break;
                }
            } while (opcion != 0);
        }

        private static void MenuCobros()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== COBROS ===");
                Console.WriteLine("1. Registrar cobro");
                Console.WriteLine("2. Listar cobros de estudiante");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Opción: ");

                switch (opcion)
                {
                    case 1:
                        int estudianteId = LeerEnteroPositivo("ID del estudiante: ");
                        bool esInscripcion = LeerSiNo("¿Es cobro de inscripción? (S/N): ");
                        int cantidadCursos = LeerEnteroPositivo("Cantidad de cursos incluidos en el cobro: ");

                        var cursos = new System.Collections.Generic.List<Curso>();
                        for (int i = 0; i < cantidadCursos; i++)
                        {
                            int cursoId = LeerEnteroPositivo($"ID del curso #{i + 1}: ");
                            Curso curso = gestor.BuscarCurso(cursoId);
                            if (curso == null)
                            {
                                Mensaje("Curso inexistente. Operación cancelada.");
                                cursos.Clear();
                                break;
                            }
                            cursos.Add(curso);
                        }

                        if (cursos.Any())
                        {
                            DateTime fecha = LeerFecha("Fecha del cobro (dd/MM/yyyy): ");
                            MostrarResultado(gestor.RegistrarCobro(estudianteId, fecha, esInscripcion, cursos));
                        }
                        break;

                    case 2:
                        estudianteId = LeerEnteroPositivo("ID del estudiante: ");
                        var cobros = gestor.CobrosPorEstudiante(estudianteId);
                        if (!cobros.Any())
                            Mensaje("No existen cobros para ese estudiante.");
                        else
                            foreach (var cobro in cobros)
                                Console.WriteLine(cobro);
                        break;

                    case 0: break;
                    default: Mensaje("Opción inválida."); break;
                }
            } while (opcion != 0);
        }

        private static void MenuReportes()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== REPORTES ===");
                Console.WriteLine("1. Resumen general");
                Console.WriteLine("2. Estudiantes inscritos por curso");
                Console.WriteLine("3. Cursos activos");
                Console.WriteLine("4. Total cobrado por estudiante");
                Console.WriteLine("0. Volver");

                opcion = LeerEntero("Opción: ");

                switch (opcion)
                {
                    case 1:
                        Console.WriteLine(gestor.GenerarResumen());
                        break;

                    case 2:
                        int cursoId = LeerEnteroPositivo("ID del curso: ");
                        var inscritos = gestor.EstudiantesPorCurso(cursoId);
                        Console.WriteLine($"Total de inscritos: {inscritos.Count()}");
                        foreach (var e in inscritos) Console.WriteLine($"- {e.Id}: {e.NombreCompleto}");
                        break;

                    case 3:
                        var activos = gestor.ListarCursos().Where(c => c.Activo);
                        foreach (var c in activos) Console.WriteLine($"- {c.Id}: {c.Nombre}");
                        break;

                    case 4:
                        int estudianteId = LeerEnteroPositivo("ID del estudiante: ");
                        decimal total = gestor.CobrosPorEstudiante(estudianteId).Sum(c => c.Total);
                        Console.WriteLine($"Total cobrado: {total:C2}");
                        break;

                    case 0: break;
                    default: Mensaje("Opción inválida."); break;
                }
            } while (opcion != 0);
        }

        private static bool EstaInscrito(int estudianteId, int cursoId)
        {
            return gestor.EstaInscrito(estudianteId, cursoId);
        }

        private static void MostrarEstudiante(Estudiante e)
        {
            Console.WriteLine($"ID: {e.Id} | Nombre: {e.NombreCompleto} | Edad: {e.Edad} | Tel: {e.Telefono}");
            Console.WriteLine($"   Cursos inscritos: {e.Cursos.Count}");
        }

        private static void MostrarCurso(Curso c)
        {
            Console.WriteLine($"ID: {c.Id} | {c.Nombre} | Costo: {c.CostoTotal:C2} | Duración: {c.Duracion} {c.UnidadDuracion}");
            Console.WriteLine($"   Activo: {(c.Activo ? "Sí" : "No")} | Frecuencia: {c.FrecuenciaCuota} | Inscritos: {c.Estudiantes.Count}");
        }

        private static string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string valor = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(valor))
                    return valor.Trim();

                Console.WriteLine("Entrada obligatoria.");
            }
        }

        private static int LeerEntero(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (int.TryParse(Console.ReadLine(), out int valor))
                    return valor;

                Console.WriteLine("Debe introducir un número entero válido.");
            }
        }

        private static int LeerEnteroPositivo(string mensaje)
        {
            while (true)
            {
                int valor = LeerEntero(mensaje);
                if (valor > 0)
                    return valor;

                Console.WriteLine("El valor debe ser mayor que cero.");
            }
        }

        private static decimal LeerDecimalNoNegativo(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine();
                if (decimal.TryParse(entrada, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal valor) && valor >= 0)
                    return valor;

                Console.WriteLine("Introduzca un valor decimal válido y no negativo.");
            }
        }

        private static DateTime LeerFecha(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime fecha))
                    return fecha;

                Console.WriteLine("Formato inválido. Use dd/MM/yyyy.");
            }
        }

        private static TimeSpan LeerHora(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (TimeSpan.TryParseExact(Console.ReadLine(), @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan hora))
                    return hora;

                Console.WriteLine("Formato inválido. Use HH:mm.");
            }
        }

        private static bool LeerSiNo(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string valor = Console.ReadLine()?.Trim().ToUpperInvariant();
                if (valor == "S") return true;
                if (valor == "N") return false;
                Console.WriteLine("Responda S o N.");
            }
        }

        private static void MostrarResultado(string resultado)
        {
            Console.WriteLine(resultado);
            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();
        }

        private static void Mensaje(string mensaje)
        {
            Console.WriteLine(mensaje);
            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}
