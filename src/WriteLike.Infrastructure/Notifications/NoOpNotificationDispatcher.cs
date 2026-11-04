using WriteLike.Application.Abstractions;
using WriteLike.Application.Ports;

namespace WriteLike.Infrastructure.Notifications;

/// <summary>
/// Placeholder until the real SignalR + Redis fan-out dispatcher lands.
/// The real implementation MUST include anti-flood behavior: aggregation of burst likes,
/// per-user inbox caps, and digest scheduling.
/// </summary>
internal sealed class NoOpNotificationDispatcher : INotificationDispatcher
{
    public Task DispatchAsync(TextNotification notification, CancellationToken cancellationToken)
        => Task.CompletedTask;
}
