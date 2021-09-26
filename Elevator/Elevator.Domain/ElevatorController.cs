using System;
using Microsoft.Extensions.Logging;

namespace Elevator.Domain
{
    public class ElevatorController : IElevatorController
    {
        private readonly ILogger<ElevatorController> _logger;
        private readonly IElevator _elevator;

        public ElevatorController(ILogger<ElevatorController> logger, IElevator elevator)
        {
            _logger = logger;
            _elevator = elevator;
        }

        public void GoToFloor(int floorNumber)
        {
            _elevator.GoToFloor(floorNumber);
        }

        public MoveDirection GetMoveDirection()
        {
            return _elevator.GetMoveDirection();
        }

        public TimeSpan GetEstimatedTimeToFloor(int floorNumber)
        {
            return _elevator.GetEstimatedTimeToFloor(floorNumber);
        }

        public void DoEmergencyStop()
        {
            throw new NotImplementedException();
        }
    }
}
