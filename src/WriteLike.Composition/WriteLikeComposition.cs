using Microsoft.Extensions.DependencyInjection;
using WriteLike.Ai;
using WriteLike.Application.Features.Personas;
using WriteLike.Infrastructure;

namespace WriteLike.Composition;

/// <summary>
/// The composition root — the ONLY place where Infrastructure and Ai are referenced.
/// Api calls AddWriteLike() and gets a fully wired system; it sees no Infrastructure/Ai types.
/// </summary>
public static class WriteLikeComposition
{
    public static IServiceCollection AddWriteLike(this IServiceCollection services)
        => services
            .AddInfrastructure()
            .AddAiPipeline()
            .AddSingleton<GetPersonaHandler>();
}
