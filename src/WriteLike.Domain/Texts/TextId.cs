namespace WriteLike.Domain.Texts;

/// <summary>
/// Strongly-typed id for a generated text (the persisted, shareable artifact —
/// distinct from the ephemeral chat session that produced it).
/// </summary>
public readonly record struct TextId(Guid Value)
{
    public static TextId New() => new(Guid.NewGuid());
}
