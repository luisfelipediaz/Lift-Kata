using AwesomeAssertions;

namespace Lift.Tests;

public class UnitTest1
{
    [Fact]
    public void Lift_CanOpenDoors()
    {
        var lift = new Lift();

        lift.OpenDoors();

        lift.AreDoorsOpen.Should().BeTrue();
    }
}

public class Lift
{
    public void OpenDoors()
    {
        AreDoorsOpen = true;
    }

    public bool AreDoorsOpen { get; private set; }
}