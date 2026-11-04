using Microsoft.Extensions.DependencyInjection;
using WriteLike.Application.Ports;

namespace WriteLike.Ai;

/// <summary>
/// Wires the AI pipeline into DI. Called ONLY from WriteLike.Composition.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddAiPipeline(this IServiceCollection services)
    {
        services.AddSingleton<IAiGenerationOrchestrator, Pipeline.OpenCodeGenerationOrchestrator>();
        return services;
    }
}
