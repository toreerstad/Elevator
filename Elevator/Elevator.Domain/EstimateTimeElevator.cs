using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Elevator.Domain
{
    public class EstimateTimeElevator : Elevator
    {
        private readonly ITaskDelayer _taskDelayer;
        private readonly Dictionary<int, int> _floorElapsedTime = new Dictionary<int, int>();

        public EstimateTimeElevator(
            ILogger<Elevator> logger, 
            IOptions<ElevatorOptions> options, 
            ITaskDelayer taskDelayer,
            Destinations destinations,
            ElevatorPosition elevatorPosition) 
            : base(logger, options, taskDelayer)
        {
            _taskDelayer = taskDelayer;
            Destinations = destinations;
            ElevatorPosition = elevatorPosition;
        }


        public override TimeSpan GetEstimatedTimeToFloor(int floor)
        {
            var timeInMilliseconds = _floorElapsedTime.ContainsKey(floor) ? _floorElapsedTime[floor] : 0;
            return TimeSpan.FromMilliseconds(timeInMilliseconds);
        }

        protected override void StopAtFloor()
        {
            var elapsedTimeInMilliseconds = ((TaskTimeAccumulator)_taskDelayer).TotalElapsedMilliseconds;
            _floorElapsedTime.Add(ElevatorPosition.Floor, elapsedTimeInMilliseconds);
            base.StopAtFloor();
        }
    }
}
