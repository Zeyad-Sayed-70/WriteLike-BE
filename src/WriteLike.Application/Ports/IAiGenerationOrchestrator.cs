using WriteLike.Application.Abstractions;

namespace WriteLike.Application.Ports;

/// <summary>
/// The AI generation pipeline port: absorb persona → classify prompt → plan blocks →
/// ask gap questions → generate block-by-block → combine to markdown. Follow-ups version the text.
/// Implemented in WriteLike.Ai around the OpenCode runtime; Application never knows which model ran.
/// </summary>
public interface IAiGenerationOrchestrator
{
    Task<GenerationPlan> PlanAsync(GenerationRequest request, CancellationToken cancellationToken);

    Task<string> GenerateBlockAsync(
        GenerationPlan plan,
        int blockIndex,
        string? userAnswer,
        CancellationToken cancellationToken);
}
