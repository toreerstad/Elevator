using System;

namespace Elevator.Domain
{
    public interface IElevatorController
    {
        public void GoToFloor(int floorNumber);
        public MoveDirection GetMoveDirection();
        public TimeSpan GetEstimatedTimeToFloor(int floorNumber);
        public void DoEmergencyStop();

    }
}