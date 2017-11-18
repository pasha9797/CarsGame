using CarsGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evacuator
{
    public class Evac : Vehicle
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

        public override void Move()
        {
            if (target != null)
            {
                if (!Broken)
                {
                    if (!target.IsVisible())//если машинка удалилась то за ней ехать не надо
                        Hide();
                    if (!DidWeCrash())//движение
                    {
                        SetPos(Position.X, Position.Y + C.VehicleStep * 2);
                    }
                    else
                    {
                        if (Context.FindVehicleInPoint(GetFrontPoint(), this).Direction == Direction.UP && !Context.FindVehicleInPoint(GetFrontPoint(), this).Broken)//лобовое столкновение
                            BreakVehicle(true);
                    }
                }
                if (targetReached)//если везём машину
                {
                    target.SetPos(Position.X, Position.Y - target.Size.Width * 0.5F);
                    target.Direction = Direction;
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
            else
                throw new VehicleException("Evacuator has no target");
        }
        private bool IsCloseToTarget()//подъехал к цели или нет
        {
            if (target != null)
            {
                bool close = false;
                if (Math.Abs(Position.Y - target.Position.Y) <= Size.Width)
                    close = true;
                return close;
            }
            else
                throw new VehicleException("Evacuator has no target");
        }
        private void StartCoords()//координаты для начала движения
        {
            if (target != null)
            {
                float X;
                float Y = -Size.Width;
                if (target.Direction == Direction.RIGHT)
                    X = target.Position.X;
                else if (target.Direction == Direction.DOWN)
                    X = target.Position.X - target.Size.Height;
                else if (target.Direction == Direction.LEFT)
                    X = target.Position.X - target.Size.Width;
                else
                    X = target.Position.X;
                Position = new System.Drawing.PointF(X, Y);
            }
            else
                throw new VehicleException("Evacuator has no target");
        }

        public Evac(Vehicle targ, IGameContext gc) : base(null, gc)
        {
            PicPath = "EvacPic.png";
            target = targ;
            targetReached = false;
            BrokenPicPath = "EvacPicBroken.png";
            StartCoords();
            Turns = new Turn[]
            {
                Turn.STRAIGHT,
                Turn.STRAIGHT,
                Turn.STRAIGHT,
                Turn.STRAIGHT
            };
        }
    }
}
