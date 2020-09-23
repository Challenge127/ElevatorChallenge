using System;
namespace ElevatorChallenge
{

    enum ElevatorStatus
    {
        up,
        down,
        idle,
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter number of floors");
            int floors = InputManager.ReadInt(2, 50);
            Console.WriteLine("Enter number of elevators");
            int elevators = InputManager.ReadInt(1, 50);
            ElevatorManager elevatorManager = new ElevatorManager(floors, elevators, new PassengerElevatorFactory());
            PassengerManager passengerManager = new PassengerManager(new DefaultPassengerFactory(), floors);
            int menuSelect = -1;

            elevatorManager.DisplayElevatorStatus();

            while (menuSelect != 0)
            {
                Console.WriteLine("0) Quit Application \n1) Call elevator to a floor \n2) Add passengers to a floor \n3) Disembark passengers from elevator \n4) Advance Simulation");
                menuSelect = InputManager.ReadInt();

                switch (menuSelect)
                {
                    case 0:
                        return;
                    case 1:
                        Console.WriteLine("Select floor to call elevator to");
                        elevatorManager.CallElevator(InputManager.ReadInt(0,floors - 1));
                        break;
                    case 2:
                        Console.WriteLine("Select floor");
                        int floor = InputManager.ReadInt(0, floors -1);
                        Console.WriteLine("Select number of passengers");
                        int passengers = InputManager.ReadInt(0);
                        passengerManager.AddPassengers(floor, 1, passengers);
                        break;
                    case 3:
                        Console.WriteLine("Select elevator");
                        int elevatorIndex = InputManager.ReadInt(0, elevators - 1);
                        elevatorManager.DisembarkPassengers(elevatorIndex);
                        break;
                    case 4:
                        elevatorManager.RunSimulationStep();
                        elevatorManager.EmbarkPassengers(passengerManager.GetWaitingPassengers());
                        break;
                    default:
                        break;
                }
                Console.Clear();
                elevatorManager.DisplayElevatorStatus();
                passengerManager.DisplayPassengerStatus();
            }
        }
    }
}
