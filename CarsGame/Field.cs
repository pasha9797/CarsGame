using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;

namespace CarsGame
{
    public static class Field
    {
        private static Random rand = new Random();
        private static Road[] roads = new Road[12];
        private static Crossway[] crossways = new Crossway[4];
        private static List<Vehicle> vehicles = new List<Vehicle>();
        private static List<Road> startingRoads = new List<Road>();
        private static Queue<Evacuator> evacsToCreate = new Queue<Evacuator>();
        private static Timer killtext = new Timer(C.AlertTime);
        private static string message = null;
        private static Brush messageColor;
        private static int stepsForGenerate = 0;
        private static int stepsForChangeHour = 0;
        private static int score = C.StartScore;
        private static int hour = 0;
        private static Font smallFont = new Font("Tahoma", 12);
        private static Font bigFont = new Font("Tahoma", 48);
        private static Brush infoBrush = Brushes.Blue;
        private static Brush gameoverBrush = Brushes.Red;
        private static int linestep = 23;
        private static float mainInfoStart = Program.mainForm.width / 2 - (C.mainInfoWidth / 2);
        private static float gameoverStartX = Program.mainForm.width / 2 - (C.gameoverWidth / 2);
        private static float gameoverStartY = Program.mainForm.height / 2 - (C.gameoverHeight / 2);
        private static bool gameOver = false;
        private static Brush brush = Brushes.Green;
        private static System.Media.SoundPlayer player = new System.Media.SoundPlayer();

        public static int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }
        public static Random Rand
        {
            get
            {
                return rand;
            }
        }

        public static void Initialize()//генерация поля и запуск игры
        {
            //горизонтальные дороги
            roads[0] = new HorRoad(0, C.IVertRoadPic.Size.Height);
            roads[1] = new HorRoad(C.IHorRoadPic.Size.Width + C.ICrossPic.Size.Width, C.IVertRoadPic.Size.Height);
            roads[2] = new HorRoad((C.IHorRoadPic.Size.Width + C.ICrossPic.Size.Width) * 2, C.IVertRoadPic.Size.Height);
            roads[3] = new HorRoad(0, C.IHorRoadPic.Size.Height + C.IVertRoadPic.Size.Height * 2);
            roads[4] = new HorRoad(C.IHorRoadPic.Size.Width + C.ICrossPic.Size.Width, C.IHorRoadPic.Size.Height + C.IVertRoadPic.Size.Height * 2);
            roads[5] = new HorRoad((C.IHorRoadPic.Size.Width + C.ICrossPic.Size.Width) * 2, C.IHorRoadPic.Size.Height + C.IVertRoadPic.Size.Height * 2);

            //вертикальные дороги
            roads[6] = new VertRoad(C.IHorRoadPic.Size.Width, 0);
            roads[7] = new VertRoad(C.IHorRoadPic.Size.Width * 2 + C.ICrossPic.Size.Width, 0);
            roads[8] = new VertRoad(C.IHorRoadPic.Size.Width, C.IVertRoadPic.Size.Height + C.ICrossPic.Size.Height);
            roads[9] = new VertRoad(C.IHorRoadPic.Size.Width * 2 + C.ICrossPic.Size.Width, C.IVertRoadPic.Size.Height + C.ICrossPic.Size.Height);
            roads[10] = new VertRoad(C.IHorRoadPic.Size.Width, (C.IVertRoadPic.Size.Height + C.ICrossPic.Size.Height) * 2);
            roads[11] = new VertRoad(C.IHorRoadPic.Size.Width * 2 + C.ICrossPic.Size.Width, (C.IVertRoadPic.Size.Height + C.ICrossPic.Size.Height) * 2);

            //перекрёстки
            crossways[0] = new Crossway(C.IHorRoadPic.Size.Width, C.IVertRoadPic.Size.Height);
            crossways[1] = new Crossway(C.IHorRoadPic.Size.Width * 2 + C.ICrossPic.Size.Width, C.IVertRoadPic.Size.Height);
            crossways[2] = new Crossway(C.IHorRoadPic.Size.Width, C.IVertRoadPic.Size.Height * 2 + C.ICrossPic.Size.Height);
            crossways[3] = new Crossway(C.IHorRoadPic.Size.Width * 2 + C.ICrossPic.Size.Width, C.IVertRoadPic.Size.Height * 2 + C.ICrossPic.Size.Height);

            //прописываем какими перекрестками начинается и заканчивается каждая дорога
            roads[0].StartCrossway = null; roads[0].EndCrossway = crossways[0];
            roads[1].StartCrossway = crossways[0]; roads[1].EndCrossway = crossways[1];
            roads[2].StartCrossway = crossways[1]; roads[2].EndCrossway = null;
            roads[3].StartCrossway = null; roads[3].EndCrossway = crossways[2];
            roads[4].StartCrossway = crossways[2]; roads[4].EndCrossway = crossways[3];
            roads[5].StartCrossway = crossways[3]; roads[5].EndCrossway = null;
            roads[6].StartCrossway = null; roads[6].EndCrossway = crossways[0];
            roads[7].StartCrossway = null; roads[7].EndCrossway = crossways[1];
            roads[8].StartCrossway = crossways[0]; roads[8].EndCrossway = crossways[2];
            roads[9].StartCrossway = crossways[1]; roads[9].EndCrossway = crossways[3];
            roads[10].StartCrossway = crossways[2]; roads[10].EndCrossway = null;
            roads[11].StartCrossway = crossways[3]; roads[11].EndCrossway = null;

            //список дорог, которые начинаются с краю экрана
            startingRoads.Add(roads[0]);
            startingRoads.Add(roads[2]);
            startingRoads.Add(roads[3]);
            startingRoads.Add(roads[5]);
            startingRoads.Add(roads[6]);
            startingRoads.Add(roads[7]);
            startingRoads.Add(roads[10]);
            startingRoads.Add(roads[11]);

            //прописываем какие дороги примыкают к каждому перекрестку
            crossways[0].ConnectedRoads[(int)Direction.UP] = roads[6] as VertRoad;
            crossways[0].ConnectedRoads[(int)Direction.DOWN] = roads[8] as VertRoad;
            crossways[0].ConnectedRoads[(int)Direction.LEFT] = roads[0] as HorRoad;
            crossways[0].ConnectedRoads[(int)Direction.RIGHT] = roads[1] as HorRoad;
            crossways[1].ConnectedRoads[(int)Direction.UP] = roads[7] as VertRoad;
            crossways[1].ConnectedRoads[(int)Direction.DOWN] = roads[9] as VertRoad;
            crossways[1].ConnectedRoads[(int)Direction.LEFT] = roads[1] as HorRoad;
            crossways[1].ConnectedRoads[(int)Direction.RIGHT] = roads[2] as HorRoad;
            crossways[2].ConnectedRoads[(int)Direction.UP] = roads[8] as VertRoad;
            crossways[2].ConnectedRoads[(int)Direction.DOWN] = roads[10] as VertRoad;
            crossways[2].ConnectedRoads[(int)Direction.LEFT] = roads[3] as HorRoad;
            crossways[2].ConnectedRoads[(int)Direction.RIGHT] = roads[4] as HorRoad;
            crossways[3].ConnectedRoads[(int)Direction.UP] = roads[9] as VertRoad;
            crossways[3].ConnectedRoads[(int)Direction.DOWN] = roads[11] as VertRoad;
            crossways[3].ConnectedRoads[(int)Direction.LEFT] = roads[4] as HorRoad;
            crossways[3].ConnectedRoads[(int)Direction.RIGHT] = roads[5] as HorRoad;

            //правила поворота машин на перекрестках
            crossways[0].Turning[(int)VehType.CAR] = Turn.LEFT;
            crossways[0].Turning[(int)VehType.TRUCK] = Turn.STRAIGHT;
            crossways[0].Turning[(int)VehType.BUS] = Turn.RIGHT;
            crossways[1].Turning[(int)VehType.CAR] = Turn.LEFT;
            crossways[1].Turning[(int)VehType.TRUCK] = Turn.RIGHT;
            crossways[1].Turning[(int)VehType.BUS] = Turn.STRAIGHT;
            crossways[2].Turning[(int)VehType.CAR] = Turn.STRAIGHT;
            crossways[2].Turning[(int)VehType.TRUCK] = Turn.LEFT;
            crossways[2].Turning[(int)VehType.BUS] = Turn.RIGHT;
            crossways[3].Turning[(int)VehType.CAR] = Turn.RIGHT;
            crossways[3].Turning[(int)VehType.TRUCK] = Turn.STRAIGHT;
            crossways[3].Turning[(int)VehType.BUS] = Turn.LEFT;

            killtext.AutoReset = false;
            killtext.Elapsed += KillText;
        }
        private static void GenerateVehicles()//удаление старых и добавл новых машин
        {
            //удаление уехавших
            int i = 0;
            while (i < vehicles.Count)
            {
                if (!vehicles[i].IsVisible())
                {
                    if (vehicles[i] is Evacuator)
                        (vehicles[i] as Evacuator).Target.Hide();
                    else if (!vehicles[i].Broken)
                        score += C.SuccessVehicleScore;
                    vehicles.RemoveAt(i);
                }
                else i++;
            }

            //генерация новых
            int whereToCreate = Rand.Next(8);
            Road road = startingRoads[whereToCreate];
            int vType = Rand.Next(3);
            Vehicle v = CreateVehicle((VehType)vType, road);
            if (FindVehicleInPoint(v.GetFrontPoint(), v) == null)
            {
                vehicles.Add(v);
            }

            //эвакуаторы
            if (evacsToCreate.Count > 0)
            {
                if (FindVehicleInPoint(evacsToCreate.First().GetFrontPoint(), evacsToCreate.First()) == null)
                {
                    vehicles.Add(evacsToCreate.Dequeue());
                }
            }
        }
        private static Vehicle CreateVehicle(VehType vType, Road road)//Создать авто
        {
            Vehicle v = null;
            switch (vType)
            {
                case VehType.CAR:
                    v = new Car(road);
                    break;
                case VehType.TRUCK:
                    v = new Truck(road);
                    break;
                case VehType.BUS:
                    v = new Bus(road);
                    break;
            }
            return v;
        }
        public static void CallEvacuator(Vehicle target)//заспавнить эвакуатор
        {
            Evacuator v = new Evacuator(target);
            v.Direction = Direction.DOWN;
            evacsToCreate.Enqueue(v);
        }
        public static Vehicle FindVehicleInPoint(PointF pos, Vehicle current)//поиск машины в точке
        {
            Vehicle veh = null;
            bool check = false;
            int i = 0;
            while (i < vehicles.Count && !check)
            {
                veh = vehicles[i];
                if (veh != current)
                {
                    switch (veh.Direction)
                    {
                        case Direction.UP:
                            check = (InBounds(pos.X, veh.Position.X, veh.Position.X + veh.Size.Height)) &&
                                (InBounds(pos.Y, veh.Position.Y, veh.Position.Y - veh.Size.Width));
                            break;
                        case Direction.DOWN:
                            check = (InBounds(pos.X, veh.Position.X, veh.Position.X - veh.Size.Height)) &&
                                 (InBounds(pos.Y, veh.Position.Y, veh.Position.Y + veh.Size.Width));
                            break;
                        case Direction.LEFT:
                            check = (InBounds(pos.X, veh.Position.X, veh.Position.X - veh.Size.Width)) &&
                                     (InBounds(pos.Y, veh.Position.Y, veh.Position.Y - veh.Size.Height));
                            break;
                        case Direction.RIGHT:
                            check = (InBounds(pos.X, veh.Position.X, veh.Position.X + veh.Size.Width)) &&
                                       (InBounds(pos.Y, veh.Position.Y, veh.Position.Y + veh.Size.Height));
                            break;
                    }
                }
                i++;
            }
            if (check) return veh;
            else return null;
        }
        public static bool InBounds(float point, float first, float second)// point принадл [first, second]
        {
            return ((point >= first && point <= second) || (point >= second && point <= first));
        }
        public static void CheckFixLight(float x, float y)//кликнул ли по светофору
        {
            int i = 0;
            bool found = false;
            while (i < crossways.Count() && !found)
            {
                if (x <= crossways[i].Position.X && x >= crossways[i].Position.X - C.IHorGreenPic.Size.Width
                    && y <= crossways[i].Position.Y && y >= crossways[i].Position.Y - C.IHorGreenPic.Size.Height)
                {
                    if (crossways[i].LightMode == LightMode.BROKEN)
                    {
                        PlaySound(C.FixSound);
                        Alert("Ремонтная бригада спешит на вызов!", Brushes.Green);
                        crossways[i].FixLightDelayed();//бригада едет какое-то время
                    }
                    else Alert("Ремонт не требуется!", Brushes.Red);
                    found = true;
                }
                i++;
            }
        }
        private static void Alert(string text, Brush color)//сообщение внизу
        {
            killtext.Stop();
            message = text;
            messageColor = color;
            killtext.Start();
        }
        private static void KillText(Object source, ElapsedEventArgs e)//убрать сообщение
        {
            message = null;
        }
        public static void MainUpdate()//основной таймер
        {
            foreach (Vehicle veh in vehicles)//двигаем машины
            {
                veh.Move();
            }

            //Генерация и удаление машин
            stepsForGenerate++;
            if (stepsForGenerate >= C.CreateVehiclesInterval / C.UpdateInterval)
            {
                GenerateVehicles();
                stepsForGenerate = 0;
            }

            //Смена часа
            stepsForChangeHour++;
            if (stepsForChangeHour >= C.DayLength / C.UpdateInterval)
            {
                hour++;
                stepsForChangeHour = 0;
            }
            //игра окончена
            if (score <= 0)
            {
                Program.mainForm.updateState.Stop();
                gameOver = true;
            }
        }
        public static void LightsInspection()//тех осмотр светофоров
        {
            if (score < C.InspectionScore)
                Alert("Вы должны иметь " + C.InspectionScore + " очков!", Brushes.Red);
            else
            {
                for (int i = 0; i < crossways.Count(); i++)
                {
                    crossways[i].CrosswayChance = C.CrosswayChance;
                    if (crossways[i].LightMode == LightMode.BROKEN) crossways[i].FixLightDelayed();
                }
                score -= C.InspectionScore;
                Alert("Тех. осмотр проведён!", Brushes.Green);
            }
        }
        private static Image TurnImage(int crossID, VehType vType)//определить значок поворота
        {
            switch (crossways[crossID].Turning[(int)vType])
            {
                case Turn.RIGHT:
                    return C.ITurnRightPic;
                case Turn.LEFT:
                    return C.ITurnLeftPic;
                default:
                    return C.ITurnStraightPic;
            }
        }
        public static void PlaySound(string path)//звук
        {
            player.SoundLocation = path;
            player.Load();
            player.Play();
        }
        public static void Draw(Graphics g)//обновление экрана
        {
            g.DrawImage(C.IFieldPic, 0, 0);
            for (int i = 0; i < C.RoadsNumber; i++)
            {
                g.DrawImage(roads[i].Picture, roads[i].Position.X, roads[i].Position.Y);
            }
            for (int i = 0; i < C.CrosswaysNumber; i++)
            {
                g.DrawImage(C.ICrossPic, (float)crossways[i].Position.X, (float)crossways[i].Position.Y);
                g.DrawImage(crossways[i].LightImage, (float)crossways[i].Position.X - C.IHorGreenPic.Size.Width, (float)crossways[i].Position.Y - C.IHorGreenPic.Size.Height);

                g.DrawImage(C.ICrossInfoPic, (float)crossways[i].Position.X + C.ICrossPic.Size.Width, (float)crossways[i].Position.Y - C.ICrossInfoPic.Size.Height * 2);
                g.DrawImage(TurnImage(i, VehType.CAR), (float)crossways[i].Position.X + C.ICrossPic.Size.Width, (float)crossways[i].Position.Y - C.ICrossInfoPic.Size.Height);
                g.DrawImage(TurnImage(i, VehType.TRUCK), (float)crossways[i].Position.X + C.ICrossPic.Size.Width + (C.ICrossInfoPic.Size.Width - C.ITurnRightPic.Size.Width) / 2, (float)crossways[i].Position.Y - C.ICrossInfoPic.Size.Height);
                g.DrawImage(TurnImage(i, VehType.BUS), (float)crossways[i].Position.X + C.ICrossPic.Size.Width + (C.ICrossInfoPic.Size.Width - C.ITurnRightPic.Size.Width), (float)crossways[i].Position.Y - C.ICrossInfoPic.Size.Height);
            }
            for (int i = 0; i < vehicles.Count; i++)
            {
                Vehicle vehicle = vehicles[i];
                if (vehicle.CurPlacement is Road) brush = Brushes.Red;
                else if (vehicle.CurPlacement is Crossway) brush = Brushes.Blue;
                else brush = Brushes.Green;
                //g.FillRectangle(brush, (float)vehicle.Position.X, (float)vehicle.Position.Y, 10, 10);
                g.TranslateTransform((float)vehicle.Position.X, (float)vehicle.Position.Y);
                switch (vehicle.Direction)
                {
                    case Direction.UP:
                        g.RotateTransform(-90);
                        g.DrawImage(vehicle.Picture, 0, 0);
                        break;
                    case Direction.DOWN:
                        g.RotateTransform(90);
                        g.DrawImage(vehicle.Picture, 0, 0);
                        break;
                    case Direction.LEFT:
                        g.RotateTransform(180);
                        g.DrawImage(vehicle.Picture, 0, 0);
                        break;
                    case Direction.RIGHT:
                        g.DrawImage(vehicle.Picture, 0, 0);
                        break;
                }
                g.ResetTransform();
            }
            g.FillRectangle(Brushes.White, mainInfoStart, 5, C.mainInfoWidth, C.mainInfoHeight);
            g.DrawString("Время: " + (hour % 24) + ":00", smallFont, infoBrush, mainInfoStart + 3, 6);
            g.DrawString("Счёт: " + score, smallFont, infoBrush, mainInfoStart + 3, 6 + linestep);
            g.DrawString("Количество машин: " + vehicles.Count, smallFont, infoBrush, mainInfoStart + 3, 6 + 2 * linestep);
            if (message != null)
                g.DrawString(message, smallFont, messageColor, mainInfoStart + 3, 6 + 3.5F * linestep);

            if (gameOver)
            {
                g.FillRectangle(Brushes.LightYellow, gameoverStartX, gameoverStartY, C.gameoverWidth, C.gameoverHeight);
                g.DrawString("Игра окончена!", bigFont, gameoverBrush, gameoverStartX + 2, gameoverStartY + C.gameoverHeight / 3);
                g.DrawString("Вы играли " + hour + " часов.", smallFont, gameoverBrush, gameoverStartX + 2, gameoverStartY + C.gameoverHeight / 3 + 100);
            }
        }
    }
}
