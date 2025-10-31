using AwesomeAssertions;
using Domain;

namespace Lift.Tests;

public class LiftSystemUnitTests
{
    private readonly Domain.Lift _lift;
    private readonly LiftSystem _system;

    public LiftSystemUnitTests()
    {
        _lift = new Domain.Lift();
        _system = new LiftSystem(_lift);
    }

    [Fact]
    public void LiftSystem_MovesToRequestedFloor()
    {
        _system.Request(2);
        ExecuteLiftTicks(1);

        _lift.CurrentFloor.Should().Be(2);
        _lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_MovesToRequestedFloorAndOpenDoors()
    {
        _system.Request(4);
        ExecuteLiftTicks(4);

        _lift.CurrentFloor.Should().Be(4);
        _lift.AreDoorsOpen.Should().BeTrue();
    }

    [Fact]
    public void List_MovesToRequestedFloor_BelowToInitialFloor()
    {
        var lift = new Domain.Lift(initialFloor: 7);
        var system = new LiftSystem(lift);
        system.Request(1);
        system.Tick();

        lift.CurrentFloor.Should().Be(6);
        lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_FinishTheRequest_When_ArriveToDestinationAndOpenTheDoors()
    {
        _system.Request(2);
        ExecuteLiftTicks(2);

        _lift.CurrentFloor.Should().Be(2);
        _lift.AreDoorsOpen.Should().BeTrue();
        _system.HasNoPendingRequest().Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_ShouldNotFinishTheRequest_When_DidntArriveToTheRequestedFloor()
    {
        _system.Request(3);
        ExecuteLiftTicks(1);

        _lift.CurrentFloor.Should().Be(2);
        _lift.AreDoorsOpen.Should().BeFalse();
        _system.HasNoPendingRequest().Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_ShouldNotFinishTheRequest_When_ArriveToTheRequestedFloorButDidNotOpenTheDoors()
    {
        _system.Request(2);
        ExecuteLiftTicks(1);

        _lift.CurrentFloor.Should().Be(2);
        _lift.AreDoorsOpen.Should().BeFalse();
        _system.HasNoPendingRequest().Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_ShouldCloseTheDoorsBeforeToStartARequest()
    {
        _system.Request(2);
        ExecuteLiftTicks(2);
        _system.Request(1);
        ExecuteLiftTicks(3);

        _lift.CurrentFloor.Should().Be(1);
        _lift.AreDoorsOpen.Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_LiftSystem_ShouldCloseAndMoveInTwoDifferentMoments()
    {
        _system.Request(2);
        ExecuteLiftTicks(2);
        _system.Request(5);
        ExecuteLiftTicks(2);
        _lift.CurrentFloor.Should().Be(3);
        _lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_ShouldDoNothing_When_FinishARequest_And_ThereIsNoMoreRequest()
    {
        _system.Request(2);
        ExecuteLiftTicks(3);

        _system.HasNoPendingRequest().Should().BeTrue();
        _lift.CurrentFloor.Should().Be(2);
        _lift.AreDoorsOpen.Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_MovesToCalledFloor()
    {
        _system.Call(3, Direction.Up);
        _system.Tick();

        _lift.CurrentFloor.Should().Be(2);
        _lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_MovesToCalledFloorAndOpenDoors()
    {
        _system.Call(3, Direction.Up);
        ExecuteLiftTicks(3);

        _lift.CurrentFloor.Should().Be(3);
        _lift.AreDoorsOpen.Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_CantHandleARequest_While_HasACall()
    {
        var caller = () => _system.Request(9);
        _system.Call(4, Direction.Up);
        ExecuteLiftTicks(2);
        caller.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void LiftSystem_CantHandlerACall_While_HasARequest()
    {
        var caller = () => _system.Call(7, Direction.Up);
        _system.Request(4);
        ExecuteLiftTicks(2);
        caller.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void LiftSystem_ShouldDoNothing_When_FinishACall_And_ThereIsNoMoreCalls()
    {
        _system.Call(4, Direction.Up);
        ExecuteLiftTicks(5);

        _system.HasNoPendingCalls().Should().BeTrue();
        _lift.IsInFloor(4).Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_ShouldCloseTheDoorsBeforeToStartACall()
    {
        _system.Call(2, Direction.Up);
        ExecuteLiftTicks(2);
        _system.Call(5, Direction.Up);
        ExecuteLiftTicks(2);
        _lift.IsInFloor(3).Should().BeTrue();
        _lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void List_MovesToCalledFloor_BelowToInitialFloor()
    {
        var lift = new Domain.Lift(initialFloor: 7);
        var system = new LiftSystem(lift);
        system.Call(5, Direction.Up);
        system.Tick();
        system.Tick();
        system.Tick();

        system.HasNoPendingCalls().Should().BeTrue();
        lift.IsInFloor(5).Should().BeTrue();
        lift.AreDoorsOpen.Should().BeTrue();
    }

    [Theory]
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(16)]
    public void Lift_CantMoveOutOfTheBoundariesAbove(int floor)
    {
        var caller = () => _system.Request(floor);

        caller.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    public void Lift_CantMoveOutOfTheBoundariesBelow(int floor)
    {
        var caller = () => _system.Request(floor);

        caller.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Lift_CantMoveInOtherDirectionThanWasCalled_When_DirectionIsUp()
    {
        _system.Call(2, Direction.Up);
        ExecuteLiftTicks(2);

        var caller = () => _system.Request(1);
        
        caller.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Lift_CantMoveInOtherDirectionThanWasCalled_When_DirectionInDown()
    {
        _system.Call(5, Direction.Down);
        ExecuteLiftTicks(5);

        var caller = () => _system.Request(6);
        
        caller.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Lift_ShouldClearCallConstraintWhenItFinish()
    {
        _system.Call(2, Direction.Up);
        ExecuteLiftTicks(2);
        _system.Request(3);
        ExecuteLiftTicks(3);
        _system.Request(1);
        ExecuteLiftTicks(4);
        
        _lift.CurrentFloor.Should().Be(1);
        _lift.AreDoorsOpen.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(11)]
    [InlineData(12)]
    public void Lift_CantBeCalledOutOfBoundaries(int floor)
    {
        var caller = () => _system.Call(floor, Direction.Up);
        
        caller.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    private void ExecuteLiftTicks(int times)
    {
        for (var i = 0; i < times; i++)
            _system.Tick();
    }
}