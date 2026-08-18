# Public Architecture

Sharply combines Clean Architecture on the backend with a feature-first React client.

| Area | Responsibility |
| --- | --- |
| Domain | Entities, value objects, invariants |
| Application | Commands/queries, ports, scheduling, mastery |
| Infrastructure | EF Core, PostgreSQL, Supabase adapters |
| API | Minimal endpoints, JWT, validation, composition |
| Web | Learning flows, runners, state, i18n |

Domain has no framework dependencies. Application depends on interfaces rather than EF Core or ASP.NET Core. Code execution is isolated in browser/WASM workers while the API owns durable progress and authorization.
