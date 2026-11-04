using WriteLike.Domain.Personas;

namespace WriteLike.Application.Ports;

/// <summary>
/// Read side for personas (writer/singer cards shown on the homepage).
/// Implemented in Infrastructure (Postgres, cached in Redis).
/// </summary>
public interface IPersonaRepository
{
    Task<Persona?> FindAsync(PersonaId id, CancellationToken cancellationToken);
}
