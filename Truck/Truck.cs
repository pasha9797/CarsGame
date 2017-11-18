using CarsGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truck
{
    [PluginForLoad(true)]
    public class Truck : Vehicle
    {
        public Truck(Road road, IGameContext gc) : base(road, gc)
        {
            PicPath = "TruckPic.png";
            BrokenPicPath = "TruckPicBroken.png";
            Turns = new Turn[]
            {
                Turn.RIGHT,
                Turn.LEFT,
                Turn.STRAIGHT,
                Turn.RIGHT
            };
        }
    }
}
