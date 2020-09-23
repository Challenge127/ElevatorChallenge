using System.Collections.Generic;
using System.Linq;
using System;
namespace ElevatorChallenge
{

    interface IElevator
    {
        int Floor { get { return Floor; } }
        int DestinationFloor { get { return DestinationFloor; } }
        ElevatorStatus Direction { get { return Direction; } }
        void SetDestinationFloor(int floor);
        void Move();
        int GetNumberOfPassengers();
        int MaxPassengers { get { return MaxPassengers; } }
        List<IPassenger> Passengers { get { return Passengers; } }
        bool AddPassenger(IPassenger passenger);

        bool DoorsOpen { get { return DoorsOpen; } }

    }

    abstract class ElevatorFactory
    {
        public abstract IElevator CreateNewElevator();
    }

    class PassengerElevatorFactory : ElevatorFactory
    {
        public override IElevator CreateNewElevator()
        {
            return new PassengerElevator(0, 9);
        }
    }

    class PassengerElevator : IElevator
    {

        private int _maxPassengers;
        public int MaxPassengers
        {
            get { return _maxPassengers; }
        }
        private List<IPassenger> _passengers;
        public List<IPassenger> Passengers { get { return _passengers; } }

        private int _floor;
        public int Floor
        {
            get { return _floor; }
        }

        private List<int> _floorRequests = new List<int>();

        private int _destinationFloor;
        public int DestinationFloor
        {
            get { return _destinationFloor; }
        }
        private ElevatorStatus _direction;
        public ElevatorStatus Direction
        {
            get { return _direction; }
        }

        private bool _doorsOpen = true;

        public bool DoorsOpen { get { return _doorsOpen; } }

        public PassengerElevator(int floor, int maxPassengers)
        {
            _floor = floor;
            _passengers = new List<IPassenger>();
            _direction = ElevatorStatus.idle;
            _maxPassengers = maxPassengers;
        }

        public void SetDestinationFloor(int floor)
        {
            _floorRequests.Add(floor);
        }

        public void CalculateNextDestination()
        {

            int nextFloor = _floor;

            if (_floorRequests.Count > 0)
            {
                switch (_direction)
                {
                    //Ensure that requests for floors in the same direction the elevator is moving take priority
                    case ElevatorStatus.up:
                        nextFloor = _floorRequests.OrderBy(o => o).First();
                        break;
                    case ElevatorStatus.down:
                        nextFloor = _floorRequests.OrderBy(o => o).First();
                        break;
                    case ElevatorStatus.idle:
                        int nearestRequestedFloor = -1;
                        foreach (int request in _floorRequests)
                        {
                            if (request < nearestRequestedFloor || nearestRequestedFloor == -1)
                            {
                                nearestRequestedFloor = request;
                            }
                        }
                        nextFloor = nearestRequestedFloor;
                        break;
                }
            }
            _destinationFloor = nextFloor;
        }

        public void Move()
        {
            CalculateNextDestination();

            int difference = _floor - _destinationFloor;

            if (difference < 0)
            {
                MoveUp();
            }
            else if (difference > 0)
            {
                MoveDown();
            }

            CheckReachedDestination();
            CheckIsIdle();
        }

        private void CheckReachedDestination()
        {
            int difference = _floor - _destinationFloor;
            if (difference == 0)
            {
                Stop();
                for (int i = 0; i < _floorRequests.Count; i++)
                {
                    if (_floorRequests[i] == _floor)
                    {
                        _floorRequests.RemoveAt(i);
                    }
                }
            }
        }

        private void CheckIsIdle(){
            if (_floorRequests.Count == 0)
            {
                _direction = ElevatorStatus.idle;
            }
        }

        private void MoveUp()
        {
            _doorsOpen = false;
            _direction = ElevatorStatus.up;
            _floor++;
        }

        private void MoveDown()
        {
            _doorsOpen = false;
            _direction = ElevatorStatus.down;
            _floor--;
        }

        private void Stop()
        {
            _doorsOpen = true;
        }

        public int GetNumberOfPassengers()
        {
            return _passengers.Count;
        }

        public bool AddPassenger(IPassenger passenger)
        {

            bool canBoard = false;
            if (_passengers.Count + 1 <= _maxPassengers && _doorsOpen == true)
            {
                _passengers.Add(passenger);
                canBoard = true;
            }
            return canBoard;
        }
    }
}

