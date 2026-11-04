using WriteLike.Domain.Personas;
using WriteLike.Application.Ports;

namespace WriteLike.Application.Features.Personas;

/// <summary>Query: fetch one persona for the persona cards / chat panel.</summary>
public sealed record GetPersonaQuery(PersonaId PersonaId);

/// <summary>The persona as the outside world sees it.</summary>
public sealed record PersonaDto(PersonaId Id, string Name, PersonaKind Kind);

/// <summary>
/// Use-case handler pattern: one query/command record + one sealed handler,
/// constructor-injected ports only (never EF/Redis/HTTP directly).
/// </summary>
public sealed class GetPersonaHandler(IPersonaRepository personas)
{
    public async Task<PersonaDto?> HandleAsync(GetPersonaQuery query, CancellationToken cancellationToken)
    {
        var persona = await personas.FindAsync(query.PersonaId, cancellationToken);
        return persona is null ? null : new PersonaDto(persona.Id, persona.Name, persona.Kind);
    }
}
