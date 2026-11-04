using WriteLike.Application.Abstractions;
using WriteLike.Application.Ports;
using WriteLike.Domain.Ai;

namespace WriteLike.Ai.Pipeline;

/// <summary>
/// Placeholder implementation of the OpenCode-backed generation pipeline.
/// The real pipeline lives here in future steps:
///   absorb persona .md dataset → classify prompt → plan blocks → ask gap questions
///   (the agent must ASK, never assume) → generate block-by-block → combine to markdown
///   → follow-ups create new versions of the text.
/// Per-user model selection and API keys for non-free models are passed in via the
/// pipeline options, never persisted here.
/// </summary>
internal sealed class OpenCodeGenerationOrchestrator : IAiGenerationOrchestrator
{
    public Task<GenerationPlan> PlanAsync(GenerationRequest request, CancellationToken cancellationToken)
        => throw new NotImplementedException(
            "OpenCode pipeline not wired yet — implement persona absorption + planning here.");

    public Task<string> GenerateBlockAsync(
        GenerationPlan plan,
        int blockIndex,
        string? userAnswer,
        CancellationToken cancellationToken)
        => throw new NotImplementedException(
            "OpenCode pipeline not wired yet — implement block generation here.");
}
