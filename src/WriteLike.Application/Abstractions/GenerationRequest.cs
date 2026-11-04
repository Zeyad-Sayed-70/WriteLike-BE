using WriteLike.Domain.Ai;
using WriteLike.Domain.Personas;

namespace WriteLike.Application.Abstractions;

/// <summary>
/// What the user asked the persona agent for (their first message, or a follow-up).
/// </summary>
public sealed record GenerationRequest(PersonaId PersonaId, string UserPrompt);
