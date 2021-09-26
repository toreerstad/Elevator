using System;

namespace Elevator.Domain
{
    public class ElevatorPosition
    {
        public MoveDirection MoveDirection { get; set; }
        public int Floor { get; set; } = 1;

        public void SwitchDirection()
        {
            if (MoveDirection == MoveDirection.None)
            {
                throw new InvalidOperationException("Cannot switch direction when no direction");
            }

            MoveDirection = MoveDirection == MoveDirection.Up
                ? MoveDirection.Down
                : MoveDirection.Up;
        }

        public void UpdateToNextFloor()
        {
            if (MoveDirection == MoveDirection.None)
            {
                throw new InvalidOperationException("Cannot update floor when no direction");
            }

            Floor = MoveDirection == MoveDirection.Up
                ? Floor + 1
                : Floor - 1;
        }
    }
}