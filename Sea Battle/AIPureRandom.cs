using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sea_Battle
{
    class AIPureRandom : ISolver
    {
        GameField field;

        public AIPureRandom(GameField gField)
        {
            this.field = gField;
        }

        public (int, List<Point>) MakeStep()
        {
            Random r = new Random();
            Point p = new Point(r.Next(0,10), r.Next(0,10));
            (int result, List<Point> points) = this.field.PerformAttack(p);
            return (result, points);
        }
    }
}
