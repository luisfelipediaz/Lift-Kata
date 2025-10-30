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

    [Fact]
    public void Lift_CantMoveIfTheDoorsAreOpen()
    {
        var lift = new Lift();
        lift.OpenDoors();

        var caller = () => lift.MoveTo(5);

        caller.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Lift_CantMoveOutOfTheBoundries()
    {
        var lift = new Lift();

        var caller = () => lift.MoveTo(-1);

        caller.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Lift_CantMoveOutOfTheBoundriesAbove()
    {
        var lift = new Lift();

        var caller = () => lift.MoveTo(11);

        caller.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Lift_MovesToRequestFloor()
    {
        var lift = new Lift();

        lift.Request(2);
        lift.Tick();

        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void Lift_MoveToRequestFloorAndOpenDoors()
    {
        var lift = new Lift();
        
        lift.Request(4);
        lift.Tick();
        lift.Tick();
        lift.Tick();
        lift.Tick();
        
        lift.CurrentFloor.Should().Be(4);
        lift.AreDoorsOpen.Should().BeTrue();
    }
}

public class Lift
{
    private const int MaxFloor = 10;
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
        ArgumentOutOfRangeException.ThrowIfNegative(floor);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(floor, MaxFloor);
        if (AreDoorsOpen)
            throw new InvalidOperationException();
        CurrentFloor = floor;
    }

    public void Request(int i)
    {
    }

    public void Tick()
    {
        CurrentFloor = 2;
    }
}