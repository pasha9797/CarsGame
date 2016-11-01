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
                int r = RandomGen.Rand.Next(C.CrosswayChance);
                if (r == 0)
                {
                    lightMode = C.BROKENLIGHT;
                }
                else lightMode = 1 - lightMode;
            }
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

        public Crossway(double x, double y)
        {
            position = new double[2] { x, y };
            size = new double[2];
            turning = new int[3];
            size[C.X] = C.CrossSize.Width;
            size[C.Y] = C.CrossSize.Height;
            lightMode = C.HORGREEN;
            upDownConnected = new VertRoad[2];
            leftRightConnected = new HorRoad[2];
            changeLight = new Timer(RandomGen.Rand.Next(C.TrafficLightChangeIntervalMin, C.TrafficLightChangeIntervalMax));
            changeLight.AutoReset = true;
            changeLight.Elapsed += ChangeLightMode;
            changeLight.Start();
        }
    }
}
