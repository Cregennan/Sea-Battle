using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Sea_Battle
{
    public class GameField
    {
        public int[,] Field { get; private set; } = new int[10, 10];
        public int[] Sizes { get; private set; } = new int[4] {4, 3, 2, 1 };

        List<Point> ships = new List<Point>();



        public int ShipCount { get; private set; }

        public bool isPointInField(Point p)
        { 
            return !(p.X > 9 || p.X < 0 || p.Y > 9 || p.Y < 0);
        }

        public bool isShipPartFound(Point p)
        {
            switch (GetFState(p))
            {
                case GameEngine.FieldStates.Unknown://Неизвестно
                case GameEngine.FieldStates.Unplacable://Невозможно размещение
                case GameEngine.FieldStates.NotPresents://Нет корабля
                case GameEngine.FieldStates.Dropped:
                    return false;

                default:
                    return true;      
            }
        }
        public bool isEmptyFound(Point p)
        {
            switch (GetFState(p))
            {
                case GameEngine.FieldStates.Unknown://Неизвестно
                case GameEngine.FieldStates.Unplacable://Невозможно размещение
                case GameEngine.FieldStates.NotPresents://Нет корабля
                    return true;

                default:
                    return false;      
            }
        }

        public bool PlaceShip(int length, Point position, int direction)
        {
            List<Point> shipPoints = new List<Point>();

            List<Point> hoodPoints = new List<Point>();

            if (Sizes[length - 1] == 0)
            {
                return false;
            }
            Vector directionVector = GameEngine.DirectionVectors.GetDirectionVector(direction);

            Point currentPosition = Point.Add(position, new Vector(0, 0));

            for (int i = 0; i < length; i++)
            {
                if (IsShipPartPlacable(currentPosition))
                {

                    shipPoints.Add(currentPosition);
                    hoodPoints.AddRange(GetHood(currentPosition));
                    currentPosition = Point.Add(currentPosition, directionVector);
                }
                else
                {
                    return false;
                }
            }
            foreach (Point p in hoodPoints)
            {
                SetFState(p, GameEngine.FieldStates.Unplacable);
            }

            foreach (Point p in shipPoints)
            {
                SetFState(p,GameEngine.FieldStates.Ship);
            }

            Sizes[length - 1]--;
            ShipCount++;
            ships.Add(position);
            return true;
        }
        public bool TryPlaceShip(int length, Point position, int direction)
        {
            List<Point> shipPoints = new List<Point>();

            List<Point> hoodPoints = new List<Point>();

            Vector directionVector = GameEngine.DirectionVectors.GetDirectionVector(direction);

            Point currentPosition = Point.Add(position, new Vector(0, 0));
            if (Sizes[length - 1] == 0)
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                if (IsShipPartPlacable(currentPosition))
                {

                    shipPoints.Add(currentPosition);
                    hoodPoints.AddRange(GetHood(currentPosition));
                    currentPosition = Point.Add(currentPosition, directionVector);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void Clear()
        {
            Field = new int[10, 10];
            Sizes = new int[4] {4,3,2,1 };
            ships.Clear();
        }

        public void MakeRandomShipPlacement()
        {
            this.Clear();
            int[] sizes = new int[10] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
            Random r = new Random();

            for(int s = 0; s < 10; s++)
            {
                while(true)
                {
                    var d = r.Next(0, 2);
                    var p = new Point(r.Next(0, 10), r.Next(0, 10));
                    Debug.WriteLine("Attempt to place ship to " + p.ToString());
                    var result = this.PlaceShip(sizes[s], p, d);
                    if (result)
                    {
                        break;  
                    }
                }
            }
        }
        private bool IsShipPartPlacable(Point position)
        {
            if (position.X < 0 || position.X > 9 || position.Y < 0 || position.Y > 9)
            {
                return false;
            }
            if (Field[(int)position.X, (int)position.Y] != 0)
            {
                return false;
            }
            
            return true;
        }
        private List<Point> GetHood(Point position)
        {
            List<Vector> hood = new List<Vector> { new Vector(1, -1), new Vector(1, 0), new Vector(1, 1), new Vector(0, 1), new Vector(-1, 1), new Vector(-1, 0), new Vector(-1, -1), new Vector(0, -1) };
            List<Point> points = new List<Point>();

            for (int i = 0; i < hood.Count; i++)
            {
                Point p = Point.Add(position, hood[i]);
                if (!isPointInField(p))
                {
                    continue;
                }
                points.Add(p);
            }
            return points;
        }

        public int GetFState(Point p)
        {
            return Field[(int) p.X, (int) p.Y];
        }

        private void SetFState(Point p, int value)
        {
            this.Field[(int) p.X, (int) p.Y] = value;
        }
        public List<Point> GetAllShipParts(Point p)
        {
            List<Point> points = new List<Point>();
            Point t;

            if (!isShipPartFound(p))
            {
                return new List<Point>();
            }
            else
            {
                points.Add(p);
            }


            t = p;
            for (int i = 0; i < 4; i++)
            {
                t.Offset(1,0);
                if (isPointInField(t) && isShipPartFound(t))
                {
                    points.Add(t);
                }
                else
                {
                    break;
                }
            }
            t = p;
            for (int i = 0; i < 4; i++)
            {
                t.Offset(-1,0);
                if (isPointInField(t) && isShipPartFound(t))
                {
                    points.Add(t);
                }
                else
                {
                    break;
                }
            }
            t = p;
            for (int i = 0; i < 4; i++)
            {
                t.Offset(0,1);
                if (isPointInField(t) && isShipPartFound(t))
                {
                    points.Add(t);
                }
                else
                {
                    break;
                }
            }
            t = p;
            for (int i = 0; i < 4; i++)
            {
                t.Offset(0,-1);
                if (isPointInField(t) && isShipPartFound(t))
                {
                    points.Add(t);
                }
                else
                {
                    break;
                }
            }

            return points;
        }

        public bool RemoveShip(Point p)
        {
            var shipPoints = GetAllShipParts(p);
            int length = shipPoints.Count;
            if (length == 0)
            {
                return false;
            }
            Sizes[length - 1]++;




            var hood = GetShipNeighbourhood(p);


            foreach(var point in shipPoints)
            {
                SetFState(point, GameEngine.FieldStates.Unknown);
                ships.Remove(point);
            }
            foreach(var point in hood)
            {
                if (isEmptyFound(p))
                {
                    SetFState(point, GameEngine.FieldStates.Unknown);
                }
            }
            foreach(var ship in ships)
            {
                List<Point> current_hood = GetShipNeighbourhood(ship);
                foreach(var point in current_hood)
                {
                    SetFState(point, GameEngine.FieldStates.Unplacable);
                }
            }

            return true;
        }



        public (int, List<Point>) PerformAttack(Point p)
        {
            List<Point> points = new List<Point>();

            switch (GetFState(p))
            {
                case GameEngine.FieldStates.Unknown: //Неизвестно
                case GameEngine.FieldStates.Unplacable: //Невозможно размещение
                case GameEngine.FieldStates.NotPresents: //Нет корабля
                    SetFState(p, GameEngine.FieldStates.Dropped);
                    points.Add(p);
                    return (GameEngine.AttackResults.Missed, points);


                case GameEngine.FieldStates.DestroyedShipPiece: //Кусок уничтоженного корабля
                    return (GameEngine.AttackResults.Hit, new List<Point>(){ p });
                case GameEngine.FieldStates.TotalDestroyedShip: //Полностью уничтоженный корабль
                    return (GameEngine.AttackResults.Killed, new List<Point>(){ p });
                case GameEngine.FieldStates.Dropped:
                    points.Add(p);
                    return (GameEngine.AttackResults.AlreadyHit, points);

                case GameEngine.FieldStates.Ship:
                    SetFState(p, GameEngine.FieldStates.DestroyedShipPiece);
                    var shipPoints = GetAllShipParts(p);
                    bool shipIntegrity = false;
                    foreach (var shipPoint in shipPoints)
                    {
                        if (GetFState(shipPoint) == GameEngine.FieldStates.Ship)
                        {
                            shipIntegrity = true;
                            break;
                        }
                    }

                    if (!shipIntegrity)
                    {
                        foreach (var shipPoint in shipPoints)
                        {
                            SetFState(shipPoint, GameEngine.FieldStates.TotalDestroyedShip);
                        }

                        ShipCount--;
                        Debug.WriteLine(ShipCount);
                        if (ShipCount < 1)
                        {
                            return (GameEngine.AttackResults.NoMoreShips, shipPoints);
                        }
                        else
                        {
                            return (GameEngine.AttackResults.Killed, shipPoints);
                        }
                    }
                    else
                    {
                        return (GameEngine.AttackResults.Hit, new List<Point>() {p});
                    }

                default:
                    break;
            }

            return (GameEngine.AttackResults.Unexpected, null);   
        }

        public List<Point> GetShipNeighbourhood(Point p)
        {
            List<Point> hood = new List<Point>();
            var shipParts = GetAllShipParts(p);
            foreach(var part in shipParts)
            {
               hood.AddRange(GetHood(part));
            }
            hood = hood.Distinct().ToList();
            foreach(var part in shipParts)
            {
                hood.Remove(part);
            }

            return hood;
        }
    }
}
