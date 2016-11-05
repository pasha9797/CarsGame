using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsGame
{
    public class Evacuator:Vehicle
    {
        private Vehicle target;
        private bool targetReached;

        public Vehicle Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }
        protected override void PictureBroken()
        {
            picture = C.IEvacBrokenPic;
        }
        protected override int GetTurn()
        {
            return C.UP;
        }
        public override void Move()
        {       
            if (!Broken)
            {
                if (!target.IsVisible()) position[C.X] = Program.mainForm.width * 4;
                if (!DidWeCrash())
                {
                    position[C.Y] += C.VehicleStep*2;
                }
                else
                {
                    if (Field.FindVehicleInPoint(GetFrontPoint(), this).Direction == C.UP && !Field.FindVehicleInPoint(GetFrontPoint(), this).Broken)//лобовое столкновение
                        BreakVehicle(true);
                }
            }
            if (targetReached)//если везём машину
            {
                    target.Position[C.X] = position[C.X];
                    target.Position[C.Y] = position[C.Y] - target.Size[C.X] * 0.5;
                target.Direction = direction;
            }
            else
            {
                if (IsCloseToTarget()) //если достигли цели
                {
                    target.StopKill();
                    targetReached = true;
                }
            }
        }
        private bool IsCloseToTarget()
        {
            bool close=false;
            if (Math.Abs(position[C.Y] - target.Position[C.Y]) <= size[C.X])
                close = true;
            return close;
        }
        private void StartCoords()
        {
            position = new double[2];
            position[C.Y] = -size[C.X];
            if (target.Direction == C.RIGHT)
                position[C.X] = target.Position[C.X];
            else if (target.Direction == C.DOWN)
                position[C.X] = target.Position[C.X] - target.Size[C.Y];
            else if (target.Direction == C.LEFT)
                position[C.X] = target.Position[C.X] - target.Size[C.X];
            else
                position[C.X] = target.Position[C.X];
        }

        public Evacuator(Vehicle targ) : base(null)
        {
            picture = C.IEvacPic;
            this.size[C.X] = C.EvacSize.Width;
            this.size[C.Y] = C.EvacSize.Height;
            this.target = targ;
            StartCoords();
            targetReached = false;
        }
    }
}
