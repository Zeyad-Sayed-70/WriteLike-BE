using WriteLike.Domain.Texts;

namespace WriteLike.Application.Abstractions;

/// <summary>
/// An event worth telling a user about (comment, like, etc.) — the notification
/// service fans these out; aggregation and anti-flood live behind the port.
/// </summary>
public sealed record TextNotification(TextId TextId, string Reason, DateTimeOffset OccurredAt);
