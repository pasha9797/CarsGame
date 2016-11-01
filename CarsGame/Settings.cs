using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarsGame
{
    public static class C
    {
        public const int UP = 0;
        public const int RIGHT = 1;
        public const int DOWN = 2;
        public const int LEFT = 3;

        public const int CAR = 0;
        public const int TRUCK = 1;
        public const int BUS = 2;

        public static int X = 0;
        public static int Y = 1;

        public static int ONROAD = 0;
        public static int ONCROSSWAY = 1;

        public static int HORGREEN = 0;
        public static int VERTGREEN = 1;
        public static int BROKENLIGHT = 2;
        public static int FIXINGLIGHT = 3;

        public static int UpdateInterval = 50;
        public static int CreateVehiclesInterval = 2000;
        public static int DayTime = 1000;
        public static int TrafficLightChangeIntervalMax = 5000;
        public static int TrafficLightChangeIntervalMin = 3500;
        public static int AlertTime = 2000;
        public static int FixLightDelay = 4500;
        public static int KillVehicleDelay = 10000;
        public static int VehicleStep = 5;//на сколько пикселей сдвинется машина
        public static int OutOfView = 50;
        public static int CrosswayChance = 7;//чем больше цифра, тем меньше вероятность поломки
        public static int DailyScore = 350;
        public static int CrashScore = 115;
        public static int mainInfoWidth = 300;
        public static int mainInfoHeight = 110;
        public static int gameoverWidth = 480;
        public static int gameoverHeight = 200;

        public static int RoadsNumber = 12;
        public static int CrosswaysNumber = 4;

        public static string CrashSound = "../../Images/crashsound.wav";
        public static string FixSound = "../../Images/fixsound.wav";

        public static string CarPic = "../../Images/car.png";
        public static string TruckPic = "../../Images/truck.png";
        public static string BusPic = "../../Images/bus.png";
        public static string CarBrokenPic = "../../Images/car_broken.png";
        public static string TruckBrokenPic = "../../Images/truck_broken.png";
        public static string BusBrokenPic = "../../Images/bus_broken.png";
        public static string FieldPic = "../../Images/field.png";
        public static string VertRoadPic = "../../Images/vert_road.png";
        public static string HorRoadPic = "../../Images/hor_road.png";
        public static string CrossPic = "../../Images/crossway.png";
        public static string HorGreenPic = "../../Images/horgreen.png";
        public static string VertGreenPic = "../../Images/VertGreen.png";
        public static string BrokenLightPic = "../../Images/brokenlight.png";
        public static string FixingLightPic = "../../Images/fixinglight.png";

        public static Image ICarPic = Image.FromFile(CarPic);
        public static Image ITruckPic = Image.FromFile(TruckPic);
        public static Image IBusPic = Image.FromFile(BusPic);
        public static Image ICarBrokenPic = Image.FromFile(CarBrokenPic);
        public static Image ITruckBrokenPic = Image.FromFile(TruckBrokenPic);
        public static Image IBusBrokenPic = Image.FromFile(BusBrokenPic);
        public static Image IFieldPic = Image.FromFile(FieldPic);
        public static Image IVertRoadPic = Image.FromFile(VertRoadPic);
        public static Image IHorRoadPic = Image.FromFile(HorRoadPic);
        public static Image ICrossPic = Image.FromFile(CrossPic);
        public static Image IHorGreenPic = Image.FromFile(HorGreenPic);
        public static Image IVertGreenPic = Image.FromFile(VertGreenPic);
        public static Image IBrokenLightPic = Image.FromFile(BrokenLightPic);
        public static Image IFixingLightPic = Image.FromFile(FixingLightPic);

        public static Size HorRoadSize = Image.FromFile(HorRoadPic).Size;
        public static Size VertRoadSize = Image.FromFile(VertRoadPic).Size;
        public static Size CrossSize = Image.FromFile(CrossPic).Size;
        public static Size CarSize = Image.FromFile(CarPic).Size;
        public static Size TruckSize = Image.FromFile(TruckPic).Size;
        public static Size BusSize = Image.FromFile(BusPic).Size;

        public static int StepsToPassVertRoad = (int)(VertRoadSize.Height/VehicleStep*0.51);
        public static int StepsToPassHorRoad = (int)(HorRoadSize.Width / VehicleStep*0.75);
        public static int StepsToPassCrosswayStraight = (int)(CrossSize.Height / VehicleStep * 1.8);
        public static int StepsToPassCrosswayRight = (int)(CrossSize.Height / VehicleStep *0.6);
        public static int StepsToPassCrosswayLeft = (int)(CrossSize.Height / VehicleStep*1.9);
        public static int StepsToPassCrosswayLeftHalf = (int)(CrossSize.Height / VehicleStep * 1.3);
    }
}
