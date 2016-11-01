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
        protected override int GetTurn()
        {
            return (currentRoadorCrossway as Crossway).Turning[C.CAR];
        }

        public Truck(double x, double y) : base(x, y)
        {
            picture = C.ITruckPic;
            this.size[C.X] = C.TruckSize.Width;
            this.size[C.Y] = C.TruckSize.Height;
        }
    }
}
