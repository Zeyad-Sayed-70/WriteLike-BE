using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using Xunit;
using RefAssembly = System.Reflection.Assembly;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace WriteLike.ArchitectureTests;

/// <summary>
/// Naming and placement conventions that keep AI agents putting code where it belongs.
/// If one of these fails, move the code — never the rule.
/// </summary>
public sealed class PlacementAndNamingTests
{
    private static readonly RefAssembly Application =
        typeof(WriteLike.Application.AssemblyReference).Assembly;
    private static readonly RefAssembly Infrastructure =
        typeof(WriteLike.Infrastructure.AssemblyReference).Assembly;
    private static readonly RefAssembly Api =
        typeof(WriteLike.Api.AssemblyReference).Assembly;

    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(Application, Infrastructure, Api)
        .Build();

    private static readonly IObjectProvider<IType> ApplicationPortsLayer =
        Types().That().ResideInNamespace("WriteLike.Application.Ports").As("Application Ports");

    private static readonly IObjectProvider<IType> InfrastructureLayer =
        Types().That().ResideInAssembly(Infrastructure).As("Infrastructure");

    private static readonly IObjectProvider<IType> ApiLayer =
        Types().That().ResideInAssembly(Api).As("Api");

    [Fact]
    public void RepositoryInterfacesMustLiveInApplicationPorts()
        => Interfaces().That().HaveNameContaining("Repository").Should()
            .Be(ApplicationPortsLayer)
            .Because("Ports belong to Application; Infrastructure only IMPLEMENTS them")
            .Check(Architecture);

    [Fact]
    public void RepositoryImplementationsMustLiveInInfrastructure()
        => Classes().That().HaveNameContaining("Repository").Should()
            .Be(InfrastructureLayer)
            .Because("Repositories (EF Core/Redis) are port implementations in Infrastructure")
            .Check(Architecture);

    [Fact]
    public void EndpointGroupsMustLiveInApi()
        => Classes().That().HaveNameContaining("Endpoints").Should()
            .Be(ApiLayer)
            .Because("HTTP endpoint groups are delivery concerns and live in Api")
            .Check(Architecture);

    [Fact]
    public void HandlersMustBeSealed()
        => Classes().That().HaveNameContaining("Handler").Should()
            .BeSealed()
            .Because("Use-case handlers are not extension points; seal them")
            .Check(Architecture);
}
