# Arquitectura pública

Sharply usa Clean Architecture y frontend feature-first.

| Área | Decisión |
| --- | --- |
| Domain | Reglas puras sin frameworks |
| Application | CQRS, FSRS/BKT y puertos |
| Infrastructure | EF Core, Supabase y adaptadores |
| API | Minimal APIs, JWT y composición |
| Web | React, Query, i18n y runners client-side |

La ejecución de código del alumno ocurre en un entorno acotado del cliente. La API conserva progreso, maestría y reglas del dominio. Los DTO son explícitos y los errores siguen ProblemDetails.
