using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarsGame
{
    public class HorRoad:Road
    {
        public override double[] GetStartPosition()//поставить машину на начало дороги
        {
                return new double[2] { carsStart[C.X], carsStart[C.Y] };
        }
        public override double[] GetEndPosition()//поставить машину на конец дороги
        {
            return new double[2] { carsEnd[C.X], carsEnd[C.Y] };
        }
        public override int GetDirectionToMove(double[] pos)//куда ехать в зависимости от положения
        {
            if (pos[C.X] == carsStart[C.X] && pos[C.Y] == carsStart[C.Y])
            {
                return C.RIGHT;
            }
            else if (pos[C.X] == carsEnd[C.X] && pos[C.Y] == carsEnd[C.Y])
            {
                return C.LEFT;
            }
            else return -1;
        }

        public HorRoad(double x, double y):base(x,y)
        {
            picture = C.IHorRoadPic;
            this.size[C.X] = C.HorRoadSize.Width;
            this.size[C.Y] = C.HorRoadSize.Height;
            carsStart = new double[2];
            carsEnd = new double[2];
            carsStart[C.X] = position[C.X] - C.OutOfView;
            carsStart[C.Y] = position[C.Y] + C.HorRoadSize.Height / 2;
            carsEnd[C.X] = position[C.X] + C.HorRoadSize.Width + C.OutOfView;
            carsEnd[C.Y] = position[C.Y] + C.HorRoadSize.Height / 2;
        }
    }
}
