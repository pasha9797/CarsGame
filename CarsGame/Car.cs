using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarsGame
{
    public class Car:Vehicle
    {
        protected override void PictureBroken()//замена текстурки на сломанную
        {
            picture = C.ICarBrokenPic;
        }
        protected override int GetTurn()//в какую сторону повернуть на текущем перекрестке
        {
                return (curPlacement as Crossway).Turning[C.CAR];
        }

        public Car(Road road):base(road)
        {
            picture = C.ICarPic;
         
        }
    }
}
