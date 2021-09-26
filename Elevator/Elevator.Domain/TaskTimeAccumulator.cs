using System.Threading.Tasks;

namespace Elevator.Domain
{
    public class TaskTimeAccumulator : ITaskDelayer
    {
        public int TotalElapsedMilliseconds { get; private set; }
        public Task Delay(int milliseconds)
        {
            TotalElapsedMilliseconds += milliseconds;
            return Task.CompletedTask;
        }
    }
}
