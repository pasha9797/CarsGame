using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarsGameLib
{
    public class HorRoad : Road
    {
        public override Direction GetDirectionToMove(PointF pos)//куда ехать в зависимости от положения
        {
            if (pos.X == carsStart.X && pos.Y == carsStart.Y)
            {
                return Direction.RIGHT;
            }
            else if (pos.X == carsEnd.X && pos.Y == carsEnd.Y)
            {
                return Direction.LEFT;
            }
            else
                throw new RoadException("Can not get a direction to move for a vehicle (Wrong vehicle position)");
        }

        public HorRoad(float x, float y, IGameContext gc) : base(x, y, gc)
        {
            picture = C.IHorRoadPic;
            this.size.Width = C.IHorRoadPic.Size.Width;
            this.size.Height = C.IHorRoadPic.Size.Height;
            carsStart = new PointF(position.X - C.VehicleSize.Width, position.Y + C.bigDelta);
            carsEnd = new PointF(position.X + C.IHorRoadPic.Size.Width + C.VehicleSize.Width, position.Y + C.smallDelta);
        }
    }
}
