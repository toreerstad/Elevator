using System;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Elevator.Domain
{
    public class Elevator : IElevator
    {
        private readonly  ILogger<IElevator> _logger;
        private readonly ITaskDelayer _taskDelayer;
        private readonly ElevatorOptions _options;
        protected Destinations Destinations = new Destinations();
        protected ElevatorPosition ElevatorPosition = new ElevatorPosition();

        public Elevator(ILogger<Elevator> logger, IOptions<ElevatorOptions> options, ITaskDelayer taskDelayer)
        {
            _logger = logger;
            _taskDelayer = taskDelayer;
            _options = options.Value;
        }

        public virtual void GoToFloor(int floor)
        {
            if (floor < 1 || floor > _options.NumberOfFloors)
            {
                _logger.LogWarning($"Illegal floor number {floor}. Choose a value between 1 and {_options.NumberOfFloors}");
                return;
            }

            if (!Destinations.Contains(floor))
            {
                Destinations.Add(floor);
            }

            if (ElevatorPosition.MoveDirection == MoveDirection.None)
            {
                if (ElevatorPosition.Floor == floor)
                {
                    _logger.LogInformation($"Elevator already at floor {ElevatorPosition.Floor}");
                    Destinations.Remove(floor);
                    return;
                }

                ElevatorPosition.MoveDirection = ElevatorPosition.Floor < floor
                    ? MoveDirection.Up
                    : MoveDirection.Down;

                StartMove();
            }
        }

        public MoveDirection GetMoveDirection()
        {
            return ElevatorPosition.MoveDirection;
        }

        public virtual TimeSpan GetEstimatedTimeToFloor(int floor)
        {
            // Create a elevator for estimating time with copies of state from this elevator
            var destinations = new Destinations();
            destinations.AddRange(Destinations);

            if (!destinations.Contains(floor))
            {
                destinations.Add(floor);
            }

            var elevatorPosition = new ElevatorPosition
            {
                Floor = ElevatorPosition.Floor,
                MoveDirection = ElevatorPosition.MoveDirection
            };

            var estimateTimeElevator = new EstimateTimeElevator(
                new NullLogger<Elevator>(),
                Options.Create(_options),
                new TaskTimeAccumulator(),
                destinations,
                elevatorPosition);

            if (elevatorPosition.MoveDirection == MoveDirection.None)
            {
                estimateTimeElevator.GoToFloor(floor);
            }
            else
            {
                estimateTimeElevator.ContinueMove();
            }

            // Hack! Sleep to make sure all tasks are finished before gathering result
            Thread.Sleep(50);

            return estimateTimeElevator.GetEstimatedTimeToFloor(floor);
        }

        private void StartMove()
        {
            _logger.LogInformation($"Start moving {ElevatorPosition.MoveDirection} from floor {ElevatorPosition.Floor}");

            _taskDelayer.Delay(_options.MoveTimeBetweenFloorsInMilliseconds).ContinueWith(_ =>
            {
                SetNextFloor();
            });
        }

        private void ContinueMove()
        {
            _logger.LogInformation($"Passing floor {ElevatorPosition.Floor}");

            _taskDelayer.Delay(_options.MoveTimeBetweenFloorsInMilliseconds).ContinueWith(_ =>
            {
                SetNextFloor();
            });
        }

        private void SetNextFloor()
        {
            ElevatorPosition.UpdateToNextFloor();
            if (Destinations.Contains(ElevatorPosition.Floor))
            {
                StopAtFloor();
            }
            else
            {
                ContinueMove();
            }
        }

        protected virtual void StopAtFloor()
        {
            _logger.LogInformation($"Stopped at floor {ElevatorPosition.Floor}");
            Destinations.Remove(ElevatorPosition.Floor);

            OpenAndCloseDoors(() =>
            {
                if (Destinations.Any())
                {
                    if (!Destinations.HasDestinationsInDirection(ElevatorPosition.MoveDirection, ElevatorPosition.Floor))
                    {
                        ElevatorPosition.SwitchDirection();
                    }

                    StartMove();
                }
                else
                {
                    _logger.LogInformation("Final destination reached. Waiting for new commands.");
                    ElevatorPosition.MoveDirection = MoveDirection.None;
                }
            });
        }

        private void OpenAndCloseDoors(Action callback)
        {
            _taskDelayer.Delay(_options.OpenCloseDoorsTimeInMillisecond).ContinueWith(t =>
            {
                _logger.LogInformation("Doors opened");
                _taskDelayer.Delay(_options.OpenCloseDoorsTimeInMillisecond).ContinueWith(t =>
                {
                    _logger.LogInformation("Doors closed");
                    callback();
                });
            });
        }
    }
}
