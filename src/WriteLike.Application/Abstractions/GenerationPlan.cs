using WriteLike.Domain.Ai;

namespace WriteLike.Application.Abstractions;

/// <summary>
/// The block-by-block plan the AI proposes before writing: which blocks, in what order,
/// and which gaps require a question to the user (the agent must ASK, never assume).
/// </summary>
public sealed record GenerationPlan(GenerationSessionId SessionId, IReadOnlyList<PlannedBlock> Blocks);

/// <summary>One planned block of the final text.</summary>
public sealed record PlannedBlock(string Title, string Purpose, bool RequiresUserInput);
