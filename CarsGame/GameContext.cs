using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;
using CarsGameLib;
using Evacuator;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace CarsGame
{
    public class GameContext : IGameContext
    {
        private Random rand;   
        private Size formSize;           
        private int score;
        private int hour;
        private System.Media.SoundPlayer player;
        private Dictionary<Type, string> pluginsDirs;
        private List<Vehicle> vehicles;
        private Crossway[] crossways;
        private AlertMethod alertMethod;
        private CallEvacMethod callEvacMethod;

        public Dictionary<Type, string> PluginsDirs
        {
            get
            {
                return pluginsDirs;
            }
        }
        public Random Rand
        {
            get
            {
                return rand;
            }
        }
        public int Score
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
        public Size FormSize
        {
            get
            {
                return formSize;
            }
        }
        public int Hour
        {
            get
            {
                return hour;
            }
            set
            {
                hour = value;
            }
        }

        public Vehicle FindVehicleInPoint(PointF pos, Vehicle current)//поиск машины в точке
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
                            check = (C.InBounds(pos.X, veh.Position.X, veh.Position.X + veh.Size.Height)) &&
                                (C.InBounds(pos.Y, veh.Position.Y, veh.Position.Y - veh.Size.Width));
                            break;
                        case Direction.DOWN:
                            check = (C.InBounds(pos.X, veh.Position.X, veh.Position.X - veh.Size.Height)) &&
                                 (C.InBounds(pos.Y, veh.Position.Y, veh.Position.Y + veh.Size.Width));
                            break;
                        case Direction.LEFT:
                            check = (C.InBounds(pos.X, veh.Position.X, veh.Position.X - veh.Size.Width)) &&
                                     (C.InBounds(pos.Y, veh.Position.Y, veh.Position.Y - veh.Size.Height));
                            break;
                        case Direction.RIGHT:
                            check = (C.InBounds(pos.X, veh.Position.X, veh.Position.X + veh.Size.Width)) &&
                                       (C.InBounds(pos.Y, veh.Position.Y, veh.Position.Y + veh.Size.Height));
                            break;
                    }
                }
                i++;
            }
            if (check) return veh;
            else return null;
        }   
        public void CallEvacuator(Vehicle target)//заспавнить эвакуатор
        {
            callEvacMethod(target);
        }
        public void Alert(string text, Brush color)//сообщение внизу
        {
            alertMethod(text, color);
        }
        public void PlaySound(string path)//звук
        {
            player.SoundLocation = path;
            player.Load();
            player.Play();
        }
        public void AddTurnRules(Type type, Turn[] turns)
        {
            for (int i = 0; i < turns.Length; i++)
            {
                crossways[i].Turning[type] = turns[i];
            }
        }
        
        public GameContext(Size formSize, List<Vehicle> vehicles, Crossway[] crossways, AlertMethod alertMethod, CallEvacMethod callEvacMethod)
        {
            this.formSize = formSize;
            this.vehicles = vehicles;
            this.crossways = crossways;
            this.alertMethod = alertMethod;
            this.callEvacMethod = callEvacMethod;
            rand = new Random();                  
            score = C.StartScore;
            hour = 0;
            player = new System.Media.SoundPlayer();
            pluginsDirs = new Dictionary<Type, string>();         
        }
    }
}
