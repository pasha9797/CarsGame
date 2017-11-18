using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsGameLib;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Evacuator;

namespace CarsGame
{
    public class Model
    {
        GameContext context;
        private List<Type> vehTypes;
        private List<Vehicle> vehicles;
        private Crossway[] crossways;
        private List<Road> startingRoads;
        Queue<Evac> evacsToCreate;
        private Road[] roads;
        private int stepsForGenerate;
        private int stepsForChangeHour;
        private Font smallFont;
        private Font bigFont;
        private Brush infoBrush;
        private Brush gameoverBrush;
        private int linestep = 23;
        private float mainInfoStart;
        private float gameoverStartX;
        private float gameoverStartY;
        private bool gameOver;
        private Brush brush;
        private static readonly Type pluginType = typeof(Vehicle), attrType = typeof(PluginForLoadAttribute);
        private const string PluginsRelativeDir = "\\Plugins";
        private const string AssemblySearchPattern = "*.dll";
        private string message;
        private Brush messageColor;
        private System.Timers.Timer killtext;

        private void GenerateInfrastructure()//генерация поля и запуск игры
        {
            //горизонтальные дороги
            roads[0] = new HorRoad(0, C.IVertRoadPic.Size.Height, context);
            roads[1] = new HorRoad(C.IHorRoadPic.Size.Width + C.ICrossPic.Size.Width, C.IVertRoadPic.Size.Height, context);
            roads[2] = new HorRoad((C.IHorRoadPic.Size.Width + C.ICrossPic.Size.Width) * 2, C.IVertRoadPic.Size.Height, context);
            roads[3] = new HorRoad(0, C.IHorRoadPic.Size.Height + C.IVertRoadPic.Size.Height * 2, context);
            roads[4] = new HorRoad(C.IHorRoadPic.Size.Width + C.ICrossPic.Size.Width, C.IHorRoadPic.Size.Height + C.IVertRoadPic.Size.Height * 2, context);
            roads[5] = new HorRoad((C.IHorRoadPic.Size.Width + C.ICrossPic.Size.Width) * 2, C.IHorRoadPic.Size.Height + C.IVertRoadPic.Size.Height * 2, context);

            //вертикальные дороги
            roads[6] = new VertRoad(C.IHorRoadPic.Size.Width, 0, context);
            roads[7] = new VertRoad(C.IHorRoadPic.Size.Width * 2 + C.ICrossPic.Size.Width, 0, context);
            roads[8] = new VertRoad(C.IHorRoadPic.Size.Width, C.IVertRoadPic.Size.Height + C.ICrossPic.Size.Height, context);
            roads[9] = new VertRoad(C.IHorRoadPic.Size.Width * 2 + C.ICrossPic.Size.Width, C.IVertRoadPic.Size.Height + C.ICrossPic.Size.Height, context);
            roads[10] = new VertRoad(C.IHorRoadPic.Size.Width, (C.IVertRoadPic.Size.Height + C.ICrossPic.Size.Height) * 2, context);
            roads[11] = new VertRoad(C.IHorRoadPic.Size.Width * 2 + C.ICrossPic.Size.Width, (C.IVertRoadPic.Size.Height + C.ICrossPic.Size.Height) * 2, context);

            //перекрёстки
            crossways[0] = new Crossway(C.IHorRoadPic.Size.Width, C.IVertRoadPic.Size.Height, context);
            crossways[1] = new Crossway(C.IHorRoadPic.Size.Width * 2 + C.ICrossPic.Size.Width, C.IVertRoadPic.Size.Height, context);
            crossways[2] = new Crossway(C.IHorRoadPic.Size.Width, C.IVertRoadPic.Size.Height * 2 + C.ICrossPic.Size.Height, context);
            crossways[3] = new Crossway(C.IHorRoadPic.Size.Width * 2 + C.ICrossPic.Size.Width, C.IVertRoadPic.Size.Height * 2 + C.ICrossPic.Size.Height, context);

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
        }
        private void LoadPlugins()
        {
            string pluginsDir = Application.StartupPath + PluginsRelativeDir;
            string[] pluginDirs = Directory.GetDirectories(pluginsDir);

            foreach (string pluginDir in pluginDirs)
            {
                string[] assemblyFiles = Directory.GetFiles(pluginDir, AssemblySearchPattern);
                foreach (string assemblyFile in assemblyFiles)
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFile(assemblyFile);
                        // загрузка всех типов из сборки и поиск среди них плагинов
                        Type[] types = assembly.GetTypes();
                        foreach (Type type in types)
                        {
                            try
                            {
                                if (type.BaseType == pluginType)
                                {
                                    object[] attrs = type.GetCustomAttributes(attrType, false);
                                    if (attrs.Length != 0 && (attrs[0] as PluginForLoadAttribute).ForLoad)
                                    {
                                        vehTypes.Add(type);
                                        context.PluginsDirs.Add(type, pluginDir);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message, "Ошибка загрузки плагина " + type.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // не удалось правильно загрузить сборку
                        MessageBox.Show(e.ToString(), string.Format("Error load assembly \"{0}\"", assemblyFile));
                    }
                }
            }
            context.PluginsDirs.Add(typeof(Evac), "C:\\Users\\pasha\\Documents\\Visual Studio 2015\\Projects\\CarsGame\\Evacuator\\bin\\Debug");
        }
        public void ManageVehiclesSituation()//удаление старых и добавл новых машин
        {
            //удаление уехавших
            int i = 0;
            while (i < vehicles.Count)
            {
                if (!vehicles[i].IsVisible())
                {
                    if (vehicles[i] is Evac)
                        (vehicles[i] as Evac).Target.Hide();
                    else if (!vehicles[i].Broken)
                        context.Score += C.SuccessVehicleScore;
                    vehicles.RemoveAt(i);
                }
                else i++;
            }

            //генерация новых
            if (vehTypes.Count > 0)
            {
                int whereToCreate = context.Rand.Next(startingRoads.Count);
                Road road = startingRoads[whereToCreate];
                int vType = context.Rand.Next(vehTypes.Count);
                Vehicle v = CreateVehicle(vehTypes[vType], road);
                if (context.FindVehicleInPoint(v.GetFrontPoint(), v) == null)
                {
                    vehicles.Add(v);
                }
            }

            //эвакуаторы
            if (evacsToCreate.Count > 0)
            {
                if (context.FindVehicleInPoint(evacsToCreate.First().GetFrontPoint(), evacsToCreate.First()) == null)
                {
                    vehicles.Add(evacsToCreate.Dequeue());
                }
            }
        }
        private Vehicle CreateVehicle(Type vType, Road road)//Создать авто
        {
            object obj = Activator.CreateInstance(vType, new object[] { road, context });
            return obj as Vehicle;
        }
        public void CallEvacuator(Vehicle target)//заспавнить эвакуатор
        {
            Evac v = new Evac(target, context);
            v.Direction = Direction.DOWN;
            evacsToCreate.Enqueue(v);
        }
        public void MoveVehicles()
        {
            foreach (Vehicle veh in vehicles)//двигаем машины
            {
                veh.Move();
            }
        }
        public void CheckFixLight(float x, float y)//кликнул ли по светофору
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
                        context.PlaySound(C.FixSound);
                        context.Alert("Ремонтная бригада спешит на вызов!", Brushes.Green);
                        crossways[i].FixLightDelayed();//бригада едет какое-то время
                    }
                    else context.Alert("Ремонт не требуется!", Brushes.Red);
                    found = true;
                }
                i++;
            }
        }
        public void LightsInspection()//тех осмотр светофоров
        {
            if (context.Score < C.InspectionScore)
                context.Alert("Вы должны иметь " + C.InspectionScore + " очков!", Brushes.Red);
            else
            {
                for (int i = 0; i < crossways.Count(); i++)
                {
                    crossways[i].CrosswayChance = C.CrosswayChance;
                    if (crossways[i].LightMode == LightMode.BROKEN) crossways[i].FixLightDelayed();
                }
                context.Score -= C.InspectionScore;
                context.Alert("Тех. осмотр проведён!", Brushes.Green);
            }
        }
        public void MainUpdate()//основной таймер
        {
            MoveVehicles();

            //Генерация и удаление машин
            stepsForGenerate++;
            if (stepsForGenerate >= C.CreateVehiclesInterval / C.UpdateInterval)
            {
                ManageVehiclesSituation();
                stepsForGenerate = 0;
            }

            //Смена часа
            stepsForChangeHour++;
            if (stepsForChangeHour >= C.DayLength / C.UpdateInterval)
            {
                context.Hour++;
                stepsForChangeHour = 0;
            }
            //игра окончена
            if (context.Score<=0)
            {
                Program.mainForm.updateState.Stop();
                gameOver = true;
            }
        }
        public void Alert(string text, Brush color)//сообщение внизу
        {
            killtext.Stop();
            message = text;
            messageColor = color;
            killtext.Start();
        }
        private void KillText(Object source, EventArgs e)//убрать сообщение
        {
            message = null;
        }
        public void Draw(Graphics g)//обновление экрана
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

                /*g.DrawImage(C.ICrossInfoPic, (float)crossways[i].Position.X + C.ICrossPic.Size.Width, (float)crossways[i].Position.Y - C.ICrossInfoPic.Size.Height * 2);
                g.DrawImage(TurnImage(i, VehType.CAR), (float)crossways[i].Position.X + C.ICrossPic.Size.Width, (float)crossways[i].Position.Y - C.ICrossInfoPic.Size.Height);
                g.DrawImage(TurnImage(i, VehType.TRUCK), (float)crossways[i].Position.X + C.ICrossPic.Size.Width + (C.ICrossInfoPic.Size.Width - C.ITurnRightPic.Size.Width) / 2, (float)crossways[i].Position.Y - C.ICrossInfoPic.Size.Height);
                g.DrawImage(TurnImage(i, VehType.BUS), (float)crossways[i].Position.X + C.ICrossPic.Size.Width + (C.ICrossInfoPic.Size.Width - C.ITurnRightPic.Size.Width), (float)crossways[i].Position.Y - C.ICrossInfoPic.Size.Height);*/
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
            g.DrawString("Время: " + (context.Hour % 24) + ":00", smallFont, infoBrush, mainInfoStart + 3, 6);
            g.DrawString("Счёт: " + context.Score, smallFont, infoBrush, mainInfoStart + 3, 6 + linestep);
            g.DrawString("Количество машин: " + vehicles.Count, smallFont, infoBrush, mainInfoStart + 3, 6 + 2 * linestep);
            if (message != null)
                g.DrawString(message, smallFont, messageColor, mainInfoStart + 3, 6 + 3.5F * linestep);

            if (gameOver)
            {
                g.FillRectangle(Brushes.LightYellow, gameoverStartX, gameoverStartY, C.gameoverWidth, C.gameoverHeight);
                g.DrawString("Игра окончена!", bigFont, gameoverBrush, gameoverStartX + 2, gameoverStartY + C.gameoverHeight / 3);
                g.DrawString("Вы играли " + context.Hour + " часов.", smallFont, gameoverBrush, gameoverStartX + 2, gameoverStartY + C.gameoverHeight / 3 + 100);
            }
        }

        public Model(Size formSize)
        {
            roads = new Road[12];
            crossways = new Crossway[4];
            vehTypes = new List<Type>();
            startingRoads = new List<Road>();
            evacsToCreate = new Queue<Evac>();
            stepsForGenerate = 0;
            stepsForChangeHour = 0;
            smallFont = new Font("Tahoma", 12);
            bigFont = new Font("Tahoma", 48);
            infoBrush = Brushes.Blue;
            gameoverBrush = Brushes.Red;
            mainInfoStart = formSize.Width / 2 - (C.mainInfoWidth / 2);
            gameoverStartX = formSize.Width / 2 - (C.gameoverWidth / 2);
            gameoverStartY = formSize.Height / 2 - (C.gameoverHeight / 2);
            gameOver = false;
            brush = Brushes.Green;
            vehicles = new List<Vehicle>();
            killtext = new System.Timers.Timer(C.AlertTime);
            killtext.AutoReset = false;
            killtext.Elapsed += KillText;
            message = null;
            context = new GameContext(formSize, vehicles, crossways, Alert, CallEvacuator);
            GenerateInfrastructure();
            LoadPlugins();
        }
    }
}
