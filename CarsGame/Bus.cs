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
        protected override Turn GetTurn()
        {
            return (curPlacement as Crossway).Turning[(int)VehType.BUS];
        }

        public Bus(Road road) : base(road)
        { 
            picture = C.IBusPic;
        }
    }
}
