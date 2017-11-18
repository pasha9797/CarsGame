using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsGameLib
{
    public interface IGameContext
    {
        Random Rand { get; }
        Size FormSize { get; }
        int Score { get; set; }
        int Hour { get; }
        Dictionary<Type, string> PluginsDirs { get; }

        void CallEvacuator(Vehicle target);
        void PlaySound(string sound);
        void AddTurnRules(Type type, Turn[] turns);
        void Alert(string str, Brush color);
        Vehicle FindVehicleInPoint(PointF pos, Vehicle current);
    }
}
