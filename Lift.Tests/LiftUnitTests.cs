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

    public void MoveTo(int floor)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(floor);
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
    private void MoveUp() => CurrentFloor++;
    private void MoveDown() => CurrentFloor--;
}