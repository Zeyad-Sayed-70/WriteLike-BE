# AGENTS.md — WriteLike Backend

This file is the rulebook for **all** AI coding agents working in this repo (Codex, Cursor,
Claude Code, Copilot coding agent, Gemini CLI — they all read AGENTS.md natively).
Every rule below is **also enforced mechanically** by the build and CI. If a build error
contradicts your instinct, the build is right.

## Commands — run before you claim any task is done

```bash
dotnet build WriteLike.slnx -c Debug        # must be 0 warnings / 0 errors (warnings ARE errors)
dotnet format WriteLike.slnx                 # never commit formatting violations
dotnet test WriteLike.slnx -c Debug          # includes WriteLike.ArchitectureTests (the rulebook)
```

If `WriteLike.ArchitectureTests` fails: **your code is in the wrong place. Move it.**
Never edit that project, and never weaken a rule to make a test pass.

## Solution map & dependency matrix

| Project | Role | May reference |
| --- | --- | --- |
| `src/WriteLike.Domain` | Entities, value objects, domain events. **Zero dependencies.** | — |
| `src/WriteLike.Application` | Use cases, DTOs, **ports** (interfaces) | Domain |
| `src/WriteLike.Ai` | OpenCode pipeline: absorb persona → plan → blocks → combine. Implements AI ports | Domain, Application |
| `src/WriteLike.Infrastructure` | EF Core (Postgres), Redis, SignalR, repositories, counters. Implements data ports | Domain, Application |
| `src/WriteLike.Composition` | DI composition root — the ONLY place Infrastructure & Ai are wired | Domain, Application, Ai, Infrastructure |
| `src/WriteLike.Api` | HTTP host: endpoints, auth, SignalR hubs, rate-limit policies | Application, Composition |
| `tests/WriteLike.ArchitectureTests` | Executable architecture rules — **DO NOT EDIT** | all src projects |
| `tests/WriteLike.UnitTests` | Domain & Application unit tests | Domain, Application |
| `tests/WriteLike.IntegrationTests` | Testcontainers tests (Postgres + Redis) | all src projects |

Transitive project references are **disabled** (`DisableTransitiveProjectReferences=true`):
referencing Application does **not** silently give you Domain. Declare every reference you use,
and never add a reference outside this table — move the code instead.

## Where new code goes

| You are adding | It goes in | Pattern |
| --- | --- | --- |
| An entity / value object / domain event | `src/WriteLike.Domain/<Aggregate>/` | sealed class; ids are `readonly record struct` |
| A use case | `src/WriteLike.Application/Features/<Feature>/` | one command record + one handler (sealed) |
| A port (interface) | `src/WriteLike.Application/Ports/` | `I...` name, returns `Task<...>`, takes `CancellationToken` |
| A port implementation | `Infrastructure` (EF/Redis/SignalR) or `Ai` (OpenCode) | `internal sealed`, named after the interface |
| An HTTP endpoint | `src/WriteLike.Api/Features/<Feature>/` | minimal-API group class `<Feature>Endpoints.cs` |
| An AI pipeline step (plan, ask, generate, combine, version) | `src/WriteLike.Ai/Pipeline/` | one class per step |
| A background job / dispatcher | `src/WriteLike.Infrastructure/Jobs/` | hosted service, registered in `AddInfrastructure` |

## Hard rules (violations fail the build — never work around them)

1. **Domain compiles against nothing but the base class library.** EF Core, `System.Net.Http`,
   JSON serialization, DI, logging are all *compile errors* there (`BannedSymbols.txt`).
2. **Application defines ports; Infrastructure and Ai implement them.** Application never
   references EF Core, Redis, `HttpClient`, or SignalR.
3. **Api touches Application and Composition only.** It cannot see Infrastructure or Ai types
   even by accident — the references don't exist and banned-API lists + architecture tests back that up.
4. **Do not add `ProjectReference`s beyond the table above.** Never add one to make a compile
   error go away — move the code to the correct project instead.
5. **Package versions live only in `Directory.Packages.props`** (central package management).
   Inline `Version=` on a `PackageReference` is a NuGet error (NU1008). Adding a new package
   means editing that file → human review (CODEOWNERS).
6. **Never edit these files** (protected by CODEOWNERS; a human must approve):
   `Directory.Build.props`, `Directory.Packages.props`, `nuget.config`, `.editorconfig`,
   `build/`, `**/BannedSymbols.txt`, `**/PublicAPI.*.txt`,
   `tests/WriteLike.ArchitectureTests/`, `CODEOWNERS`, `AGENTS.md`, `.github/`.
   If a rule blocks your change, stop and report the conflict — do not weaken the rule.
7. **Types are `internal` by default.** Making something public is a deliberate API change:
   the `PublicAPI.Unshipped.txt` file must be updated in the same commit.
8. **Warnings are errors** (`TreatWarningsAsErrors`). Fix the warning; never suppress it
   without a written reason in the PR description.
9. **Lockfiles are committed** (`packages.lock.json`). CI restores with `--locked-mode`,
   so dependency changes cannot sneak in.

## Domain context (so you generate correct code)

The product: users pick a persona (famous writer/singer), chat with an agent that absorbs the
persona's `.md` dataset, and generate persona-authentic texts with a visible pipeline:
**absorb → classify prompt (conversational vs. generation) → plan blocks → ask gap questions
(the agent must ASK, never assume) → generate block-by-block → combine to markdown →
follow-ups create new versions.** Chat sessions are ephemeral (Redis); generated texts are
persisted (Postgres) and shareable with read/like/comment counters (Redis-backed, accurate
under 100k+ concurrent likes) and capped, aggregated notifications (SignalR fan-out).
Sign-in is triggered on the first message attempt, preserving the user's typed input.

## When you finish a task

Run the Commands block, then report: what changed, files touched, and the final
`dotnet build` + `dotnet test` summaries. If anything remains unimplemented, say so
explicitly — never silently leave a placeholder.
