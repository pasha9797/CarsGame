using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CarsGameLib
{
    public abstract class Vehicle
    {
        private PointF position;
        private Direction direction;
        private string picPath, brokenPicPath;
        private Image picture;
        private int stepsCount;
        private object curPlacement;
        private Timer killcar;
        private bool broken;
        private IGameContext context;
        private Turn[] turns;

        public IGameContext Context
        {
            get
            {
                return context;
            }
        }
        public Size Size
        {
            get
            {
                return picture.Size;
            }
        }
        public string PicPath
        {
            get
            {
                return picPath;
            }
            set
            {
                picPath = value;
                picture = Image.FromFile(Context.PluginsDirs[this.GetType()]+"\\"+picPath);
            }
        }
        public string BrokenPicPath
        {
            get
            {
                return brokenPicPath;
            }
            set
            {
                brokenPicPath = value;
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
        public Turn[] Turns
        {
            get
            {
                return turns;
            }
            set
            {
                turns = value;
                Context.AddTurnRules(this.GetType(), turns);
            }
        }

        public virtual void Move()//двигаем по таймеру в Field
        {
            if (!Broken)
            {
                if (!DidWeCrash())
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
                else //препятствие на пути
                {
                    if (curPlacement is Crossway)
                    {
                        if ((curPlacement as Crossway).LightMode == LightMode.BROKEN ||
                            (curPlacement as Crossway).LightMode == LightMode.FIXING)
                            BreakVehicle(true);//врезались
                        else
                        {
                            Vehicle v = Context.FindVehicleInPoint(GetFrontPoint(), this);
                            if (Context.FindVehicleInPoint(v.GetFrontPoint(), v) == this)
                                BreakVehicle(true);//лобовое столкновение
                        }
                    }
                }
            }
        }
        public void BreakVehicle(bool BreakOpponent)//сделать аварию
        {
            if (!broken)
            {
                broken = true;
                SetBrokenPicture();
                Context.CallEvacuator(this);
                if (BreakOpponent)
                {
                    Context.PlaySound(C.CrashSound);

                    Vehicle veh = Context.FindVehicleInPoint(GetFrontPoint(), this);
                    if (veh != null && !veh.Broken) veh.BreakVehicle(false);//ломаем того, в кого врезались               
                }
                Context.Score-=C.CrashScore;

                killcar = new Timer(C.KillVehicleDelay);
                killcar.AutoReset = false;
                killcar.Elapsed += HideByTimer;
                killcar.Start();
            }
            else
                throw new VehicleException("Trying to break a car wich is already broken");
        }
        public void StopKill()//остановить таймер на уничтожение машины
        {
            if (killcar != null)
                killcar.Stop();
            else
                throw new VehicleException("Trying to stop killing the car wich is not supposed to be killed");
        }
        public void HideByTimer(Object source, ElapsedEventArgs e)//убрать за границу экрана(машина удалится таймером в Field)
        {
            Hide();
        }
        public void Hide()//собственно убирание
        {
            position.X = Context.FormSize.Width * 4;
        }
        public bool IsVisible()//не скрыта ли машина за экраном
        {

            if (position.X + Size.Width < 0 || position.X - Size.Width > Context.FormSize.Width ||
            position.Y + Size.Width < 0 || position.Y - Size.Width > Context.FormSize.Height)
                return false;
            else return true;

        }
        protected bool DidWeCrash()//проверка на аварию
        {
            PointF frontPoint = GetFrontPoint();
            return Context.FindVehicleInPoint(frontPoint, this) != null;
        }
        public PointF GetFrontPoint()//координата середины переднего бампера
        {
            PointF frontPoint = new PointF();
            switch (direction)
            {
                case Direction.UP:
                    frontPoint.X = position.X + Size.Height / 2;
                    frontPoint.Y = position.Y - Size.Width;
                    break;
                case Direction.DOWN:
                    frontPoint.X = position.X - Size.Height / 2;
                    frontPoint.Y = position.Y + Size.Width;
                    break;
                case Direction.LEFT:
                    frontPoint.X = position.X - Size.Width;
                    frontPoint.Y = position.Y - Size.Height / 2;
                    break;
                case Direction.RIGHT:
                    frontPoint.X = position.X + Size.Width;
                    frontPoint.Y = position.Y + Size.Height / 2;
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
            else if (curPlacement is Road)
            {
                if (curPlacement is HorRoad)
                    return C.StepsToPassHorRoad;
                else return C.StepsToPassVertRoad;
            }
            else
                throw new VehicleException("Wrong vehicle placement (Neither crossway nor road");
        }
        private void SetTurnDir()//повернуть машину куда нужно
        {
            AddTurn(ref direction, GetTurn());
        }
        protected void AddTurn(ref Direction direction, Turn turn)//получить конечное положение авто после поворота
        {
            switch (direction)
            {
                case Direction.RIGHT:
                    switch (turn)
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
            if (currentRoad != null)
            {
                switch (direction)
                {
                    case Direction.UP:
                    case Direction.LEFT:
                        return currentRoad.StartCrossway;
                    case Direction.DOWN:
                    case Direction.RIGHT:
                        return currentRoad.EndCrossway;
                    default:
                        return null;
                }
            }
            else throw new VehicleException("Vehicle is expected to be on road but it is not");
        }
        protected PointF GetResumeRoadPosition()//для сьезда с перекрестка на дорогу
        {
            if (curPlacement is Road)
            {
                switch (direction)
                {
                    case Direction.RIGHT:
                    case Direction.DOWN:
                        return (CurPlacement as Road).GetResumePosition(0, direction);
                    case Direction.LEFT:
                    case Direction.UP:
                        return (CurPlacement as Road).GetResumePosition(1, direction);
                    default:
                        return new PointF(0, 0);
                }
            }
            else
                throw new VehicleException("Vehicle is expected to be on road but it is not");

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
                default:
                    return false;
            }

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
        protected Turn GetTurn()//куда поворачивать на перекрестке
        {
            if (curPlacement is Crossway)
                return (curPlacement as Crossway).Turning[this.GetType()];
            else
                throw new VehicleException("Vehicle is expected to be on crossway but it is not");
        }
        protected void SetBrokenPicture()//замена текстурки
        {
            picture = Image.FromFile(Context.PluginsDirs[this.GetType()] + "\\"+brokenPicPath);
        }

        public Vehicle(Road road, IGameContext gc)
        {
            if (road != null)
            {
                position = road.GetNullPosition();
                curPlacement = road;
                direction = road.GetDirectionToMove(position);
            }
            turns = new Turn[4];
            stepsCount = -(int)(C.VehicleSize.Width / C.VehicleStep);
            broken = false;
            context = gc;
        }
    }
}
