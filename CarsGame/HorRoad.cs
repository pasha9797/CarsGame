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
            this.size[C.X] = C.IHorRoadPic.Size.Width;
            this.size[C.Y] = C.IHorRoadPic.Size.Height;
            carsStart = new double[2];
            carsEnd = new double[2];
            carsStart[C.X] = position[C.X] - C.VehicleSize.Width;
            carsStart[C.Y] = position[C.Y] + C.bigDelta;
            carsEnd[C.X] = position[C.X] + C.IHorRoadPic.Size.Width + C.VehicleSize.Width;
            carsEnd[C.Y] = position[C.Y] + C.smallDelta;
        }
    }
}
