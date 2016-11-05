using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarsGame
{
    public class VertRoad:Road
    {
        public override int GetDirectionToMove(double[] pos)
        {
            if (pos[C.X] == carsStart[C.X] && pos[C.Y] == carsStart[C.Y])
            {
                return C.DOWN;
            }
            else if (pos[C.X] == carsEnd[C.X] && pos[C.Y] == carsEnd[C.Y])
            {
                return C.UP;
            }
            else return -1;
        }

        public VertRoad(double x,double y):base(x,y)
        {
            picture = C.IVertRoadPic;
            this.size[C.X] = C.IVertRoadPic.Size.Width;
            this.size[C.Y] = C.IVertRoadPic.Size.Height;
            carsStart = new double[2];
            carsEnd = new double[2];
            carsStart[C.X] = position[C.X] + C.smallDelta;
            carsStart[C.Y] = position[C.Y] - C.VehicleSize.Width;
            carsEnd[C.X] = position[C.X] + C.bigDelta;
            carsEnd[C.Y] = position[C.Y] + C.IVertRoadPic.Size.Height + C.VehicleSize.Width;
        }
    }
}
