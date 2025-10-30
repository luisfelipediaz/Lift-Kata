using AwesomeAssertions;

namespace Lift.Tests;

public class LiftUnitTests
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

    [Fact]
    public void Lift_CanMove()
    {
        var lift = new Lift();

        lift.MoveTo(3);

        lift.CurrentFloor.Should().Be(3);
    }
}

public class Lift
{
    public bool AreDoorsOpen { get; private set; }
    public int CurrentFloor { get; private set; }

    public void OpenDoors()
    {
        AreDoorsOpen = true;
    }

    public void CloseDoors()
    {
        AreDoorsOpen = false;
    }

    public void MoveTo(int floor)
    {
        CurrentFloor = floor;
    }
}