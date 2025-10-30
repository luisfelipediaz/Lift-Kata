namespace Lift.Tests;

public class LiftSystem(Lift lift)
{
    private int _request;

    public bool HasNoPendingCalls() => _request == 0;
    public bool HasNoPendingRequest() => _request == 0;

    public void Request(int floor)
    {
        if (HasNoPendingCalls())
            _request = floor;
        else throw new InvalidOperationException();
    }

    public void Call(int floor)
    {
        Request(floor);
    }

    public void Tick()
    {
        if (HasNoPendingRequest()) return;
        if (IsOnTheRequestedFloor())
        {
            lift.OpenDoors();
            ClearRequest();
        }
        else if (lift.AreDoorsOpen)
        {
            lift.CloseDoors();
        }
        else
        {
            MoveLift();
        }
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