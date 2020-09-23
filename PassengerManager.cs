using System.Collections.Generic;
using System.Linq;
using System;

namespace ElevatorChallenge
{
    class PassengerManager
    {
        private List<IPassenger> _passengers;
        private PassengerFactory _passengerFactory;

        private int _floors;

        public PassengerManager(PassengerFactory factory, int floors){
            _passengerFactory = factory;
            _passengers = new List<IPassenger>();
            _floors = floors;
        }

        public void AddPassengers(int floor, int destinationFloor, int passengers)
        {
            for (int i = 0; i < passengers; i++)
            {
                _passengers.Add(_passengerFactory.CreateNewPassenger(floor, destinationFloor));
            }
        }

        public void DisplayPassengerStatus(){

            for (int i = 0; i < _floors; i++)
            {
                int passengerCount = _passengers.Count(p => p.CurrentFloor == i);
                if (passengerCount > 0)
                    Console.WriteLine(passengerCount + " passengers waiting on floor " + i);
            }
        }

        public List<IPassenger> GetWaitingPassengers(){
            return _passengers;
        }
    }
}