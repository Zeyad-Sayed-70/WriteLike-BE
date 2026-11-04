using WriteLike.Application.Abstractions;

namespace WriteLike.Application.Ports;

/// <summary>
/// Notification fan-out with anti-flood behavior (aggregation, digests, capped inboxes).
/// Implemented in Infrastructure (SignalR + background dispatcher + Redis counters).
/// </summary>
public interface INotificationDispatcher
{
    Task DispatchAsync(TextNotification notification, CancellationToken cancellationToken);
}
