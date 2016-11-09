using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarsGame
{
    public enum Direction
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
    public enum Turn
    {
        STRAIGHT = 0,
        RIGHT = 1,
        LEFT = -1
    }
    public enum LightMode
    {
        HORGREEN,
        VERTGREEN,
        BROKEN,
        FIXING
    }
    public enum VehType
    {
        CAR,
        TRUCK,
        BUS,
        EVAC
    }

    public static class C
    {
        //*** Каталог файлов ***
        private static string Root = "../../Images/";

        //*** Звуки ***
        public static string CrashSound = Root + "crashsound.wav";
        public static string FixSound = Root + "fixsound.wav";

        //*** Изображения ***
        public static Image ICarPic = Image.FromFile(Root + "car.png");
        public static Image ITruckPic = Image.FromFile(Root + "truck.png");
        public static Image IBusPic = Image.FromFile(Root + "bus.png");
        public static Image IEvacPic = Image.FromFile(Root + "evac.png");
        public static Image ICarBrokenPic = Image.FromFile(Root + "car_broken.png");
        public static Image ITruckBrokenPic = Image.FromFile(Root + "truck_broken.png");
        public static Image IBusBrokenPic = Image.FromFile(Root + "bus_broken.png");
        public static Image IEvacBrokenPic = Image.FromFile(Root + "evac_broken.png");
        public static Image IFieldPic = Image.FromFile(Root + "field.png");
        public static Image IVertRoadPic = Image.FromFile(Root + "vert_road.png");
        public static Image IHorRoadPic = Image.FromFile(Root + "hor_road.png");
        public static Image ICrossPic = Image.FromFile(Root + "crossway.png");
        public static Image IHorGreenPic = Image.FromFile(Root + "horgreen.png");
        public static Image IVertGreenPic = Image.FromFile(Root + "VertGreen.png");
        public static Image IBrokenLightPic = Image.FromFile(Root + "brokenlight.png");
        public static Image IFixingPic = Image.FromFile(Root + "fixinglight.png");
        public static Image ICrossInfoPic = Image.FromFile(Root + "crossinfo.png");
        public static Image ITurnRightPic = Image.FromFile(Root + "turnright.png");
        public static Image ITurnLeftPic = Image.FromFile(Root + "turnleft.png");
        public static Image ITurnStraightPic = Image.FromFile(Root + "turnup.png");

        //*** Размеры машин ***
        public static Size VehicleSize = ICarPic.Size;
        public static Size EvacSize = IEvacPic.Size;

        //*** Настраиваемые величины  ***
        public static int UpdateInterval = 100; //интервал передвижения и перерисовки
        public static int CreateVehiclesInterval = 2000; //интервал создания новых машин
        public static int DayLength = 1000; //длительность одного часа
        public static int ChangeLightMax = 8000; //максимальное время между переключениями режимов светофора
        public static int ChangeLightMin = 5000; //минимальное такое время
        public static int AlertTime = 2000; //длительность отображения уведомления
        public static int FixLightDelay = 4500; //за сколько будет починен светофор
        public static int KillVehicleDelay = 20000; //через сколько будет удалена машина если за ней не приедет эвакуатор
        public static int VehicleStep = 10; //на сколько пикселей сдвинется машина за 1 шаг
        public static int CrosswayChance = 15; //число, обратное шансу поломки светофора
        public static int StartScore = 450; //количество очков начисляемых каждые сутки
        public static int SuccessVehicleScore = 35;
        public static int CrashScore = 95; //сколько очков снимается за разбитую машину
        public static int InspectionScore = 500; //количество очков для тех.осмотра светофоров
        public static int mainInfoWidth = 300; //ширина основного информационного поля
        public static int mainInfoHeight = 110; //его высота
        public static int gameoverWidth = 480; //ширина уведомления о конце игры
        public static int gameoverHeight = 200; //его высота

        //*** Неизменяемые величины  ***
        public static int RoadsNumber = 12;
        public static int CrosswaysNumber = 4;
        public static int StepsToPassVertRoad = (int)((IVertRoadPic.Size.Height-VehicleSize.Width) / VehicleStep);
        public static int StepsToPassHorRoad = (int)((IHorRoadPic.Size.Width - VehicleSize.Width) / VehicleStep);
        public static int StepsToPassCrosswayStraight = (int)((ICrossPic.Size.Height + VehicleSize.Width) / VehicleStep);
        public static int StepsToPassCrosswayRight = (int)(ICrossPic.Size.Height / VehicleStep);
        public static int StepsToPassCrosswayLeft = (int)(ICrossPic.Size.Height / VehicleStep * 2);
        public static float bigDelta = (C.IHorRoadPic.Size.Height * 1.5F - C.VehicleSize.Height) / 2;
        public static float smallDelta = (C.IHorRoadPic.Size.Height / 2 + C.VehicleSize.Height) / 2;
    }
}
