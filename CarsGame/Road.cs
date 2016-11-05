using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace CarsGame
{
    public class Road
    {
        protected double[] position;
        protected double[] carsStart, carsEnd;
        protected double[] size;
        protected Image picture;
        protected Crossway startCrossway;
        protected Crossway endCrossway;

        public double[] Position
        {
            get
            {
                return position;
            }
        }
        public double[] Size
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

        public double[] GetStartPosition()//поставить машину на начало дороги
        {
            return new double[2] { carsStart[C.X], carsStart[C.Y] };
        }
        public double[] GetEndPosition()//поставить машину на конец дороги
        {
            return new double[2] { carsEnd[C.X], carsEnd[C.Y] };
        }
        public double[] GetResumePosition(int beg_end, int direction)//поставить машину на начало дороги
        {
            double[] coords;
            if (beg_end == 0) coords = new double[2] { carsStart[C.X], carsStart[C.Y] };
            else coords = new double[2] { carsEnd[C.X], carsEnd[C.Y] };
            switch (direction)
            {
                case C.RIGHT:
                    coords[C.X] += C.VehicleSize.Width;
                    break;
                case C.DOWN:
                    coords[C.Y] += C.VehicleSize.Width;
                    break;
                case C.LEFT:
                    coords[C.X] -= C.VehicleSize.Width;
                    break;
                default:
                    coords[C.Y] -= C.VehicleSize.Width;
                        break;
            }
            return coords;
        }
        public double[] GetEndResumePosition(int direction)//поставить машину на конец дороги
        {
            return new double[2] { carsEnd[C.X], carsEnd[C.Y] };
        }

        public double[] GetNullPosition()//Конец дороги, идущий от края экрана
        {
            if (this.startCrossway == null)
            {
                return GetStartPosition();
            }
            else if (this.endCrossway == null)
            {
                return GetEndPosition();
            }
            else return null;
        }
        public virtual int GetDirectionToMove(double[] pos)
        {
            return -1;
        }

        public Road(double x, double y)
        {
            position = new double[2] { x, y };
            size = new double[2];
        }
    }
}
