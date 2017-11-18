using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CarsGameLib
{
    public class Crossway
    {
        private PointF position;
        private Size size;
        private LightMode lightMode;
        private Road[] connectedRoads;
        private Timer changeLight;
        private Dictionary<Type, Turn> turning;
        private int crosswayChance;
        private Image lightImage;
        private IGameContext context;

        public IGameContext Context
        {
            get
            {
                return context;
            }
            set
            {
                context = value;
            }
        }
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
        public Dictionary<Type, Turn> Turning
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
                crosswayChance = value;
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
            if (lightMode == LightMode.HORGREEN || lightMode == LightMode.VERTGREEN)
            {
                int r = Context.Rand.Next(crosswayChance);
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
            if (lightMode == LightMode.BROKEN)
            {
                Fixing();
                Timer fixTimer = new Timer(C.FixLightDelay);
                fixTimer.AutoReset = false;
                fixTimer.Elapsed += Delay;
                fixTimer.Start();
            }
            else
                throw new CrosswayException("Trying to fix light that is not broken");
        }
        private void Delay(Object source, ElapsedEventArgs e)//починить окончательно
        {
            RandomMode();
        }
        private void RandomMode()//рандомно поставить режим светофора
        {
            int mode = Context.Rand.Next(2);
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
                case Direction.UP:
                    return new PointF(position.X + C.bigDelta, position.Y + size.Width + C.VehicleSize.Width);
                default:
                    return new PointF(0, 0);
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
                case Direction.UP:
                    return new PointF(position.X + C.bigDelta, position.Y + size.Width / 2);
                default:
                    return new PointF(0, 0);
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
                case Direction.UP:
                    return new PointF(position.X + C.bigDelta, position.Y + size.Width);
                default:
                    return new PointF(0, 0);
            }
        }

        public Crossway(float x, float y, IGameContext gc)
        {
            Context = gc;
            position = new PointF(x, y);
            size = new Size();
            turning = new Dictionary<Type, Turn>();
            size.Width = C.ICrossPic.Size.Width;
            size.Height = C.ICrossPic.Size.Height;
            RandomMode();
            crosswayChance = C.CrosswayChance;
            connectedRoads = new Road[4];
            changeLight = new Timer(Context.Rand.Next(C.ChangeLightMin, C.ChangeLightMax));
            changeLight.AutoReset = true;
            changeLight.Elapsed += ChangeLightMode;
            changeLight.Start();
        }
    }
}
