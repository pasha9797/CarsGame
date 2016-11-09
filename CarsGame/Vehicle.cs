using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace CarsGame
{
    public abstract class Vehicle
    {
        protected Size size;
        protected PointF position;
        protected Direction direction;
        protected Image picture;
        protected int stepsCount;
        protected object curPlacement;
        protected Timer killcar;
        protected bool broken;

        public Size Size
        {
            get
            {
                return size;
            }
        }
        public PointF Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public Direction Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }
        public Image Picture
        {
            get
            {
                return picture;
            }
        }
        public object CurPlacement
        {
            get
            {
                return curPlacement;
            }
            set
            {
                if (value is Road || value is Crossway)
                {
                    curPlacement = value;
                }
            }
        }
        public bool Broken
        {
            get
            {
                return broken;
            }
            set
            {
                broken = value;
            }
        }

        public virtual void Move()//двигаем по таймеру в Field
        {
            if (!DidWeCrash())
            {
                if (!Broken)
                {
                    if (curPlacement is Road)
                    {
                        if (GetCrossway() != null && stepsCount >= StepsToPass()) //пора на перекресток
                        {
                            if (CanWeGo())
                            {
                                curPlacement = GetCrossway();
                                stepsCount = 0;
                                position = (curPlacement as Crossway).GetStartPosition(direction);
                            }
                        }
                        else
                        {
                            OneMove();
                        }
                    }
                    if (curPlacement is Crossway)
                    {
                        int stepsToPass = StepsToPass();
                        if (stepsCount < stepsToPass)
                        {
                            OneMove();
                        }
                        if (stepsCount == stepsToPass / 2 && GetTurn() != Turn.STRAIGHT)//поворот
                        {
                            SetTurnDir();
                            if (GetTurn() == Turn.RIGHT)
                                position = (curPlacement as Crossway).GetAfterMiddlePosition(direction);
                            else if (GetTurn() == Turn.LEFT)
                                position = (curPlacement as Crossway).GetBeforeMiddlePosition(direction);
                        }
                        if (stepsCount >= stepsToPass)//пора на дорогу
                        {
                            CurPlacement = (curPlacement as Crossway).ConnectedRoads[(int)direction];
                            stepsCount = 0;
                            position = GetResumeRoadPosition();
                        }
                    }
                }
            }
            else
            {
                if (curPlacement is Crossway)
                {
                    if ((curPlacement as Crossway).LightMode == LightMode.BROKEN ||
                        (curPlacement as Crossway).LightMode == LightMode.FIXING)
                        BreakVehicle(true);//врезались
                    else
                    {
                        Vehicle v = Field.FindVehicleInPoint(GetFrontPoint(), this);
                        if (Field.FindVehicleInPoint(v.GetFrontPoint(), v) == this)
                            BreakVehicle(true);//лобовое столкновение
                    }
                }
            }
        }
        public void BreakVehicle(bool BreakOpponent)//сделать аварию
        {
            if (!broken)
            {
                broken = true;
                PictureBroken();
                Field.CallEvacuator(this);
                if (BreakOpponent)
                {
                    Field.PlaySound(C.CrashSound);

                    Vehicle veh = Field.FindVehicleInPoint(GetFrontPoint(), this);
                    if (veh != null) veh.BreakVehicle(false);//ломаем того, в кого врезались               
                }
                Field.Score -= C.CrashScore;

                killcar = new Timer(C.KillVehicleDelay);
                killcar.AutoReset = false;
                killcar.Elapsed += HideByTimer;
                killcar.Start();
            }
        }
        public void StopKill()//остановить таймер на уничтожение машины
        {
            killcar.Stop();
        }
        public void HideByTimer(Object source, ElapsedEventArgs e)//убрать за границу экрана(машина удалится таймером в Field)
        {
            Hide();
        }
        public void Hide()//собственно убирание
        {
            position.X = Program.mainForm.Width * 4;
        }
        public bool IsVisible()//не скрыта ли машина за экраном
        {

                if (position.X + size.Width < 0 || position.X - size.Width > Program.mainForm.width || 
                position.Y + size.Width < 0 || position.Y - size.Width > Program.mainForm.height)
                return false;
                else return true;

        }
        protected bool DidWeCrash()//проверка на аварию
        {
            PointF frontPoint = GetFrontPoint();
            return Field.FindVehicleInPoint(frontPoint, this) != null;
        }
        public PointF GetFrontPoint()//координата середины переднего бампера
        {
            PointF frontPoint = new PointF();
            switch (direction)
            {
                case Direction.UP:
                    frontPoint.X = position.X + size.Height / 2;
                    frontPoint.Y = position.Y - size.Width;
                    break;
                case Direction.DOWN:
                    frontPoint.X = position.X - size.Height / 2;
                    frontPoint.Y = position.Y + size.Width;
                    break;
                case Direction.LEFT:
                    frontPoint.X = position.X - size.Width;
                    frontPoint.Y = position.Y - size.Height / 2;
                    break;
                case Direction.RIGHT:
                    frontPoint.X = position.X + size.Width;
                    frontPoint.Y = position.Y + size.Height / 2;
                    break;
            }
            return frontPoint;
        }
        private int StepsToPass()//сколько шагов для прохода перекрестка
        {
            if (curPlacement is Crossway)
            {
                switch (GetTurn())
                {
                    case Turn.LEFT:
                        return C.StepsToPassCrosswayLeft;
                    case Turn.STRAIGHT:
                        return C.StepsToPassCrosswayStraight;
                    case Turn.RIGHT:
                        return C.StepsToPassCrosswayRight;
                    default:
                        return 0;
                }
            }
            else
            {
                if (curPlacement is HorRoad)
                    return C.StepsToPassHorRoad;
                else return C.StepsToPassVertRoad;
            }
        }
        private void SetTurnDir()//повернуть машину куда нужно
        {
            AddTurn(ref direction, GetTurn());
        }
        protected void AddTurn(ref Direction direction, Turn turn)//получить конечное положение авто после поворота
        {
            switch(direction)
            {
                case Direction.RIGHT:
                    switch(turn)
                    {
                        case Turn.RIGHT:
                            direction = Direction.DOWN;
                            break;
                        case Turn.LEFT:
                            direction = Direction.UP;
                            break;
                    }
                    break;
                case Direction.DOWN:
                    switch (turn)
                    {
                        case Turn.RIGHT:
                            direction = Direction.LEFT;
                            break;
                        case Turn.LEFT:
                            direction = Direction.RIGHT;
                            break;
                    }
                    break;
                case Direction.LEFT:
                    switch (turn)
                    {
                        case Turn.RIGHT:
                            direction = Direction.UP;
                            break;
                        case Turn.LEFT:
                            direction = Direction.DOWN;
                            break;
                    }
                    break;
                case Direction.UP:
                    switch (turn)
                    {
                        case Turn.RIGHT:
                            direction = Direction.RIGHT;
                            break;
                        case Turn.LEFT:
                            direction = Direction.LEFT;
                            break;
                    }
                    break;
            }
        }
        protected Crossway GetCrossway()//на какой перекресток свернуть
        {
            Road currentRoad = curPlacement as Road;
            switch (direction)
            {
                case Direction.UP:
                case Direction.LEFT:
                    return currentRoad.StartCrossway;
                case Direction.DOWN:
                case Direction.RIGHT:
                    return currentRoad.EndCrossway;
            }
            return null;
        }
        protected PointF GetResumeRoadPosition()//для сьезда с перекрестка на дорогу
        {
            switch (direction)
            {
                case Direction.RIGHT:
                case Direction.DOWN:
                    return (CurPlacement as Road).GetResumePosition(0, direction);
                case Direction.LEFT:
                case Direction.UP:
                default:
                    return (CurPlacement as Road).GetResumePosition(1, direction);
            }
        }
        protected bool CanWeGo()//проверка сигналов светофора
        {
            LightMode mode = GetCrossway().LightMode;
            if (mode == LightMode.BROKEN || mode == LightMode.FIXING)
                return true;
            switch (direction)
            {
                case Direction.UP:
                case Direction.DOWN:
                    return GetCrossway().LightMode == LightMode.VERTGREEN;
                case Direction.LEFT:
                case Direction.RIGHT:
                    return GetCrossway().LightMode == LightMode.HORGREEN;
            }
            return false;
        }
        protected void OneMove()//один шаг машины
        {
            switch (direction)
            {
                case Direction.UP:

                    position.Y -= C.VehicleStep;
                    stepsCount++;
                    break;
                case Direction.DOWN:

                    position.Y += C.VehicleStep;
                    stepsCount++;
                    break;
                case Direction.LEFT:

                    position.X -= C.VehicleStep;
                    stepsCount++;
                    break;
                case Direction.RIGHT:

                    position.X += C.VehicleStep;
                    stepsCount++;
                    break;
            }
        }
        public void SetPos(float x, float y)//вручную установить координаты
        {
            position.X = x;
            position.Y = y;
        }
        protected abstract Turn GetTurn();//куда поворачивать на перекрестке
        protected abstract void PictureBroken();//замена текстурки

        public Vehicle(Road road)
        {
            if (road != null)
            {
                position = road.GetNullPosition();
                curPlacement = road;
                direction = road.GetDirectionToMove(position);
            }
            size = new Size();
            stepsCount = -(int)(C.VehicleSize.Width / C.VehicleStep);
            broken = false;
            this.size.Width = C.VehicleSize.Width;
            this.size.Height = C.VehicleSize.Height;
        }
    }
}
