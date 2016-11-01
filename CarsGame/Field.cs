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
        private static Road[] roads = new Road[12];
        private static Crossway[] crossways = new Crossway[4];
        private static List<Vehicle> vehicles = new List<Vehicle>();
        private static Timer killtext = new Timer(C.AlertTime);
        private static string message = null;
        private static Brush messageColor;
        private static int stepsForGenerate = 0;
        private static int stepsForChangeHour = 0;
        private static int score = C.DailyScore;
        private static int hour = 7;
        private static Font smallFont = new Font("Tahoma", 12);
        private static Font bigFont = new Font("Tahoma", 48);
        private static Brush infoBrush = Brushes.Blue;
        private static Brush gameoverBrush = Brushes.Red;
        private static int linestep = 23;
        private static float mainInfoStart = Program.mainForm.width / 2 - (C.mainInfoWidth / 2);
        private static float gameoverStartX = Program.mainForm.width / 2 - (C.gameoverWidth / 2);
        private static float gameoverStartY = Program.mainForm.height / 2 - (C.gameoverHeight / 2);
        private static bool gameOver = false;

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

        public static void CreateRoadsAndCrossways()//генерация поля и запуск игры
        {
            roads[0] = new HorRoad(0, C.VertRoadSize.Height);
            roads[1] = new HorRoad(C.HorRoadSize.Width + C.CrossSize.Width, C.VertRoadSize.Height);
            roads[2] = new HorRoad((C.HorRoadSize.Width + C.CrossSize.Width) * 2, C.VertRoadSize.Height);

            roads[3] = new HorRoad(0, C.HorRoadSize.Height + C.VertRoadSize.Height * 2);
            roads[4] = new HorRoad(C.HorRoadSize.Width + C.CrossSize.Width, C.HorRoadSize.Height + C.VertRoadSize.Height * 2);
            roads[5] = new HorRoad((C.HorRoadSize.Width + C.CrossSize.Width) * 2, C.HorRoadSize.Height + C.VertRoadSize.Height * 2);

            roads[6] = new VertRoad(C.HorRoadSize.Width, 0);
            roads[7] = new VertRoad(C.HorRoadSize.Width * 2 + C.CrossSize.Width, 0);

            roads[8] = new VertRoad(C.HorRoadSize.Width, C.VertRoadSize.Height + C.CrossSize.Height);
            roads[9] = new VertRoad(C.HorRoadSize.Width * 2 + C.CrossSize.Width, C.VertRoadSize.Height + C.CrossSize.Height);

            roads[10] = new VertRoad(C.HorRoadSize.Width, (C.VertRoadSize.Height + C.CrossSize.Height) * 2);
            roads[11] = new VertRoad(C.HorRoadSize.Width * 2 + C.CrossSize.Width, (C.VertRoadSize.Height + C.CrossSize.Height) * 2);

            crossways[0] = new Crossway(C.HorRoadSize.Width, C.VertRoadSize.Height);
            crossways[1] = new Crossway(C.HorRoadSize.Width * 2 + C.CrossSize.Width, C.VertRoadSize.Height);
            crossways[2] = new Crossway(C.HorRoadSize.Width, C.VertRoadSize.Height * 2 + C.CrossSize.Height);
            crossways[3] = new Crossway(C.HorRoadSize.Width * 2 + C.CrossSize.Width, C.VertRoadSize.Height * 2 + C.CrossSize.Height);

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

            crossways[0].UpDownConnected[0] = roads[6] as VertRoad;
            crossways[0].UpDownConnected[1] = roads[8] as VertRoad;
            crossways[0].LeftRightConnected[0] = roads[0] as HorRoad;
            crossways[0].LeftRightConnected[1] = roads[1] as HorRoad;

            crossways[1].UpDownConnected[0] = roads[7] as VertRoad;
            crossways[1].UpDownConnected[1] = roads[9] as VertRoad;
            crossways[1].LeftRightConnected[0] = roads[1] as HorRoad;
            crossways[1].LeftRightConnected[1] = roads[2] as HorRoad;

            crossways[2].UpDownConnected[0] = roads[8] as VertRoad;
            crossways[2].UpDownConnected[1] = roads[10] as VertRoad;
            crossways[2].LeftRightConnected[0] = roads[3] as HorRoad;
            crossways[2].LeftRightConnected[1] = roads[4] as HorRoad;

            crossways[3].UpDownConnected[0] = roads[9] as VertRoad;
            crossways[3].UpDownConnected[1] = roads[11] as VertRoad;
            crossways[3].LeftRightConnected[0] = roads[4] as HorRoad;
            crossways[3].LeftRightConnected[1] = roads[5] as HorRoad;

            crossways[0].Turning[C.CAR] = C.LEFT;
            crossways[0].Turning[C.TRUCK] = C.UP;
            crossways[0].Turning[C.BUS] = C.RIGHT;

            crossways[1].Turning[C.CAR] = C.LEFT;
            crossways[1].Turning[C.TRUCK] = C.RIGHT;
            crossways[1].Turning[C.BUS] = C.UP;

            crossways[2].Turning[C.CAR] = C.UP;
            crossways[2].Turning[C.TRUCK] = C.LEFT;
            crossways[2].Turning[C.BUS] = C.RIGHT;

            crossways[3].Turning[C.CAR] = C.RIGHT;
            crossways[3].Turning[C.TRUCK] = C.UP;
            crossways[3].Turning[C.BUS] = C.LEFT;

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
                    vehicles.RemoveAt(i);                      
                }
                else i++;
            }

            //генерация новых
            int[] startingRoads = new int[8] { 0, 2, 3, 5, 6, 7, 10, 11 };
            int whereToCreate = RandomGen.Rand.Next(8);
            Road whereToCreateRoad = roads[startingRoads[whereToCreate]];
            double[] startPos = whereToCreateRoad.GetNullPosition();
            int vType = RandomGen.Rand.Next(3);
            Vehicle v;
            switch (vType)
            {
                case 0:
                    v = new Car(startPos[C.X], startPos[C.Y]);
                    break;
                case 1:
                    v = new Truck(startPos[C.X], startPos[C.Y]);
                    break;
                default:
                    v = new Bus(startPos[C.X], startPos[C.Y]);
                    break;
            }
            v.CurrentRoadorCrossway = whereToCreateRoad;
            v.Direction = whereToCreateRoad.GetDirectionToMove(startPos);
            if (FindVehicleInPoint(v.GetFrontPoint(), v) == null)
                vehicles.Add(v);
        }
        public static Vehicle FindVehicleInPoint(double[] pos, Vehicle current)//поиск машины в точке
        {
            Vehicle veh = null;
            bool check = false;
            int i = 0;
            while(i<vehicles.Count && !check)
            {
                veh = vehicles[i];
                if (veh != current)
                {
                    switch (veh.Direction)
                    {
                        case C.UP:
                            check = (InBounds(pos[C.X], veh.Position[C.X], veh.Position[C.X] + veh.Size[C.Y])) &&
                                (InBounds(pos[C.Y], veh.Position[C.Y], veh.Position[C.Y] - veh.Size[C.X]));
                            break;
                        case C.DOWN:
                            check = (InBounds(pos[C.X], veh.Position[C.X], veh.Position[C.X] - veh.Size[C.Y])) &&
                                 (InBounds(pos[C.Y], veh.Position[C.Y], veh.Position[C.Y] + veh.Size[C.X]));
                            break;
                        case C.LEFT:
                            check = (InBounds(pos[C.X], veh.Position[C.X], veh.Position[C.X] - veh.Size[C.X])) &&
                                     (InBounds(pos[C.Y], veh.Position[C.Y], veh.Position[C.Y] - veh.Size[C.Y]));
                            break;
                        case C.RIGHT:
                            check = (InBounds(pos[C.X], veh.Position[C.X], veh.Position[C.X] + veh.Size[C.X])) &&
                                       (InBounds(pos[C.Y], veh.Position[C.Y], veh.Position[C.Y] + veh.Size[C.Y]));
                            break;
                    }
                }
                i++;
            }
            if (check) return veh;
            else return null;
        }
        public static bool InBounds(double point, double first, double second)// point принадл [first, second]
        {
            return ((point >= first && point <= second) || (point >= second && point <= first));
        }
        public static void CheckFixLight(double x, double y)//кликнул ли по светофору
        {
            int i = 0;
            bool found = false;
            while(i<crossways.Count() && !found)
            {
                if(x<=crossways[i].Position[C.X] && x >= crossways[i].Position[C.X]- C.IHorGreenPic.Size.Width
                    && y <= crossways[i].Position[C.Y] && y >= crossways[i].Position[C.Y] - C.IHorGreenPic.Size.Height)
                {
                    if(crossways[i].LightMode == C.BROKENLIGHT)
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
            foreach(Vehicle veh in vehicles)//двигаем машины
            {
                veh.Move();
            }

            //Генерация и удаление машин
            stepsForGenerate++;
            if(stepsForGenerate>=C.CreateVehiclesInterval/C.UpdateInterval)
            {
                GenerateVehicles();
                /*
                if(C.CreateVehiclesInterval>500)
                    C.CreateVehiclesInterval = (int)(C.CreateVehiclesInterval*0.96);
                if (C.DayTime > 300)
                    C.DayTime = (int)(C.DayTime * 0.96);*/
                stepsForGenerate = 0;
            }

            //Смена часа
            stepsForChangeHour++;
            if (stepsForChangeHour >= C.DayTime / C.UpdateInterval)
            {
                hour++;

                if (hour == 24)
                {
                    hour = 0;
                    score += C.DailyScore;
                }
                stepsForChangeHour = 0;
            }
            if (score <= 0)
            {
                Program.mainForm.updateState.Stop();
                gameOver = true;
            }
        }
        public static void PlaySound(string path)//звук
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = path;
            player.Load();
            player.Play();
        }
        public static void Draw(Graphics g)//обновление экрана
        {
            g.Clear(Color.White);
            g.DrawImage(C.IFieldPic, 0, 0);
            for (int i = 0; i < C.RoadsNumber; i++)
            {
                g.DrawImage(roads[i].Picture, (float)roads[i].Position[C.X], (float)roads[i].Position[C.Y]);
            }
            for (int i = 0; i < C.CrosswaysNumber; i++)
            {
                g.DrawImage(C.ICrossPic, (float)crossways[i].Position[C.X], (float)crossways[i].Position[C.Y]);
                if (crossways[i].LightMode == C.HORGREEN)
                {
                    g.DrawImage(C.IHorGreenPic, (float)crossways[i].Position[C.X] - C.IHorGreenPic.Size.Width, (float)crossways[i].Position[C.Y] - C.IHorGreenPic.Size.Height);
                }
                else if (crossways[i].LightMode == C.VERTGREEN)
                {
                    g.DrawImage(C.IVertGreenPic, (float)crossways[i].Position[C.X] - C.IHorGreenPic.Size.Width, (float)crossways[i].Position[C.Y] - C.IHorGreenPic.Size.Height);
                }
                else if (crossways[i].LightMode == C.BROKENLIGHT)
                {
                    g.DrawImage(C.IBrokenLightPic, (float)crossways[i].Position[C.X] - C.IHorGreenPic.Size.Width, (float)crossways[i].Position[C.Y] - C.IHorGreenPic.Size.Height);
                }
                else
                {
                    g.DrawImage(C.IFixingLightPic, (float)crossways[i].Position[C.X] - C.IHorGreenPic.Size.Width, (float)crossways[i].Position[C.Y] - C.IHorGreenPic.Size.Height);
                }
            }
            foreach (Vehicle vehicle in vehicles)
            {
                g.FillRectangle(Brushes.Red, (float)vehicle.Position[C.X], (float)vehicle.Position[C.Y],5,5);
                g.TranslateTransform((float)vehicle.Position[C.X], (float)vehicle.Position[C.Y]);
                switch (vehicle.Direction)
                {
                    case C.UP:
                        g.RotateTransform(-90);
                        g.DrawImage(vehicle.Picture, 0, 0);
                        break;
                    case C.DOWN:
                        g.RotateTransform(90);
                        g.DrawImage(vehicle.Picture, 0, 0);
                        break;
                    case C.LEFT:
                        g.RotateTransform(180);
                        g.DrawImage(vehicle.Picture, 0, 0);
                        break;
                    case C.RIGHT:
                        g.DrawImage(vehicle.Picture, 0, 0);
                        break;
                }
                g.ResetTransform();
            }

            g.FillRectangle(Brushes.White, mainInfoStart, 5, C.mainInfoWidth, C.mainInfoHeight);
            g.DrawString("Время: "+hour+":00", smallFont, infoBrush, mainInfoStart+3, 6);
            g.DrawString("Счёт: " + score, smallFont, infoBrush, mainInfoStart + 3, 6+linestep);
            g.DrawString("Количество машин: " + vehicles.Count, smallFont, infoBrush, mainInfoStart + 3, 6+2*linestep);
            if (message != null)
                g.DrawString(message, smallFont, messageColor, mainInfoStart + 3, 6 + 3.5F * linestep);

            if(gameOver)
            {
                g.FillRectangle(Brushes.LightYellow, gameoverStartX, gameoverStartY, C.gameoverWidth, C.gameoverHeight);
                g.DrawString("Игра окончена!", bigFont, gameoverBrush, gameoverStartX + 3, gameoverStartY+ C.gameoverHeight/3);
            }
        }
    }
}
