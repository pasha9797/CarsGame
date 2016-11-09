using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace CarsGame
{
    public abstract class Road
    {
        protected PointF position;
        protected PointF carsStart, carsEnd;
        protected Size size;
        protected Image picture;
        protected Crossway startCrossway;
        protected Crossway endCrossway;

        public PointF Position
        {
            get
            {
                return position;
            }
        }
        public Size Size
        {
            get
            {
                return size;
            }
        }
        public Image Picture
        {
            get
            {
                return picture;
            }
        }
        public Crossway StartCrossway
        {
            get
            {
                return startCrossway;
            }
            set
            {
                startCrossway = value;
            }
        }
        public Crossway EndCrossway
        {
            get
            {
                return endCrossway;
            }
            set
            {
                endCrossway = value;
            }
        }

        public PointF GetStartPosition()//позиция перед заездом на начало дороги
        {
            return carsStart;
        }
        public PointF GetEndPosition()//позиция перед заездом на конец дороги
        {
            return carsEnd;
        }
        public PointF GetResumePosition(int beg_end, Direction direction)//позиция после проезда перекрёстка
        {
            PointF coords;
            if (beg_end == 0)
                coords = carsStart;
            else coords = carsEnd;
            switch (direction)
            {
                case Direction.RIGHT:
                    coords.X += C.VehicleSize.Width;
                    break;
                case Direction.DOWN:
                    coords.Y += C.VehicleSize.Width;
                    break;
                case Direction.LEFT:
                    coords.X -= C.VehicleSize.Width;
                    break;
                default:
                    coords.Y -= C.VehicleSize.Width;
                        break;
            }
            return coords;
        }
        public PointF GetNullPosition()//Конец дороги, идущий от края экрана
        {
            if (this.startCrossway == null)
            {
                return GetStartPosition();
            }
            else
            {
                return GetEndPosition();
            }
        }
        public abstract Direction GetDirectionToMove(PointF pos);//направление движения

        public Road(float x, float y)
        {
            position = new PointF(x, y );
            size = new Size();
        }
    }
}
