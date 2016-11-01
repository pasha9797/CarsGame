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

        public virtual double[] GetStartPosition()
        {
            return new double[2]{ 0,0};
        }
        public virtual double[] GetEndPosition()
        {
            return new double[2] { 0, 0 };
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
