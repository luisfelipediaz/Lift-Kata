namespace Domain;

public class Lift(int initialFloor = 1)
{
    public bool AreDoorsOpen { get; private set; }
    public int CurrentFloor { get; private set; } = initialFloor;
    public void CloseDoors() => AreDoorsOpen = false;
    public void OpenDoors() => AreDoorsOpen = true;
    public void MoveUp() => MoveTo(CurrentFloor + 1);
    public void MoveDown() => MoveTo(CurrentFloor - 1);
    public bool IsInFloor(int floor) => CurrentFloor == floor;

    private void MoveTo(int floor)
    {
        if (AreDoorsOpen)
            throw new InvalidOperationException();
        CurrentFloor = floor;
    }
}