namespace WriteLike.Api.Features;

/// <summary>
/// Endpoint-group pattern: one static class per feature, mapping that feature's routes.
/// Handlers/models come from Application; Api contains no business logic.
/// </summary>
public static class HealthEndpoints
{
    public static IEndpointRouteBuilder MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
        return app;
    }
}
