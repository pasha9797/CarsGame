using CarsGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus
{
    [PluginForLoad(true)]
    public class Bus : Vehicle
    {
        public Bus(Road road, IGameContext gc) : base(road, gc)
        {
            PicPath = "BusPic.png";
            BrokenPicPath = "BusPicBroken.png";
            Turns = new Turn[]
            {
                Turn.STRAIGHT,
                Turn.RIGHT,
                Turn.LEFT,
                Turn.STRAIGHT
            };
        }
    }
}
