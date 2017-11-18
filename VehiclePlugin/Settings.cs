using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsGameLib
{
    public delegate void AlertMethod(string message, Brush color);
    public delegate void CallEvacMethod(Vehicle target);
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

    public static class C
    {
        //*** Каталог файлов ***
        private const string Root = "../../Images/";

        //*** Звуки ***
        public const string CrashSound = Root + "crashsound.wav";
        public const string FixSound = Root + "fixsound.wav";

        //*** Изображения ***
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

        //*** Настраиваемые величины  ***
        public const int UpdateInterval = 100; //интервал передвижения и перерисовки
        public const int CreateVehiclesInterval = 2000; //интервал создания новых машин 
        public const int DayLength = 1000; //длительность одного часа
        public const int ChangeLightMax = 8000; //максимальное время между переключениями режимов светофора
        public const int ChangeLightMin = 5000; //минимальное такое время
        public const int AlertTime = 2000; //длительность отображения уведомления
        public const int FixLightDelay = 4500; //за сколько будет починен светофор
        public const int KillVehicleDelay = 20000; //через сколько будет удалена машина если за ней не приедет эвакуатор
        public const int VehicleStep = 10; //на сколько пикселей сдвинется машина за 1 шаг 
        public const int CrosswayChance = 15; //число, обратное шансу поломки светофора
        public const int StartScore = 450; //количество очков начисляемых каждые сутки
        public const int SuccessVehicleScore = 35;
        public const int CrashScore = 95; //сколько очков снимается за разбитую машину
        public const int InspectionScore = 500; //количество очков для тех.осмотра светофоров
        public const int mainInfoWidth = 300; //ширина основного информационного поля
        public const int mainInfoHeight = 110; //его высота
        public const int gameoverWidth = 480; //ширина уведомления о конце игры
        public const int gameoverHeight = 200; //его высота
        public static Size VehicleSize = new Size(111, 42);

        //*** Неизменяемые величины  ***
        public const int RoadsNumber = 12;
        public const int CrosswaysNumber = 4;
        public static int StepsToPassVertRoad = (int)((IVertRoadPic.Size.Height - VehicleSize.Width) / VehicleStep);
        public static int StepsToPassHorRoad = (int)((IHorRoadPic.Size.Width - VehicleSize.Width) / VehicleStep);
        public static int StepsToPassCrosswayStraight = (int)((ICrossPic.Size.Height + VehicleSize.Width) / VehicleStep);
        public static int StepsToPassCrosswayRight = (int)(ICrossPic.Size.Height / VehicleStep);
        public static int StepsToPassCrosswayLeft = (int)(ICrossPic.Size.Height / VehicleStep * 2);
        public static float bigDelta = (C.IHorRoadPic.Size.Height * 1.5F - C.VehicleSize.Height) / 2;
        public static float smallDelta = (C.IHorRoadPic.Size.Height / 2 + C.VehicleSize.Height) / 2;

        public static bool InBounds(float point, float first, float second)// point принадл [first, second]
        {
            return ((point >= first && point <= second) || (point >= second && point <= first));
        }
    }
}
