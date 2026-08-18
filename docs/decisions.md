# Technical Decisions

## Pure C# learning engine

Scheduling and mastery remain deterministic and framework-independent. A separate Python service would add deployment complexity without improving this use case.

## Scheduling and mastery are different signals

Spaced repetition answers when; Bayesian mastery answers whether. XP and streaks cannot change either calculation.

## Client-side execution

Bounded Sandpack/WASM runners reduce exposure of the main API to arbitrary learner code.

## Managed identity, owned authorization

Supabase issues JWTs; the .NET API validates them and applies its own policies. PostgreSQL RLS remains defense in depth.

## ADRs as project memory

Eighteen recorded decisions preserve context, alternatives, and consequences.
