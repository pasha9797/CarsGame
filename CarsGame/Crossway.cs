using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;

namespace CarsGame
{
    public class Crossway
    {
        private PointF position;
        private Size size;
        private LightMode lightMode;
        private Road[] connectedRoads;
        private Timer changeLight;
        private Turn[] turning;
        private int crosswayChance;
        private Image lightImage;

        public PointF Position
        {
            get
            {
                return position;
            }
        }
        public Image LightImage
        {
            get
            {
                return lightImage;
            }
        }
        public Size Size
        {
            get
            {
                return size;
            }
        }
        public Turn[] Turning
        {
            get
            {
                return turning;
            }
        }
        public LightMode LightMode
        {
            get
            {
                return lightMode;
            }
            set
            {
                lightMode = value;
            }
        }
        public int CrosswayChance
        {
            get
            {
                return crosswayChance;
            }
            set
            {
                crosswayChance=value;
            }
        }
        public Road[] ConnectedRoads
        {
            get
            {
                return connectedRoads;
            }
            set
            {
                connectedRoads = value;
            }
        }

        public void ChangeLightMode(Object source, ElapsedEventArgs e)//меняем сигнал
        {
            if(lightMode==LightMode.HORGREEN || lightMode==LightMode.VERTGREEN)
            {
                int r = Field.Rand.Next(crosswayChance);
                if (r == 0)
                {
                    Broken();
                }
                else
                {
                    if (lightMode == LightMode.HORGREEN)
                        VertGreen();
                    else
                        HorGreen();
                }
            }
            if (crosswayChance > 1) crosswayChance--;
        }
        public void FixLightDelayed()//бригаду вызвать
        {
            Fixing();
            Timer fixTimer = new Timer(C.FixLightDelay);
            fixTimer.AutoReset = false;
            fixTimer.Elapsed += Delay;
            fixTimer.Start();
        }
        private void Delay(Object source, ElapsedEventArgs e)//починить окончательно
        {
            RandomMode();
        }
        private void RandomMode()//рандомно поставить режим светофора
        {
            int mode = Field.Rand.Next(2);
            if (mode == 0)
                HorGreen();
            else
                VertGreen();
        }
        private void HorGreen()//режим когда зеленый горит горизонтально
        {
            lightMode = LightMode.HORGREEN;
            lightImage = C.IHorGreenPic;
        }
        private void VertGreen()//режим когда зеленый горит вертикально
        {
            lightMode = LightMode.VERTGREEN;
            lightImage = C.IVertGreenPic;
        }
        private void Broken()//режим сломанного светофора
        {
            lightMode = LightMode.BROKEN;
            lightImage = C.IBrokenLightPic;
        }
        private void Fixing()//Светофор в ремонте
        {
            lightMode = LightMode.FIXING;
            lightImage = C.IFixingPic;
        }
        public PointF GetStartPosition(Direction direction)//позиция перед заездом на перекресток
        {
            switch (direction)
            {
                case Direction.RIGHT:
                    return new PointF(position.X - C.VehicleSize.Width, position.Y + C.bigDelta);
                case Direction.DOWN:
                    return new PointF(position.X + C.smallDelta, position.Y - C.VehicleSize.Width);
                case Direction.LEFT:
                    return new PointF(position.X + size.Width + C.VehicleSize.Width, position.Y + C.smallDelta);
                default:
                    return new PointF(position.X + C.bigDelta, position.Y + size.Width + C.VehicleSize.Width);
            }
        }
        public PointF GetAfterMiddlePosition(Direction direction)//задний бампер ровно на середине перекрестка
        {
            switch (direction)
            {
                case Direction.RIGHT:
                    return new PointF(position.X + size.Width / 2, position.Y + C.bigDelta);
                case Direction.DOWN:
                    return new PointF(position.X + C.smallDelta, position.Y + size.Width / 2);
                case Direction.LEFT:
                    return new PointF(position.X + size.Width / 2, position.Y + C.smallDelta);
                default:
                    return new PointF(position.X + C.bigDelta, position.Y + size.Width / 2);
            }
        }
        public PointF GetBeforeMiddlePosition(Direction direction)//передний бампер ровно на середине перекрестка
        {
            switch (direction)
            {
                case Direction.RIGHT:
                    return new PointF(position.X, position.Y + C.bigDelta);
                case Direction.DOWN:
                    return new PointF(position.X + C.smallDelta, position.Y);
                case Direction.LEFT:
                    return new PointF(position.X + size.Width, position.Y + C.smallDelta);
                default:
                    return new PointF(position.X + C.bigDelta, position.Y + size.Width);
            }
        }

        public Crossway(float x, float y)
        {
            position = new PointF( x, y );
            size = new Size();
            turning = new Turn[3];
            size.Width = C.ICrossPic.Size.Width;
            size.Height = C.ICrossPic.Size.Height;
            RandomMode();
            crosswayChance = C.CrosswayChance;
            connectedRoads = new Road[4];
            changeLight = new Timer(Field.Rand.Next(C.ChangeLightMin, C.ChangeLightMax));
            changeLight.AutoReset = true;
            changeLight.Elapsed += ChangeLightMode;
            changeLight.Start();
        }
    }
}
