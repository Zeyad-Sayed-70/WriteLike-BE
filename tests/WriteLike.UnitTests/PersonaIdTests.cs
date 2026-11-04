using WriteLike.Domain.Personas;
using Xunit;

namespace WriteLike.UnitTests;

public sealed class PersonaIdTests
{
    [Fact]
    public void SameGuid_Produces_EqualIds()
    {
        var guid = Guid.NewGuid();
        Assert.Equal(new PersonaId(guid), new PersonaId(guid));
    }

    [Fact]
    public void DifferentGuids_Produce_DifferentIds()
    {
        Assert.NotEqual(PersonaId.New(), PersonaId.New());
    }
}
