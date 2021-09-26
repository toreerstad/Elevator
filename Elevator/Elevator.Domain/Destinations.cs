using System.Collections.Generic;
using System.Linq;

namespace Elevator.Domain
{
    public class Destinations : List<int>
    {
        public bool HasDestinationsInDirection(MoveDirection moveDirection, int currentFloor)
        {
            return moveDirection == MoveDirection.Up
                ? this.Any(destination => destination > currentFloor)
                : this.Any(destination => destination < currentFloor);
        }
    }
}