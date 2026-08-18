# Decisiones técnicas públicas

## C# puro para el motor

FSRS/BKT permanecen deterministas y testeables; no se introduce un servicio Python.

## Scheduling y maestría separados

FSRS responde cuándo repasar; BKT estima si un concepto se domina. XP no altera ninguno.

## Ejecución client-side

Sandpack y C# WASM aíslan ejercicios acotados y reducen exposición del servidor.

## Auth administrada, autorización propia

Supabase emite tokens; la API valida firma/claims y aplica políticas. RLS añade defensa en profundidad.

## ADR como memoria

18 decisiones registran contexto, alternativas y consecuencias para evitar reescrituras impulsivas.
