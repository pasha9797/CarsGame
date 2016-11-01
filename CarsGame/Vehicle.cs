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
        private int currentState;
        protected object currentRoadorCrossway;
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
        public object CurrentRoadorCrossway
        {
            get
            {
                return currentRoadorCrossway;
            }
            set
            {
                if (value is Road || value is Crossway)
                {
                    currentRoadorCrossway = value;
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

        public void Move()//двигаем по таймеру в Field
        {
            if (!DidWeCrash())
            {
                if (!Broken)
                {
                    if (currentState == C.ONROAD)
                    {
                        ProceedRoadMove();
                    }
                    if (currentState == C.ONCROSSWAY)
                    {
                        ProceedCrosswayMove();
                    }
                }
            }
            else
            {
                if (currentState == C.ONCROSSWAY)
                {
                    if ((currentRoadorCrossway as Crossway).LightMode == C.BROKENLIGHT ||
                        (currentRoadorCrossway as Crossway).LightMode == C.FIXINGLIGHT)
                        BreakVehicle(true);//врезались
                }
            }
        }
        private void ProceedRoadMove()//двигаем на дороге
        {
            Road currentRoad = currentRoadorCrossway as Road;
            switch (direction)
            {
                case C.UP:

                    if (currentRoad.StartCrossway != null && stepsCount >= C.StepsToPassVertRoad)
                    {
                        if (currentRoad.StartCrossway.LightMode != C.HORGREEN)
                        {
                            currentRoadorCrossway = currentRoad.StartCrossway;
                            stepsCount = 0;
                            currentState = C.ONCROSSWAY;
                        }
                    }
                    else
                    {
                        position[C.Y] -= C.VehicleStep;
                        stepsCount++;
                    }
                    break;
                case C.DOWN:

                    if (currentRoad.EndCrossway != null && stepsCount >= C.StepsToPassVertRoad)
                    {
                        if (currentRoad.EndCrossway.LightMode != C.HORGREEN)
                        {
                            currentRoadorCrossway = currentRoad.EndCrossway;
                            stepsCount = 0;
                            currentState = C.ONCROSSWAY;
                        }
                    }
                    else
                    {
                        position[C.Y] += C.VehicleStep;
                        stepsCount++;
                    }
                    break;
                case C.LEFT:
                    if (currentRoad.StartCrossway != null && stepsCount >= C.StepsToPassHorRoad)
                    {
                        if (currentRoad.StartCrossway.LightMode != C.VERTGREEN)
                        {
                            currentRoadorCrossway = currentRoad.StartCrossway;
                            stepsCount = 0;
                            currentState = C.ONCROSSWAY;
                        }
                    }
                    else
                    {
                        position[C.X] -= C.VehicleStep;
                        stepsCount++;
                    }
                    break;
                case C.RIGHT:

                    if (currentRoad.EndCrossway != null && stepsCount >= C.StepsToPassHorRoad)
                    {
                        if (currentRoad.EndCrossway.LightMode != C.VERTGREEN)
                        {
                            currentRoadorCrossway = currentRoad.EndCrossway;
                            stepsCount = 0;
                            currentState = C.ONCROSSWAY;
                        }
                    }
                    else
                    {
                        position[C.X] += C.VehicleStep;
                        stepsCount++;
                    }
                    break;
            }

        }
        private void ProceedCrosswayMove()//двигаем на перекрестке
        {
            Crossway currentCrossway = currentRoadorCrossway as Crossway;
            int stepsToPass = StepsToPass();
            switch (direction)
            {
                case C.UP:
                    if (stepsCount < stepsToPass)
                    {
                        position[C.Y] -= C.VehicleStep;
                        stepsCount++;
                    }
                    break;
                case C.DOWN:
                    if (stepsCount < stepsToPass)
                    {
                        position[C.Y] += C.VehicleStep;
                        stepsCount++;
                    }
                    break;
                case C.LEFT:
                    if (stepsCount < stepsToPass)
                    {
                        position[C.X] -= C.VehicleStep;
                        stepsCount++;
                    }
                    break;
                case C.RIGHT:
                    if (stepsCount < stepsToPass)
                    {
                        position[C.X] += C.VehicleStep;
                        stepsCount++;
                    }
                    break;
            }
            if (stepsCount == C.StepsToPassCrosswayLeftHalf && GetTurn() == C.LEFT)//левый поворот сложный, он проходит в 2 этапа
            {
                SetTurnDir();
                SetLeftTurnPosition();
            }
            if (stepsCount >= stepsToPass)//пора свернуть на дорогу
            {
                if(GetTurn() != C.LEFT)
                    SetTurnDir();
                CurrentRoadorCrossway = currentCrossway.GetTurnRoad(this);
                currentState = C.ONROAD;
                stepsCount = 0;
                switch (direction)
                {
                    case C.RIGHT:
                    case C.DOWN:
                        position = (CurrentRoadorCrossway as Road).GetStartPosition();
                        break;
                    case C.LEFT:
                    case C.UP:
                        position = (CurrentRoadorCrossway as Road).GetEndPosition();
                        break;
                }
                ProceedRoadMove();
            }
        }
        private void SetLeftTurnPosition()//момент когда авто развернется влево
        {
            switch(direction)
            {
                case C.UP:
                    position[C.X] += size[C.X] * 0.325;
                    position[C.Y] += size[C.X] * 0.4;
                    break;
                case C.DOWN:
                    position[C.X] -= size[C.X] * 0.325;
                    position[C.Y] -= size[C.X] * 0.4;
                    break;
                case C.RIGHT:
                    position[C.Y] += size[C.X]*0.325;
                    position[C.X] -= size[C.X] * 0.4;
                    break;
                case C.LEFT:
                    position[C.Y] -= size[C.X] * 0.325;
                    position[C.X] += size[C.X] * 0.4;
                    break;
            }
        }
        public void BreakVehicle(bool BreakOpponent)//сделать аварию
        {
            if (!broken)
            {
                broken = true;
                PictureBroken();
                if (BreakOpponent)
                {
                    Field.PlaySound(C.CrashSound);
                    Vehicle veh = Field.FindVehicleInPoint(GetFrontPoint(), this);
                    if (veh != null) veh.BreakVehicle(false);//ломаем того, в кого врезались               
                }
                Field.Score -= C.CrashScore;
                Timer killcar = new Timer(C.KillVehicleDelay);
                killcar.AutoReset = false;
                killcar.Elapsed += KillVehicle;
                killcar.Start();
            }
        }
        private void KillVehicle(Object source, ElapsedEventArgs e)//убрать за границу экрана(машина удалится таймером в Field)
        {
            position[0] = Program.mainForm.Width * 4;
        }
        public bool IsVisible()//не скрыта ли машина за экраном
        {

            try
            {
                if (position[C.X] + size[C.X] < 0 || position[C.X] - size[C.X] > Program.mainForm.width) return false;
                else if (position[C.Y] + size[C.X] < 0 || position[C.Y] - size[C.X] > Program.mainForm.height) return false;
                else return true;
            }
            catch
            {
                return true;
            }
        }
        private bool DidWeCrash()//проверка на аварию
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
        protected abstract int GetTurn();
        protected abstract void PictureBroken();

        public Vehicle(double x, double y)
        {
            position = new double[2] { x, y };
            size = new double[2];
            stepsCount = 0;
            currentState = C.ONROAD;
            broken = false;
        }
    }
}
