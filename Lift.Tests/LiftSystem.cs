namespace Lift.Tests;

public class LiftSystem(Lift lift)
{
    private int _request;
    private int _call;

    public void Request(int floor)
    {
        _request = floor;
    }

    public void Call(int floor)
    {
        _call = floor;
    }

    public void Tick()
    {
        if (_call != 0)
        {
            if (lift.IsInFloor(_call))
            {
                lift.OpenDoors();
                return;
            }
            else
            {
                lift.MoveUp();
            }
        }
        if (!HasPendingRequest()) return;
        if (IsOnTheRequestedFloor())
        {
            lift.OpenDoors();
            ClearRequest();
        }
        else
        {
            lift.CloseDoors();
            MoveLift();
        }
    }

    public bool HasPendingRequest() => _request != 0;

    private void MoveLift()
    {
        if (ShouldMoveDown()) lift.MoveDown();
        else lift.MoveUp();
    }

    private bool ShouldMoveDown() => lift.CurrentFloor > _request;
    private bool IsOnTheRequestedFloor() => lift.IsInFloor(_request);
    private void ClearRequest() => _request = 0;
}