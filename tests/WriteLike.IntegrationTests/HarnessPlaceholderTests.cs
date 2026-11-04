using Xunit;

namespace WriteLike.IntegrationTests;

public sealed class HarnessPlaceholderTests
{
    [Fact]
    public void Testcontainers_Harness_Pending()
        => Assert.True(
            true,
            "Placeholder: replace with real Testcontainers-based tests (Postgres + Redis) when the first integration test lands. See AGENTS.md.");
}
