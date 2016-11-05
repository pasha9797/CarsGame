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
        public static Image IFixingLightPic = Image.FromFile(Root + "fixinglight.png");

        //*** Размеры машин ***
        public static Size VehicleSize = ICarPic.Size;
        public static Size EvacSize = IEvacPic.Size;

        //*** Настраиваемые величины  ***
        public static int UpdateInterval = 50; //интервал передвижения и перерисовки
        public static int CreateVehiclesInterval = 2000; //интервал создания новых машин
        public static int DayTime = 1000; //длительность одного часа
        public static int TrafficLightChangeIntervalMax = 8000; //максимальное время между переключениями режимов светофора
        public static int TrafficLightChangeIntervalMin = 5000; //минимальное такое время
        public static int AlertTime = 2000; //длительность отображения уведомления
        public static int FixLightDelay = 4500; //за сколько будет починен светофор
        public static int KillVehicleDelay = 20000; //через сколько будет удалена машина если за ней не приедет эвакуатор
        public static int VehicleStep = 5; //на сколько пикселей сдвинется машина за 1 шаг
        public static int CrosswayChance = 20; //число, обратное шансу поломки светофора
        public static int DailyScore = 350; //количество очков начисляемых каждые сутки
        public static int CrashScore = 115; //сколько очков снимается за разбитую машину
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
        public static double bigDelta = (C.IHorRoadPic.Size.Height * 1.5 - C.VehicleSize.Height) / 2;
        public static double smallDelta = (C.IHorRoadPic.Size.Height / 2 + C.VehicleSize.Height) / 2;

        //*** Обозначения ***
        public const int UP = 0;
        public const int RIGHT = 1;
        public const int DOWN = 2;
        public const int LEFT = 3;

        public const int CAR = 0;
        public const int TRUCK = 1;
        public const int BUS = 2;
        public const int EVAC = 3;

        public static int X = 0;
        public static int Y = 1;

        public static int ONROAD = 0;
        public static int ONCROSSWAY = 1;

        public static int HORGREEN = 0;
        public static int VERTGREEN = 1;
        public static int BROKENLIGHT = 2;
        public static int FIXINGLIGHT = 3;
    }
}
