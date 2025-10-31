using Domain.Extensions;

namespace Domain;

public enum Direction
{
    Up = 'U',
    Down = 'D'
}

public class LiftSystem(Lift lift)
{
    private int _request;
    private int _minFloor = 1;
    private int _maxFloor = 10;

    public bool HasNoPendingCalls() => _request == 0;
    public bool HasNoPendingRequest() => _request == 0;

    public void Request(int floor)
    {
        InvalidOperationException.ThrowIfFalse(HasNoPendingCalls());
        ArgumentOutOfRangeException.ThrowIfGreaterThan(floor, _maxFloor);
        ArgumentOutOfRangeException.ThrowIfLessThan(floor, _minFloor);

        _request = floor;
    }

    public void Call(int floor, Direction direction = 0)
    {
        if (direction == Direction.Up)
            _minFloor = floor;
        else
            _maxFloor = floor;
        Request(floor);
    }

    public void Tick()
    {
        if (HasNoPendingRequest()) return;

        TickRequest();
    }

    private void TickRequest()
    {
        if (IsOnTheRequestedFloor())
            FinishRequest();
        else if (lift.AreDoorsOpen)
            lift.CloseDoors();
        else
            MoveLift();
    }

    private void FinishRequest()
    {
        lift.OpenDoors();
        ClearRequest();
    }

    private void MoveLift()
    {
        if (ShouldMoveDown()) lift.MoveDown();
        else lift.MoveUp();
    }

    private bool ShouldMoveDown() => lift.CurrentFloor > _request;
    private bool IsOnTheRequestedFloor() => lift.IsInFloor(_request);
    private void ClearRequest() => _request = 0;
}