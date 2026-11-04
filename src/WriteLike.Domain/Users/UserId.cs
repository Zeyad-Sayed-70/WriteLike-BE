namespace WriteLike.Domain.Users;

/// <summary>Strongly-typed id for a platform user.</summary>
public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
}
