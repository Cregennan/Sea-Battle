﻿using System;
using System.Windows;
using System.Windows.Media;

namespace Sea_Battle
{
    public class GameEngine
    {
        
        public class Colors
        {
            public static SolidColorBrush GetColorFromRGBA(String hex)
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom(hex);
            }
            public static SolidColorBrush FieldEmpty = GetColorFromRGBA("#FF2222FF");
            public static SolidColorBrush FieldUnplacable = FieldEmpty;
            public static SolidColorBrush FieldUnknown = FieldEmpty;
            public static SolidColorBrush FieldNotPresents = GetColorFromRGBA("#FFAAAAAA");
            public static SolidColorBrush FieldDropped = GetColorFromRGBA("#FF444444");
            //public static SolidColorBrush FieldShip = GetColorFromRGBA("#FFFF9999");
            public static SolidColorBrush FieldShip = FieldEmpty;
            public static SolidColorBrush FieldDestroyedShipPiece = GetColorFromRGBA("#FFAA6666");
            public static SolidColorBrush FieldTotalDestroyedShip = GetColorFromRGBA("#FF994444");


        }





        public class DirectionVectors {
            public static readonly Vector Horizontal = new Vector(1, 0);
            public static readonly Vector Vertical = new Vector(0, 1);

            public static Vector GetDirectionVector(int direction)
            {
                switch (direction)
                {
                    case Directions.Horizontal:
                        return DirectionVectors.Horizontal;

                    case Directions.Vertical:
                        return DirectionVectors.Vertical;

                    default:
                        throw new NotImplementedException();

                        
                }
            }
        }
        public class Directions {
            public const int Horizontal = 0;
            public const int Vertical = 1;
        }
        public class FieldStates
        {
            public const int Unknown = 0;
            public const int Unplacable = 1;
            public const int NotPresents = 2;
            public const int Ship = 3;
            public const int DestroyedShipPiece = 4;
            public const int TotalDestroyedShip = 5;
            public const int Dropped = 6;
            
        }
        public static int GetInlinePosition(Point p)
        {
            return (int)p.X + (int)p.Y * 10;
        }

        public static Point Get2DPosition(int inlinePosition)
        {
            int y = inlinePosition / 10;
            return new Point(inlinePosition % 10, y);
        }

        public class AttackResults
        {
            public const int Missed = 0;
            public const int Hit = 1;
            public const int Killed = 2;
            public const int AlreadyHit = 3;
            public const int Unexpected = 4;
            public const int NoMoreShips = 5;
            public static String Descriptor(int AttackResult)
            {
                switch (AttackResult)
                {
                    case Missed:
                        return GameEngine.Messages.AttackResultMissed;
                    case Hit:
                        return GameEngine.Messages.AttackResultHit;
                    case Killed:
                        return GameEngine.Messages.AttackResultKilled;
                    case AlreadyHit:
                        return GameEngine.Messages.AttackResultAlreadyHit;
                    case Unexpected:
                        return GameEngine.Messages.AttackResultUnexpected;
                    case NoMoreShips:
                        return GameEngine.Messages.AttackResultNoMoreShips;
                    default:
                        return GameEngine.Messages.AttackResultUnexpected;
                }
            }
        }

        public class AI
        {
            public const int RandomSeekingState = 0;
            public const int ShipPartFoundState = 1;
            public const int GameModeIntellectual = 0;
            public const int GameModeRandom = 1;

            public static String StateDescriptor(int state)
            {
                switch(state)
                {
                    case RandomSeekingState:
                        return GameEngine.Messages.AIRandomSeekingState;
                    case ShipPartFoundState:
                        return GameEngine.Messages.AIShipPartFoundState;
                    default:
                        return GameEngine.Messages.EngineUnexpectedState;
                }
            }
            public static String GameModeDescriptor(int mode)
            {
                switch(mode)
                {
                    case GameModeRandom:
                        return GameEngine.Messages.AIGameModeRandom;
                    case GameModeIntellectual:
                        return GameEngine.Messages.AIGameModeIntellectual;
                    default:
                        return GameEngine.Messages.EngineUnexpectedState;
                }
            }

        }
        public class Messages
        {
            public const String AttackResultMissed = "Промазал";
            public const String AttackResultHit = "Есть попадание";
            public const String AttackResultKilled = "Есть пробитие";
            public const String AttackResultAlreadyHit = "Уже было попадание";
            public const String AttackResultUnexpected = "Неожиданное поведение движка";
            public const String AttackResultNoMoreShips = "Победа!";

            public const String AIRandomSeekingState = "Состояние случайного выбора";
            public const String AIShipPartFoundState = "Состояние cледования за частями корабля";
            public const String AIGameModeIntellectual = "Сложность: сложная";
            public const String AIGameModeRandom = "Сложность: легкая";

            public const String EngineUnexpectedState = "Ошибка движка. Ищите проблему в коде";

        }
        


    }
}
