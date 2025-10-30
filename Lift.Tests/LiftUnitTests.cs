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

    [Fact]
    public void Lift_CantMoveDownOutOfTheBoundries()
    {
        var lift = new Lift();

        var caller = () => lift.MoveDown();

        caller.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Lift_CantMoveUpOutOfTheBoundries()
    {
        var lift = new Lift(initialFloor: 10);

        var caller = () => lift.MoveUp();

        caller.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Lift_MovesToRequestedFloor()
    {
        var lift = new Lift();

        lift.Request(2);
        lift.Tick();

        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void Lift_MovesToRequestedFloorAndOpenDoors()
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

    [Fact]
    public void List_MovesToRequestedFloor_BelowToInitialFloor()
    {
        var lift = new Lift(initialFloor: 7);

        lift.Request(1);
        lift.Tick();

        lift.CurrentFloor.Should().Be(6);
        lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void Lift_FinishTheRequest_When_ArriveToDetinationAndOpenTheDoors()
    {
        var lift = new Lift();

        lift.Request(2);
        lift.Tick();
        lift.Tick();

        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeTrue();
        lift.HasPendingRequest().Should().BeFalse();
    }

    [Fact]
    public void Lift_ShouldNotFinishTheRequest_When_DidntArriveToTheRequestedFloor()
    {
        var lift = new Lift();

        lift.Request(3);
        lift.Tick();

        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeFalse();
        lift.HasPendingRequest().Should().BeTrue();
    }

    [Fact]
    public void Lift_ShouldNotFinishTheRequest_When_ArriveToTheRequestedFloorButDidNotOpenTheDoors()
    {
        var lift = new Lift();

        lift.Request(2);
        lift.Tick();

        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeFalse();
        lift.HasPendingRequest().Should().BeTrue();
    }
}

public class Lift(int initialFloor = 1)
{
    private const int MaxFloor = 10;
    private const int MinFloor = 1;
    private int _request;
    public bool AreDoorsOpen { get; private set; }
    public int CurrentFloor { get; private set; } = initialFloor;

    public void OpenDoors()
    {
        AreDoorsOpen = true;
    }

    public void CloseDoors()
    {
        AreDoorsOpen = false;
    }

    public void MoveUp() => MoveTo(CurrentFloor + 1);
    public void MoveDown() => MoveTo(CurrentFloor - 1);

    private void MoveTo(int floor)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(floor, MinFloor);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(floor, MaxFloor);
        if (AreDoorsOpen)
            throw new InvalidOperationException();
        CurrentFloor = floor;
    }

    public void Request(int floor)
    {
        _request = floor;
    }

    public void Tick()
    {
        if (IsOnTheRequestedFloor())
        {
            OpenDoors();
            ClearRequest();
            return;
        }

        Move();
    }

    public bool HasPendingRequest() => _request != 0;

    private void Move()
    {
        if (CurrentFloor > _request)
            MoveDown();
        else
            MoveUp();
    }

    private bool IsOnTheRequestedFloor() => CurrentFloor == _request;
    private void ClearRequest() => _request = 0;
}