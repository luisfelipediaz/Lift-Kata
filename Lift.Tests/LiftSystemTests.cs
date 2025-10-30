using AwesomeAssertions;

namespace Lift.Tests;

public class LiftSystemUnitTests
{
    [Fact]
    public void LiftSystem_MovesToRequestedFloor()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);

        system.Request(2);
        system.Tick();

        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_MovesToRequestedFloorAndOpenDoors()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);

        system.Request(4);
        system.Tick();
        system.Tick();
        system.Tick();
        system.Tick();

        lift.CurrentFloor.Should().Be(4);
        lift.AreDoorsOpen.Should().BeTrue();
    }

    [Fact]
    public void List_MovesToRequestedFloor_BelowToInitialFloor()
    {
        var lift = new Lift(initialFloor: 7);
        var system = new LiftSystem(lift);

        system.Request(1);
        system.Tick();

        lift.CurrentFloor.Should().Be(6);
        lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_FinishTheRequest_When_ArriveToDetinationAndOpenTheDoors()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);

        system.Request(2);
        system.Tick();
        system.Tick();

        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeTrue();
        (!system.HasNoPendingRequest()).Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_ShouldNotFinishTheRequest_When_DidntArriveToTheRequestedFloor()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);

        system.Request(3);
        system.Tick();

        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeFalse();
        (!system.HasNoPendingRequest()).Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_ShouldNotFinishTheRequest_When_ArriveToTheRequestedFloorButDidNotOpenTheDoors()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);

        system.Request(2);
        system.Tick();

        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeFalse();
        (!system.HasNoPendingRequest()).Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_ShouldCloseTheDoorsBeforeToStartARequest()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);

        system.Request(2);
        system.Tick();
        system.Tick();
        system.Request(1);
        system.Tick();
        system.Tick();
        system.Tick();

        lift.CurrentFloor.Should().Be(1);
        lift.AreDoorsOpen.Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_LiftSystem_ShouldCloseAndMoveInTwoDifferentMoments()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);
        
        system.Request(2);
        system.Tick();
        system.Tick();
        system.Request(5);
        system.Tick();
        system.Tick();
        
        lift.CurrentFloor.Should().Be(3);
        lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_ShouldDoNothing_When_FinishARequest_And_ThereIsNoMoreRequest()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);

        system.Request(2);
        system.Tick();
        system.Tick();
        system.Tick();

        (!system.HasNoPendingRequest()).Should().BeFalse();
        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_MovesToCalledFloor()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);

        system.Call(3);
        system.Tick();

        lift.CurrentFloor.Should().Be(2);
        lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void LiftSystem_MovesToCalledFloorAndOpenDoors()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);

        system.Call(3);
        system.Tick();
        system.Tick();
        system.Tick();

        lift.CurrentFloor.Should().Be(3);
        lift.AreDoorsOpen.Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_CantHandleARequest_While_HasACall()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);
        var caller = () => system.Request(9);
        
        system.Call(4);
        system.Tick();
        system.Tick();
        
        caller.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void LiftSystem_CantHandlerACall_While_HasARequest()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);
        var caller = () => system.Call(7);
        system.Request(4);
        system.Tick();
        system.Tick();
        
        caller.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void LiftSystem_ShouldDoNothing_When_FinishACall_And_ThereIsNoMoreCalls()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);
        
        system.Call(4);
        system.Tick();
        system.Tick();
        system.Tick();
        system.Tick();
        system.Tick();

        system.HasNoPendingCalls().Should().BeTrue();
        lift.IsInFloor(4).Should().BeTrue();
    }

    [Fact]
    public void LiftSystem_ShouldCloseTheDoorsBeforeToStartACall()
    {
        var lift = new Lift();
        var system = new LiftSystem(lift);
        
        system.Call(2);
        system.Tick();
        system.Tick();
        system.Call(5);
        system.Tick();
        system.Tick();
        
        lift.IsInFloor(3).Should().BeTrue();
        lift.AreDoorsOpen.Should().BeFalse();
    }

    [Fact]
    public void List_MovesToCalledFloor_BelowToInitialFloor()
    {
        var lift = new Lift(initialFloor: 7);
        var system = new LiftSystem(lift);
        
        system.Call(5);
        system.Tick();
        system.Tick();
        system.Tick();

        system.HasNoPendingCalls().Should().BeTrue();
        lift.IsInFloor(5).Should().BeTrue();
        lift.AreDoorsOpen.Should().BeTrue();
    }
}