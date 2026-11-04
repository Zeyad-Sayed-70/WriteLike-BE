namespace WriteLike.Domain.Personas;

/// <summary>
/// Strongly-typed id for a persona (a famous writer or singer whose .md dataset the agent absorbs).
/// </summary>
public readonly record struct PersonaId(Guid Value)
{
    public static PersonaId New() => new(Guid.NewGuid());
}
