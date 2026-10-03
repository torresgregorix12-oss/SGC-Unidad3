# Sistema de Gestión de Cursos (SGC)

Proyecto de la Actividad Grupal Integradora - Unidad 3, INF-512.

## Opción
Opción 4 - Sistema de Gestión de Cursos.

## Tecnologías
- C#
- .NET Framework 4.8.1
- Newtonsoft.Json
- Contenedor principal: `List<T>`

## Clases principales
- `Estudiante`
- `Curso`
- `HorarioCurso`
- `Cobro`
- `DatosAcademicos`
- `GestorAcademico`
- `Program`

La relación Estudiante-Curso es muchos a muchos mediante `ICollection<Curso>` e
`ICollection<Estudiante>`, tal como está planteada en el UML de la memoria.

## Funcionalidades
- Registrar, buscar, actualizar, eliminar y listar estudiantes.
- Registrar, actualizar, eliminar y listar cursos.
- Inscribir y retirar estudiantes de cursos.
- Evitar inscripciones duplicadas.
- Consultar estudiantes por curso y cursos por estudiante.
- Comprobar si un estudiante está inscrito.
- Registrar y consultar cobros.
- Generar reportes.
- Validar entradas.
- Guardar y cargar datos mediante `datos.json`.

## Ejecución
Abrir `SGC_Unidad3.sln` con Visual Studio que tenga soporte para
.NET Framework 4.8.1 y restaurar el paquete NuGet `Newtonsoft.Json`.

Ejecutar con `Ctrl + F5` o `F5`.

## Trabajo colaborativo
Cada integrante debe realizar al menos 2 commits significativos y reales en el
repositorio. No se deben inventar commits; cada commit debe corresponder a un
cambio real realizado por ese integrante.
