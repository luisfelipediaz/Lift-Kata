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

    [Fact]
    public void Lift_CanCloseDoors()
    {
        var lift = new Lift();
        lift.OpenDoors();

        lift.CloseDoors();
        
        lift.AreDoorsOpen.Should().BeFalse();
    }
}

public class Lift
{
    public bool AreDoorsOpen { get; private set; }
    
    public void OpenDoors()
    {
        AreDoorsOpen = true;
    }

    public void CloseDoors()
    {
    }
}