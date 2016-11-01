using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarsGame
{
    public class Bus:Vehicle
    {
        protected override void PictureBroken()
        {
            picture = C.IBusBrokenPic;
        }
        protected override int GetTurn()
        {
            return (currentRoadorCrossway as Crossway).Turning[C.BUS];
        }

        public Bus(double x, double y) : base(x, y)
        { 
            picture = C.IBusPic;
            this.size[C.X] = C.BusSize.Width;
            this.size[C.Y] = C.BusSize.Height;
        }
    }
}
