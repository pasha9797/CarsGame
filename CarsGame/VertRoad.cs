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
        public override double[] GetStartPosition()
        {
            return new double[2] { carsStart[C.X], carsStart[C.Y] };
        }
        public override double[] GetEndPosition()
        {
            return new double[2] { carsEnd[C.X], carsEnd[C.Y] };
        }
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
            this.size[C.X] = C.VertRoadSize.Width;
            this.size[C.Y] = C.VertRoadSize.Height;
            carsStart = new double[2];
            carsEnd = new double[2];
            carsStart[C.X] = position[C.X] + C.HorRoadSize.Height / 2;
            carsStart[C.Y] = position[C.Y] - C.OutOfView;
            carsEnd[C.X] = position[C.X] + C.HorRoadSize.Height / 2;
            carsEnd[C.Y] = position[C.Y] + C.VertRoadSize.Height + C.OutOfView;
        }
    }
}
