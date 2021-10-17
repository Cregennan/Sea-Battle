using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sea_Battle
{
    class AI : ISolver
    {
        public GameField gField;

        private int MainState;

        private List<Point> shipPoints = new List<Point>();


        public AI(GameField field)
        {
            gField = field;
            MainState = GameEngine.AI.RandomSeekingState;
        }
        public (int, List<Point>) MakeStep()
        {
            Debug.WriteLine(MainState);
            Debug.WriteLine(shipPoints);

            if (MainState == GameEngine.AI.RandomSeekingState)
            {
                Random r = new Random();
                Point p = new Point(r.Next(0,10), r.Next(0,10));
                (int result, List<Point> points) = gField.PerformAttack(p);

                switch (result)
                {
                    case GameEngine.AttackResults.Missed:
                    case GameEngine.AttackResults.AlreadyHit:
                        shipPoints.Clear();
                        break;

                    case GameEngine.AttackResults.Hit:
                        MainState = GameEngine.AI.ShipPartFoundState;

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





                return (result, points);
            }





        }








    }
}
