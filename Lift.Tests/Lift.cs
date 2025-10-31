namespace Lift.Tests;

public class Lift(int initialFloor = 1)
{
    private const int MaxFloor = 10;
    private const int MinFloor = 1;
    public bool AreDoorsOpen { get; private set; }
    public int CurrentFloor { get; private set; } = initialFloor;
    public void CloseDoors() => AreDoorsOpen = false;
    public void OpenDoors() => AreDoorsOpen = true;
    public void MoveUp() => MoveTo(CurrentFloor + 1);
    public void MoveDown() => MoveTo(CurrentFloor - 1);
    public bool IsInFloor(int floor) => CurrentFloor == floor;

    private void MoveTo(int floor)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(floor, MinFloor);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(floor, MaxFloor);
        if (AreDoorsOpen)
            throw new InvalidOperationException();
        CurrentFloor = floor;
    }
}