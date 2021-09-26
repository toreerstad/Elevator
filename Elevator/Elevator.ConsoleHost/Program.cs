using Elevator.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elevator.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            // Application code should start here.
            var controller = host.Services.GetRequiredService<IElevatorController>();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation(@"
Waiting for input command to elevator.
Floor number and Enter to send elevator to designated floor.
D and Enter to get elevator's move direction.
S and Enter for emergency stop.
Q and Enter or Ctrl+C to quit program");
            while (true)
            {
                var command = System.Console.ReadLine();
                if (int.TryParse(command, out int floor))
                {
                    controller.GoToFloor(floor);
                    var estimatedTime = controller.GetEstimatedTimeToFloor(floor);
                    logger.LogInformation($"Estimated time to floor {floor}: {estimatedTime.TotalSeconds} seconds");
                }
                else if (command?.ToLower() == "d")
                {
                    var moveDirection = controller.GetMoveDirection();
                    logger.LogInformation($"Move direction is {moveDirection}");
                }
                else if (command?.ToLower() == "s")
                {
                    logger.LogInformation("Sorry, not implemented");
                    //controller.DoEmergencyStop();
                }
                else if (command?.ToLower() == "q")
                {
                    logger.LogInformation("Quitting");
                    return;
                }
                else
                {
                    logger.LogInformation($"{command} is not a legal instruction");
                }
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLogging(configure => configure.AddConsole());
                    services.AddSingleton<IElevatorController, ElevatorController>();
                    services.AddSingleton<IElevator, Domain.Elevator>();
                    services.AddSingleton<ITaskDelayer, TaskDelayer>();

                    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    services.Configure<ElevatorOptions>(configuration.GetSection(nameof(ElevatorOptions)));
                });
    }
}
