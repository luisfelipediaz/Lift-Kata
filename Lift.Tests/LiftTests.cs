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
    public void Lift_CanMoveUpOneFloorAtTime()
    {
        var lift = new Lift();

        lift.MoveUp();

        lift.CurrentFloor.Should().Be(2);
    }

    [Fact]
    public void Lift_CanMoveDownOneFloorAtTime()
    {
        var lift = new Lift(initialFloor: 2);

        lift.MoveDown();

        lift.CurrentFloor.Should().Be(1);
    }

    [Fact]
    public void Lift_CantMoveUpIfTheDoorsAreOpen()
    {
        var lift = new Lift();
        lift.OpenDoors();

        var caller = () => lift.MoveUp();

        caller.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Lift_CantMoveDownIfTheDoorsAreOpen()
    {
        var lift = new Lift(initialFloor: 8);
        lift.OpenDoors();

        var caller = () => lift.MoveDown();

        caller.Should().Throw<InvalidOperationException>();
    }
}