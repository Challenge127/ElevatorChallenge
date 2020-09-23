using System.Collections.Generic;
using System.Linq;

namespace ElevatorChallenge
{

    abstract class PassengerFactory
    {
        public abstract IPassenger CreateNewPassenger(int floor, int destinationFloor);

    }

    class DefaultPassengerFactory : PassengerFactory
    {
        public override IPassenger CreateNewPassenger(int floor, int destinationFloor)
        {
            return new DefaultPassenger(floor, destinationFloor);
        }
    }

    interface IPassenger
    {
        int CurrentFloor { get { return CurrentFloor; } }

        int DestinationFloor { get { return DestinationFloor; } }

    }

    class DefaultPassenger : IPassenger
    {
        private int _currentFloor;
        public int CurrentFloor { get { return _currentFloor; } }


        private int _destinationFloor;

        public int DestinationFloor { get { return _destinationFloor; } }

        public DefaultPassenger(int currentFloor, int destinationFloor)
        {
            _currentFloor = currentFloor;
            _destinationFloor = destinationFloor;
        }
    }
}