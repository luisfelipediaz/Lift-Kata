namespace Lift.Tests;

public class LiftSystem(Lift lift)
{
    private int _request;

    public void Request(int floor)
    {
        _request = floor;
    }

    public void Tick()
    {
        if (IsOnTheRequestedFloor())
        {
            lift.OpenDoors();
            ClearRequest();
            return;
        }

        lift.CloseDoors();
        Move();
    }

    public bool HasPendingRequest() => _request != 0;

    private void Move()
    {
        if (lift.CurrentFloor > _request)
            lift.MoveDown();
        else
            lift.MoveUp();
    }
    
    private bool IsOnTheRequestedFloor() => lift.CurrentFloor == _request;
    private void ClearRequest() => _request = 0;
}