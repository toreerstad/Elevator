using System.Threading.Tasks;

namespace Elevator.Domain
{
    public interface ITaskDelayer
    {
        Task Delay(int milliseconds);
    }
}
