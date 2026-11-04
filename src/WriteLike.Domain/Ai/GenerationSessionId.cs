namespace WriteLike.Domain.Ai;

/// <summary>
/// Strongly-typed id for one generation run of the AI pipeline
/// (session created on first message → plan → blocks → combined output → follow-up versions).
/// </summary>
public readonly record struct GenerationSessionId(Guid Value)
{
    public static GenerationSessionId New() => new(Guid.NewGuid());
}
