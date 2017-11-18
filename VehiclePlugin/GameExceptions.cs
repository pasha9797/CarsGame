using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsGameLib
{
    public class GameException : ApplicationException
    {
        public GameException(string message) : base(message) { }
    }
    public class InfrastructureException : GameException
    {
        public InfrastructureException(string message) : base(message) { }
    }
    public class RoadException : InfrastructureException
    {
        public RoadException(string message) : base(message) { }
    }
    public class CrosswayException : InfrastructureException
    {
        public CrosswayException(string message) : base(message) { }
    }
    public class VehicleException : GameException
    {
        public VehicleException(string message) : base(message) { }
    }
}
