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
                return (currentRoadorCrossway as Crossway).Turning[C.CAR];
        }

        public Car(double x,double y):base(x,y)
        {
            picture = C.ICarPic;
            this.size[C.X] = C.CarSize.Width;
            this.size[C.Y] = C.CarSize.Height;
        }
    }
}
