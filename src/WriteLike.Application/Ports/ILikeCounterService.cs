using WriteLike.Domain.Texts;

namespace WriteLike.Application.Ports;

/// <summary>
/// High-concurrency like counters (Redis INCR + scheduled reconciliation to Postgres —
/// accurate under 100k+ simultaneous likes). Implemented in Infrastructure.
/// </summary>
public interface ILikeCounterService
{
    Task<long> IncrementAsync(TextId textId, CancellationToken cancellationToken);

    Task<long> ReadAsync(TextId textId, CancellationToken cancellationToken);
}
