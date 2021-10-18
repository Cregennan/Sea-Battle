using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sea_Battle
{
    class AIFactory
    {
        public static ISolver Produce(GameField gField, int gametype)
        {
            switch (gametype)
            {
                case GameEngine.AI.GameModeRandom:
                    return new AIPureRandom(gField);
                case GameEngine.AI.GameModeIntellectual:
                    return new AIIntellectual(gField);
                default:
                    return new AIPureRandom(gField);
            }
        }

    }
}
