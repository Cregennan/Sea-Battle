using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sea_Battle
{
    interface ISolver
    {
        (int, List<Point>) MakeStep();
    }
}
