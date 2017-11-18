using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsGameLib
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class PluginForLoadAttribute : Attribute
    {
        private bool forLoad = false;


        public PluginForLoadAttribute(bool forLoad)
        {
            this.forLoad = forLoad;
        }


        public bool ForLoad
        {
            get
            {
                return forLoad;
            }
        }
    }
}
