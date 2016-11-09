using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarsGame
{
    public class Truck:Vehicle
    {
        protected override void PictureBroken()
        {
            picture = C.ITruckBrokenPic;
        }
        protected override Turn GetTurn()
        {
            return (curPlacement as Crossway).Turning[(int)VehType.TRUCK];
        }

        public Truck(Road road) : base(road)
        {
            picture = C.ITruckPic;
        }
    }
}
