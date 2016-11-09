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
        public override Direction GetDirectionToMove(PointF pos)
        {
            if (pos.X == carsStart.X && pos.Y == carsStart.Y)
            {
                return Direction.DOWN;
            }
            else
            {
                return Direction.UP;
            }
        }

        public VertRoad(float x,float y):base(x,y)
        {
            picture = C.IVertRoadPic;
            this.size.Width = C.IVertRoadPic.Size.Width;
            this.size.Height = C.IVertRoadPic.Size.Height;
            carsStart = new PointF(position.X + C.smallDelta, position.Y - C.VehicleSize.Width);
            carsEnd = new PointF(position.X + C.bigDelta, position.Y + C.IVertRoadPic.Size.Height + C.VehicleSize.Width);
        }
    }
}
