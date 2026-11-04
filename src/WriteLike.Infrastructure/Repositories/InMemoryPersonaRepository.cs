using WriteLike.Domain.Personas;
using WriteLike.Application.Ports;

namespace WriteLike.Infrastructure.Repositories;

/// <summary>
/// Placeholder port implementation until the EF Core (Postgres) repository lands.
/// Kept working so the pipeline and endpoints can be built without a database.
/// </summary>
internal sealed class InMemoryPersonaRepository : IPersonaRepository
{
    public Task<Persona?> FindAsync(PersonaId id, CancellationToken cancellationToken)
        => Task.FromResult<Persona?>(null);
}
