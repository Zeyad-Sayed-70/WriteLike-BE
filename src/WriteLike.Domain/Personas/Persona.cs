namespace WriteLike.Domain.Personas;

/// <summary>
/// A persona the AI agent can absorb and imitate (writer or singer).
/// </summary>
public sealed class Persona(PersonaId id, string name, PersonaKind kind)
{
    public PersonaId Id { get; } = id;

    public string Name { get; } = name;

    public PersonaKind Kind { get; } = kind;
}
