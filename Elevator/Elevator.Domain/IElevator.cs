using System;

namespace Elevator.Domain
{
    public interface IElevator
    {
        void GoToFloor(int floor);
        MoveDirection GetMoveDirection();
        TimeSpan GetEstimatedTimeToFloor(int floor);
    }
}