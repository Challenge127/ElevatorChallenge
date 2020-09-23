using System.Collections.Generic;
using System.Linq;
using System;
namespace ElevatorChallenge
{
    class ElevatorManager
    {
        private int _floors;

        private List<IElevator> _elevators;

        private List<int> _unfullfilledFloorRequests = new List<int>();

        public ElevatorManager(int floors, int totalElevators, ElevatorFactory factory)
        {
            _floors = floors;
            _elevators = new List<IElevator>();
            for (int elevatorIndex = 0; elevatorIndex < totalElevators; elevatorIndex++)
            {
                _elevators.Add(factory.CreateNewElevator());
            }
        }

        public void DisplayElevatorStatus()
        {
            int index = 0;
            foreach (IElevator elevator in _elevators)
            {
                Console.WriteLine("Elevator: " + (index).ToString()
                + " Floor: " + elevator.Floor
                + " Status: " + elevator.Direction
                + " Passengers: " + elevator.GetNumberOfPassengers()
                + " Doors: " + (elevator.DoorsOpen ? "open" : "closed"));

                index++;
            }
        }

        public void EmbarkPassengers(List<IPassenger> passengers)
        {

            List<IPassenger> embarkedPassengers = new List<IPassenger>();

            foreach (IPassenger passenger in passengers)
            {
                IElevator elevator = _elevators.FirstOrDefault(e => e.Floor == passenger.CurrentFloor && e.GetNumberOfPassengers() < e.MaxPassengers);
                if (elevator != null)
                {
                    bool embarked = elevator.AddPassenger(passenger);
                    if (embarked)
                        embarkedPassengers.Add(passenger);

                }
            }

            foreach (IPassenger passenger in embarkedPassengers)
            {
                passengers.Remove(passenger);
            }
        }

        public bool DisembarkPassengers(int elevatorIndex)
        {
            IElevator elevator = _elevators[elevatorIndex];

            if (elevator.DoorsOpen == true)
            {
                List<IPassenger> disembarkedPassengers = new List<IPassenger>();
                foreach (IPassenger passenger in elevator.Passengers)
                {
                    disembarkedPassengers.Add(passenger);
                }

                foreach (IPassenger passenger in disembarkedPassengers)
                {
                    elevator.Passengers.Remove(passenger);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CallElevator(int floor){
            
            bool elevatorAssigned = false;
            
            if(AssignBestElevator(floor))
                elevatorAssigned = true;
            else
                _unfullfilledFloorRequests.Add(floor);
            
            return elevatorAssigned;
        }
        private bool AssignBestElevator(int floor)
        {

            bool elevatorAssigned = false;
            //Identify available elevators 
            List<IElevator> availableElevators = GetAvailableElevators(floor);

            //Now pick the best option from all available elevators
            IElevator closestElevator = GetNearestElevator(floor, availableElevators);
            if (closestElevator != null)
            {
                closestElevator.SetDestinationFloor(floor);
                elevatorAssigned = true;
            }

            return elevatorAssigned;
        }

        private IElevator GetNearestElevator(int floor, List<IElevator> availableElevators)
        {
            IElevator closestElevator = availableElevators.OrderBy(e => Math.Abs(e.Floor - floor)).FirstOrDefault();
            return closestElevator;
        }


        private List<IElevator> GetAvailableElevators(int floor)
        {
            List<IElevator> availableElevators = _elevators.Where(e => e.Passengers.Count < e.MaxPassengers).ToList();

            availableElevators = availableElevators.Where(e =>
            (e.Direction == ElevatorStatus.idle)
            || (floor >= e.Floor && e.Direction == ElevatorStatus.up)
            || (floor <= e.Floor && e.Direction == ElevatorStatus.down)
            ).ToList();

            return availableElevators;
        }

        private bool ElevatorOnFloor(int floor)
        {
            foreach (IElevator elevator in _elevators)
            {
                if (elevator.Floor == floor)
                    return true;
            }
            return false;
        }

        public void RunSimulationStep()
        {
            foreach (IElevator elevator in _elevators)
            {
                elevator.Move();
            }
            //Since elevators have moved, there may now be idle elevators. Check to see if we can assign elevators to any unfulfilled requests
             List<int> fulfilledFloorRequests = new List<int>();
             
             foreach (int unfulfilledFloorRequest in _unfullfilledFloorRequests)
            {
               if (AssignBestElevator(unfulfilledFloorRequest))
               {
                   fulfilledFloorRequests.Add(unfulfilledFloorRequest);
               }
            }

            foreach (int fulfilledFloorRequest in fulfilledFloorRequests)
            {
                _unfullfilledFloorRequests.Remove(fulfilledFloorRequest);
            }
        }

    }
}

