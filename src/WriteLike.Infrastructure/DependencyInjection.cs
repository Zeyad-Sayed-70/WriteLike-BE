using Microsoft.Extensions.DependencyInjection;
using WriteLike.Application.Ports;
using WriteLike.Infrastructure.Notifications;
using WriteLike.Infrastructure.Repositories;

namespace WriteLike.Infrastructure;

/// <summary>
/// Wires data-plane port implementations (EF Core/Postgres, Redis, SignalR fan-out).
/// Called ONLY from WriteLike.Composition. Api can never call this directly —
/// it cannot even see this project's types.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Placeholder wiring: real implementations (EF Core, Redis counters, SignalR)
        // replace these registrations as features land.
        services.AddSingleton<IPersonaRepository, InMemoryPersonaRepository>();
        services.AddSingleton<INotificationDispatcher, NoOpNotificationDispatcher>();
        return services;
    }
}
