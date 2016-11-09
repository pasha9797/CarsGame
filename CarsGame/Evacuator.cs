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
        protected override Turn GetTurn()
        {
            return Turn.STRAIGHT;
        }
        public override void Move()
        {       
            if (!Broken)
            {
                if (!target.IsVisible())//если машинка удалилась то за ней ехать не надо
                    Hide();
                if (!DidWeCrash())//движение
                {
                    position.Y += C.VehicleStep*2;
                }
                else
                {
                    if (Field.FindVehicleInPoint(GetFrontPoint(), this).Direction == Direction.UP && !Field.FindVehicleInPoint(GetFrontPoint(), this).Broken)//лобовое столкновение
                        BreakVehicle(true);
                }
            }
            if (targetReached)//если везём машину
            {
                target.SetPos(position.X, position.Y-target.Size.Width * 0.5F);
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
        private bool IsCloseToTarget()//подъехал к цели или нет
        {
            bool close=false;
            if (Math.Abs(position.Y - target.Position.Y) <= size.Width)
                close = true;
            return close;
        }
        private void StartCoords()//координаты для начала движения
        {
            position = new System.Drawing.PointF();
            position.Y = -size.Width;
            if (target.Direction == Direction.RIGHT)
                position.X = target.Position.X;
            else if (target.Direction == Direction.DOWN)
                position.X = target.Position.X - target.Size.Height;
            else if (target.Direction == Direction.LEFT)
                position.X = target.Position.X - target.Size.Width;
            else
                position.X = target.Position.X;
        }

        public Evacuator(Vehicle targ) : base(null)
        {
            picture = C.IEvacPic;
            this.size.Width = C.EvacSize.Width;
            this.size.Height = C.EvacSize.Height;
            this.target = targ;
            StartCoords();
            targetReached = false;
        }
    }
}
