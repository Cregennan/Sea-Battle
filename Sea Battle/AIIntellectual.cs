using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sea_Battle
{
    class AIIntellectual : ISolver
    {
        public GameField gField;

        private int MainState = GameEngine.AI.RandomSeekingState;

        private List<Point> shipPoints = new List<Point>();

        private bool IsRandomPointAvailable(Point p)
        {
            switch (gField.GetFState(p))
            {
                case GameEngine.FieldStates.Unknown:
                case GameEngine.FieldStates.Unplacable:
                case GameEngine.FieldStates.Ship:
                    return true;
                    default:
                    return false;
            }
        }




        public AIIntellectual(GameField field)
        {
            gField = field;
            MainState = GameEngine.AI.RandomSeekingState;
        }
        public (int, List<Point>) MakeStep()
        {
            Debug.WriteLine(GameEngine.AI.StateDescriptor(MainState));





            Debug.WriteLine(shipPoints.Count);
            Debug.WriteLine("");

            if (MainState == GameEngine.AI.RandomSeekingState)
            {
                Random r = new Random();
                Point p = new Point(r.Next(0,10), r.Next(0,10));

                while (true)
                {
                    if (IsRandomPointAvailable(p))
                    {
                        break;
                    }
                    else
                    {
                        p = new Point(r.Next(0,10), r.Next(0,10));
                    }
                }
                (int result, List<Point> points) = gField.PerformAttack(p);

                switch (result)
                {
                    case GameEngine.AttackResults.Missed:
                    case GameEngine.AttackResults.AlreadyHit:
                        //shipPoints.Clear();
                        break;

                    case GameEngine.AttackResults.Hit:
                        Debug.WriteLine("Hit Detected");
                        MainState = GameEngine.AI.ShipPartFoundState;
                        Debug.WriteLine(GameEngine.AI.StateDescriptor(MainState));
                        shipPoints.AddRange(gField.GetAllShipParts(p));
                        shipPoints.Remove(p);
                        
                        break;
                    case GameEngine.AttackResults.Killed:
                        MainState = GameEngine.AI.RandomSeekingState;
                        shipPoints.Clear();
                        break;
                    case GameEngine.AttackResults.Unexpected:
                    case GameEngine.AttackResults.NoMoreShips:
                        break;
                }
                return (result, points);
            }
            else
            {
                int pos = shipPoints.Count();
                Random r = new Random();

                Point p = shipPoints[r.Next(0, shipPoints.Count)];
                while (true)
                {
                    if (IsRandomPointAvailable(p))
                    {
                        break;
                    }
                    else
                    {
                        p = new Point(r.Next(0,10), r.Next(0,10));
                    }
                }
                (int result, List<Point> points) = gField.PerformAttack(p);


                switch (result)
                {
                    case GameEngine.AttackResults.Hit:
                        shipPoints.Remove(p);                        
                        break;
                    case GameEngine.AttackResults.Killed:
                        MainState = GameEngine.AI.RandomSeekingState;
                        shipPoints.Clear();
                        break;
                    case GameEngine.AttackResults.Unexpected:
                    case GameEngine.AttackResults.NoMoreShips:
                        break;
                }

                return (result, points);
            }





        }








    }
}
