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
        system.HasPendingRequest().Should().BeFalse();
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
        system.HasPendingRequest().Should().BeTrue();
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
        system.HasPendingRequest().Should().BeTrue();
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

        lift.CurrentFloor.Should().Be(1);
        lift.AreDoorsOpen.Should().BeTrue();
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

        system.HasPendingRequest().Should().BeFalse();
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
}