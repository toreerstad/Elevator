using Elevator.Domain;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace Elevator.Tests
{
    public class ElevatorControllerTests
    {
        private readonly ElevatorController _elevatorController;
        private readonly ElevatorOptions _options;

        public ElevatorControllerTests()
        {
            _options = new ElevatorOptions
            {
                NumberOfFloors = 6,
                MoveTimeBetweenFloorsInMilliseconds = 2000,
                OpenCloseDoorsTimeInMillisecond = 1000,
                StopTimeAtFloorInMilliseconds = 1000
            };
            var elevator = new Domain.Elevator(A.Fake<ILogger<Domain.Elevator>>(), Options.Create(_options), new TaskDelayer());
            _elevatorController = new ElevatorController(A.Fake<ILogger<ElevatorController>>(), elevator);
        }

        [Fact]
        public void GoToFloor_StartsElevator_When_Stopped()
        {
            _elevatorController.GoToFloor(2);
            _elevatorController.GetMoveDirection().Should().Be(MoveDirection.Up);
        }

        [Fact]
        public void GetEstimatedTimeToFloor_Should_Calculate_Time_Based_On_Number_Of_Floors_To_Destination()
        {
            var expectedTimeInMilliseconds = 4 * _options.MoveTimeBetweenFloorsInMilliseconds;
            _elevatorController.GetEstimatedTimeToFloor(5).TotalMilliseconds.Should().Be(expectedTimeInMilliseconds);
        }
    }
}
