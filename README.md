# WriteLike — Enforced Solution Scaffold

A .NET 10 solution skeleton where **architectural boundaries are enforced by the build, not by convention** — designed for repos where AI coding agents (Codex, Cursor, Claude Code, Copilot, Gemini CLI) contribute code.

**Everything in this scaffold was built and verified**: build 0 warnings / 0 errors (warnings are errors), 9 architecture rules + 3 unit/integration tests passing, `dotnet format --verify-no-changes` clean, locked-mode restore working. Three deliberate violations ("canaries") were committed during setup to prove each guardrail fires; re-run them after major changes.

## Prerequisites

- .NET SDK 10 (`dotnet --version` → 10.x)
- Git

## Everyday commands

```bash
dotnet restore WriteLike.slnx --locked-mode   # CI restores this way: dependency graph cannot drift
dotnet build WriteLike.slnx -c Debug         # 0 warnings / 0 errors — warnings ARE errors
dotnet format WriteLike.slnx                 # fix formatting locally
dotnet format WriteLike.slnx --verify-no-changes   # CI gate: formatting must be committed clean
dotnet test WriteLike.slnx -c Debug          # includes the architecture rulebook
```

## The enforcement layers (see AGENTS.md for the rules themselves)

| Layer | Where | What it catches |
| --- | --- | --- |
| Project graph + `DisableTransitiveProjectReferences` | `Directory.Build.props`, csprojs | Forbidden project references; transitively "inheriting" references |
| Build-time reference matrix | `Directory.Build.props` → `EnforceReferenceMatrix` target | A declared-but-unused forbidden reference (fails the BUILD, not just tests) |
| Banned API analyzer (`RS0030`) | per-project `BannedSymbols.txt` + `build/BannedSymbols.Global.txt` | EF/HTTP/Redis/ASP.NET usage in the wrong layer — compile errors with human-readable guidance |
| Architecture tests (ArchUnitNET) | `tests/WriteLike.ArchitectureTests` | Type-level layer violations, port placement, naming/placement conventions |
| Public API analyzer | `PublicAPI.*.txt` in Domain & Application | Accidental public-surface drift (renames/moves/new publics break the build) |
| Central Package Management + lockfiles | `Directory.Packages.props`, `packages.lock.json` | Package version drift, invented versions, dependency-graph sneaks |
| `nuget.config` package source mapping | root | Unapproved package feeds |
| CI | `.github/workflows/ci.yml` | All of the above, on every PR |
| CODEOWNERS + branch protection | GitHub settings | Agents editing the guardrails themselves |

**Note (verified empirically):** `Microsoft.CodeAnalysis.BannedApiAnalyzers` does **not** support namespace bans (`N:` lines are silently ignored). Ban concrete *gateway types* (`T:`) instead — lines for packages the project doesn't reference are preemptive and bite the moment the package is added.

## Canary drills — run these after setup, and whenever a new agent onboards

```bash
# 1. Banned API in Domain → expect: error RS0030: 'HttpClient' is banned in this project: Domain does no I/O.
cat > src/WriteLike.Domain/DontDoThis.cs <<'EOF'
using System.Net.Http;
namespace WriteLike.Domain;
internal static class OutboundCaller { public static readonly HttpClient Client = new(); }
EOF
dotnet build src/WriteLike.Domain -c Debug   # must FAIL
rm src/WriteLike.Domain/DontDoThis.cs

# 2. Api uses an Infrastructure type → expect: CS0234 (Api has no reference; transitive refs disabled)
#    (add typeof(WriteLike.Infrastructure.AssemblyReference) to Program.cs, build Api, revert)

# 3. Forbidden reference added → expect: error ARCHITECTURE VIOLATION: WriteLike.Api must not reference WriteLike.Infrastructure
dotnet add src/WriteLike.Api reference src/WriteLike.Infrastructure
dotnet build src/WriteLike.Api -c Debug     # must FAIL
sed -i '/WriteLike.Infrastructure/d' src/WriteLike.Api/WriteLike.Api.csproj
```

## GitHub setup (do this once)

1. Replace `@your-org/architects` in `CODEOWNERS` with your real team.
2. Branch protection on `main`: require pull request before merging, **require approvals ≥ 1** (CODEOWNERS blocks self-approval on guard files), require status checks (`enforce` job), dismiss stale approvals, forbid force pushes.
3. Commit the `packages.lock.json` files — CI restores with `--locked-mode`.

## What is intentionally left to implement

- The OpenCode pipeline in `src/WriteLike.Ai/Pipeline/` (placeholder orchestrator throws `NotImplementedException`)
- EF Core (Postgres) and Redis implementations in `src/WriteLike.Infrastructure` (in-memory placeholder + ports exist)
- Testcontainers harness in `src/.../IntegrationTests`
