namespace Lift.Tests;

public class LiftSystem(Lift lift)
{
    private int _request;
    private int _call;

    public bool HasNoPendingCalls() => _call == 0;
    public bool HasNoPendingRequest() => _request == 0;

    public void Request(int floor)
    {
        if (HasNoPendingCalls())
            _request = floor;
        else throw new InvalidOperationException();
    }

    public void Call(int floor)
    {
        if (HasNoPendingRequest())
            _call = floor;
        else throw new InvalidOperationException();
    }

    public void Tick()
    {
        ProcessCalls();
        ProcessRequests();
    }

    private void ProcessCalls()
    {
        if (HasNoPendingCalls()) return;

        if (IsOnTheCalledFloor())
        {
            lift.OpenDoors();
            ClearCall();
        }
        else if (lift.AreDoorsOpen)
        {
            lift.CloseDoors();
        }
        else
        {
            lift.MoveUp();
        }
    }

    private void ProcessRequests()
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
    private bool IsOnTheCalledFloor() => lift.IsInFloor(_call);
    private bool IsOnTheRequestedFloor() => lift.IsInFloor(_request);
    private void ClearRequest() => _request = 0;
    private void ClearCall() => _call = 0;
}