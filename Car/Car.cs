using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsGameLib;

namespace Car
{
    [PluginForLoad(true)]
    public class Car:Vehicle
    {
        public Car(Road road, IGameContext gc):base(road,gc)
        {
            PicPath = "CarPic.png";
            BrokenPicPath = "CarPicBroken.png";
            Turns = new Turn[]
            {
                Turn.LEFT,
                Turn.STRAIGHT,
                Turn.RIGHT,
                Turn.LEFT
            };
        }
    }
}
