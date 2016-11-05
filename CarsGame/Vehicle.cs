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
        protected double[] size;
        protected double[] position;
        protected int direction;
        protected Image picture;
        private int stepsCount;
        protected object curPlacement;
        protected Timer killcar;
        private bool broken;

        public double[] Size
        {
            get
            {
                return size;
            }
        }
        public double[] Position
        {
            get
            {
                return position;
            }
        }
        public int Direction
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
                        if (GetCrossway() != null && stepsCount >= StepsToPass())
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
                        if (stepsCount == stepsToPass / 2 && GetTurn() != C.UP)//поворот
                        {
                            SetTurnDir();
                            if (GetTurn() == C.RIGHT)
                                position = (curPlacement as Crossway).GetAfterMiddlePosition(direction);
                            else if (GetTurn() == C.LEFT)
                                position = (curPlacement as Crossway).GetBeforeMiddlePosition(direction);
                        }
                        if (stepsCount >= stepsToPass)//пора заехать
                        {
                            CurPlacement = (curPlacement as Crossway).GetTurnRoad(this);
                            stepsCount = 0;
                            position = GetResumeRoadPosition();
                        }
                    }
                }
            }
            else
            {
                if (curPlacement is Crossway && !(this is Evacuator))
                {
                    if ((curPlacement as Crossway).LightMode == C.BROKENLIGHT ||
                        (curPlacement as Crossway).LightMode == C.FIXINGLIGHT)
                        BreakVehicle(true);//врезались
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
                killcar.Elapsed += KillVehicle;
                killcar.Start();
            }
        }
        public void StopKill()
        {
            killcar.Stop();
        }
        public void KillVehicle(Object source, ElapsedEventArgs e)//убрать за границу экрана(машина удалится таймером в Field)
        {
            position[0] = Program.mainForm.Width * 4;
        }
        public bool IsVisible()//не скрыта ли машина за экраном
        {

                if (position[C.X] + size[C.X] < 0 || position[C.X] - size[C.X] > Program.mainForm.width || 
                position[C.Y] + size[C.X] < 0 || position[C.Y] - size[C.X] > Program.mainForm.height)
                return false;
                else return true;

        }
        protected bool DidWeCrash()//проверка на аварию
        {
            double[] frontPoint = GetFrontPoint();
            return Field.FindVehicleInPoint(frontPoint, this) != null;
        }
        public double[] GetFrontPoint()//координата середины переднего бампера
        {
            double[] frontPoint = new double[2];
            switch (direction)
            {
                case C.UP:
                    frontPoint[C.X] = position[C.X] + size[C.Y] / 2;
                    frontPoint[C.Y] = position[C.Y] - size[C.X];
                    break;
                case C.DOWN:
                    frontPoint[C.X] = position[C.X] - size[C.Y] / 2;
                    frontPoint[C.Y] = position[C.Y] + size[C.X];
                    break;
                case C.LEFT:
                    frontPoint[C.X] = position[C.X] - size[C.X];
                    frontPoint[C.Y] = position[C.Y] - size[C.Y] / 2;
                    break;
                case C.RIGHT:
                    frontPoint[C.X] = position[C.X] + size[C.X];
                    frontPoint[C.Y] = position[C.Y] + size[C.Y] / 2;
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
                    case C.LEFT:
                        return C.StepsToPassCrosswayLeft;
                    case C.UP:
                        return C.StepsToPassCrosswayStraight;
                    case C.RIGHT:
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
            AddTurn(ref direction, GetTurnUnit(GetTurn()));
        }
        protected int GetTurnUnit(int turnDir)// -1 если налево, 0 если прямо, 1 если направо
        {
            switch (turnDir)
            {
                case C.LEFT:
                    return -1;
                case C.UP:
                    return 0;
                case C.RIGHT:
                    return 1;
                default:
                    return 0;
            }
        }
        protected void AddTurn(ref int direction, int turn)//получить конечное положение авто после поворота
        {
            direction += turn;
            if (direction < 0) direction = 4 + direction;
            direction %= 4;
        }
        protected Crossway GetCrossway()
        {
            Road currentRoad = curPlacement as Road;
            switch (direction)
            {
                case C.UP:
                case C.LEFT:
                    return currentRoad.StartCrossway;
                case C.DOWN:
                case C.RIGHT:
                    return currentRoad.EndCrossway;
            }
            return null;
        }
        protected double[] GetResumeRoadPosition()
        {
            switch (direction)
            {
                case C.RIGHT:
                case C.DOWN:
                    return (CurPlacement as Road).GetResumePosition(0, direction);
                case C.LEFT:
                case C.UP:
                    return (CurPlacement as Road).GetResumePosition(1, direction);
            }
            return null;
        }
        protected bool CanWeGo()
        {
            int mode = GetCrossway().LightMode;
            if (mode == C.BROKENLIGHT || mode == C.FIXINGLIGHT)
                return true;
            switch (direction)
            {
                case C.UP:
                case C.DOWN:
                    return GetCrossway().LightMode == C.VERTGREEN;
                case C.LEFT:
                case C.RIGHT:
                    return GetCrossway().LightMode == C.HORGREEN;
            }
            return false;
        }
        protected void OneMove()
        {
            switch (direction)
            {
                case C.UP:

                    position[C.Y] -= C.VehicleStep;
                    stepsCount++;
                    break;
                case C.DOWN:

                    position[C.Y] += C.VehicleStep;
                    stepsCount++;
                    break;
                case C.LEFT:

                    position[C.X] -= C.VehicleStep;
                    stepsCount++;
                    break;
                case C.RIGHT:

                    position[C.X] += C.VehicleStep;
                    stepsCount++;
                    break;
            }
        }
        protected abstract int GetTurn();
        protected abstract void PictureBroken();

        public Vehicle(Road road)
        {
            if (road != null)
            {
                position = road.GetNullPosition();
                curPlacement = road;
                direction = road.GetDirectionToMove(position);
            }
            size = new double[2];
            stepsCount = -(int)(C.VehicleSize.Width / C.VehicleStep);
            broken = false;
            this.size[C.X] = C.VehicleSize.Width;
            this.size[C.Y] = C.VehicleSize.Height;
        }
    }
}
