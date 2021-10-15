using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sea_Battle
{
    public class Ship
    {
        /*
         * Определение направлений
         * 0 - C
         * 1 - В
         * 2 - Ю
         * 3 - З
         */
         
        public int length { get; private set; }

        public int direction {
            get;
            private set;
        }

        public Vector facing { get; private set; }

        public Point position { get; private set; }
        public Ship(int length, int direction, Point position)
        {
            switch (direction)
            {
                case GameEngine.Directions.Horizontal:
                    facing = GameEngine.DirectionVectors.Horizontal;
                    break;
                case GameEngine.Directions.Vertical:
                    facing = GameEngine.DirectionVectors.Vertical;
                    break;
            }
            this.length = length;
            this.direction = direction;
            this.position = position;
        }
    }
}
