using System.Threading.Tasks;

namespace Elevator.Domain
{
    public class TaskDelayer : ITaskDelayer
    {
        public Task Delay(int milliseconds)
        {
            return Task.Delay(milliseconds);
        }
    }
}
