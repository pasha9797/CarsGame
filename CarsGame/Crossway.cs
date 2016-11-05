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
        private double[] position;
        private double[] size;
        private int lightMode;
        private VertRoad[] upDownConnected;
        private HorRoad[] leftRightConnected;
        private Timer changeLight;
        private int[] turning;
        private int crosswayChance;

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
        public int[] Turning
        {
            get
            {
                return turning;
            }
        }
        public int LightMode
        {
            get
            {
                return lightMode;
            }
            set
            {
                if (value >= 0 && value <= 2) lightMode = value;
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
        public VertRoad[] UpDownConnected
        {
            get
            {
                return upDownConnected;
            }
            set
            {
                upDownConnected = value;
            }
        }
        public HorRoad[] LeftRightConnected
        {
            get
            {
                return leftRightConnected;
            }
            set
            {
                leftRightConnected = value;
            }
        }

        public void ChangeLightMode(Object source, ElapsedEventArgs e)//меняем сигнал
        {
            if(lightMode==C.HORGREEN || lightMode==C.VERTGREEN)
            {
                int r = RandomGen.Rand.Next(crosswayChance);
                if (r == 0)
                {
                    lightMode = C.BROKENLIGHT;
                }
                else lightMode = 1 - lightMode;
            }
            if (crosswayChance > 1) crosswayChance--;
        }
        public void FixLightDelayed()//бригаду вызвать
        {
            lightMode = C.FIXINGLIGHT;
            Timer fixTimer = new Timer(C.FixLightDelay);
            fixTimer.AutoReset = false;
            fixTimer.Elapsed += Delay;
            fixTimer.Start();
        }
        private void Delay(Object source, ElapsedEventArgs e)//починить окончательно
        {
            lightMode = C.HORGREEN;
        }
        public Road GetTurnRoad(Vehicle veh)//на какую дорогу свернет данная машина
        {
            switch(veh.Direction)
            {
                case C.UP:
                    return upDownConnected[0];
                case C.DOWN:
                    return upDownConnected[1];
                case C.LEFT:
                    return leftRightConnected[0];
                case C.RIGHT:
                    return leftRightConnected[1];
                default:
                    return null;
            }
        }
        public double[] GetStartPosition(int direction)
        {
            switch(direction)
            {
                case C.RIGHT:
                    return new double[2] { position[C.X]-C.VehicleSize.Width, position[C.Y] + C.bigDelta };
                case C.DOWN:
                    return new double[2] { position[C.X]+C.smallDelta, position[C.Y]-C.VehicleSize.Width };
                case C.LEFT:
                    return new double[2] { position[C.X]+size[C.X]+C.VehicleSize.Width, position[C.Y] + C.smallDelta };
                default:
                    return new double[2] { position[C.X]+C.bigDelta, position[C.Y] + size[C.X] + C.VehicleSize.Width };
            }
        }
        public double[] GetAfterMiddlePosition(int direction)
        {
            switch (direction)
            {
                case C.RIGHT:
                    return new double[2] { position[C.X]+size[C.X]/2, position[C.Y] + C.bigDelta };
                case C.DOWN:
                    return new double[2] { position[C.X] + C.smallDelta, position[C.Y]+ size[C.X] / 2 };
                case C.LEFT:
                    return new double[2] { position[C.X] +size[C.X]/2, position[C.Y] + C.smallDelta };
                default:
                    return new double[2] { position[C.X] + C.bigDelta, position[C.Y] + size[C.X] /2};
            }
        }
        public double[] GetBeforeMiddlePosition(int direction)
        {
            switch (direction)
            {
                case C.RIGHT:
                    return new double[2] { position[C.X], position[C.Y] + C.bigDelta };
                case C.DOWN:
                    return new double[2] { position[C.X] + C.smallDelta, position[C.Y]};
                case C.LEFT:
                    return new double[2] { position[C.X] + size[C.X], position[C.Y] + C.smallDelta };
                default:
                    return new double[2] { position[C.X] + C.bigDelta, position[C.Y] + size[C.X] };
            }
        }

        public Crossway(double x, double y)
        {
            position = new double[2] { x, y };
            size = new double[2];
            turning = new int[3];
            size[C.X] = C.ICrossPic.Size.Width;
            size[C.Y] = C.ICrossPic.Size.Height;
            lightMode = C.HORGREEN;
            crosswayChance = C.CrosswayChance;
            upDownConnected = new VertRoad[2];
            leftRightConnected = new HorRoad[2];
            changeLight = new Timer(RandomGen.Rand.Next(C.TrafficLightChangeIntervalMin, C.TrafficLightChangeIntervalMax));
            changeLight.AutoReset = true;
            changeLight.Elapsed += ChangeLightMode;
            changeLight.Start();
        }
    }
}
