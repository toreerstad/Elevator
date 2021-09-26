namespace Elevator.Domain
{
    public class ElevatorOptions
    {
        public int NumberOfFloors { get; set; }
        public int MoveTimeBetweenFloorsInMilliseconds { get; set; }
        public int OpenCloseDoorsTimeInMillisecond { get; set; }
        public int StopTimeAtFloorInMilliseconds { get; set; }
    }
}
