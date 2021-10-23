using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sea_Battle
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class GameWindow
    {
        GameField EnemyField;
        GameField PlayerField;
        int GameMode;

        ISolver solver;
       
        bool EventHold = false;

        void IsGameEnded(int AttackResult, bool playerOffender = false)
        {
            if (AttackResult == GameEngine.AttackResults.NoMoreShips){
                if (playerOffender)
                {
                    PlayerWonWindow win = new PlayerWonWindow(true);
                    win.ShowDialog();
                    this.Close();
                }
                else
                {
                    PlayerWonWindow win = new PlayerWonWindow(false);
                    win.ShowDialog();
                    this.Close();
                }
            }
        }

        GameEngine.Colors.FieldColors GetColorPalette(Canvas canv)
        {
            if (canv == enemyFieldCanvas)
            {
                return new GameEngine.Colors.GameEnemyField();
            }
            else
            {
                return new GameEngine.Colors.GamePlayerField();
            }
        }


        void UpdateCellColor(Point p, GameField gField, Canvas field, GameEngine.Colors.FieldColors colors)
        {
            var pos = GameEngine.GetInlinePosition(p);
            switch (gField.GetFState(p))
                {
                    case GameEngine.FieldStates.Ship:
                        ((Rectangle)field.Children[pos]).Fill = colors.Ship;
                        break;
                    case GameEngine.FieldStates.Unplacable:
                        ((Rectangle)field.Children[pos]).Fill = colors.Unplacable;
                        break;
                    case GameEngine.FieldStates.Dropped:
                        ((Rectangle)field.Children[pos]).Fill = colors.Dropped;
                        break;
                    case GameEngine.FieldStates.Unknown:
                        ((Rectangle)field.Children[pos]).Fill = colors.Unknown;
                        break;
                    case GameEngine.FieldStates.TotalDestroyedShip:
                        ((Rectangle)field.Children[pos]).Fill = colors.TotalDestroyedShip;
                        break;
                    case GameEngine.FieldStates.DestroyedShipPiece:
                        ((Rectangle)field.Children[pos]).Fill = colors.DestroyedShipPiece;
                        break;
                    case GameEngine.FieldStates.NotPresents:
                        ((Rectangle)field.Children[pos]).Fill = colors.NotPresents;
                        break;
                    default:
                        break;
                }
        }

        void UpdateField(GameField gField, Canvas field)
        {
            GameEngine.Colors.FieldColors colors = GetColorPalette(field);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var p = new Point(j, i);
                    UpdateCellColor(p, gField, field, colors);
                }
            }
        }

        void InitFieldRects(Canvas canv, GameField gField)
        {
            for (int i = 0; i < 100; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = 30;
                rect.Height = 30;
                rect.Fill = GameEngine.Colors.GetColorFromRGBA("#FFF4F4F5");
                rect.Stroke = GameEngine.Colors.GetColorFromRGBA("#FF000000");
                rect.StrokeThickness = 1;
                if (canv == enemyFieldCanvas)
                {
                    rect.MouseDown += (object sender, MouseButtonEventArgs e) => Rect_MouseDown(sender, e, gField);
                }
                Canvas.SetLeft(rect, (i % 10) * 30);
                Canvas.SetTop(rect, (i / 10) * 30);
                canv.Children.Add(rect);
            }
        }

        private async void Rect_MouseDown(object sender, MouseButtonEventArgs e, GameField gField)
        {
            if (EventHold)
            {
                return;
            }
            Canvas canv = (Canvas)((Rectangle)sender).Parent;
            Rectangle rect = (Rectangle)sender;
            int numberInField = canv.Children.IndexOf(rect);
            Point fieldPosition  = GameEngine.Get2DPosition(numberInField);
            int state = gField.GetFState(fieldPosition);
            switch (state)
            {
                case GameEngine.FieldStates.DestroyedShipPiece:
                case GameEngine.FieldStates.Dropped:
                case GameEngine.FieldStates.TotalDestroyedShip:
                    return;
                default:
                    break;
            }




            (int AttackResult, List<Point> shipPoints) = gField.PerformAttack(fieldPosition);
            HandleFieldEvent(AttackResult, shipPoints, gField, canv);
            IsGameEnded(AttackResult, true);
                //FullscreenMessage.Show(GameEngine.AttackResults.Descriptor(AttackResult), "#FFFFFFFF", "#FF000000", 800, mainGrid);


            GameStatus.Text = GameEngine.Messages.GameAIStep;
            EventHold = true;
            await Task.Delay(GameEngine.AI.WaitTimeMillis);
            (int AIAttackResult, List<Point> AIpoints) = solver.MakeStep();
            HandleFieldEvent(AIAttackResult, AIpoints, PlayerField, playerFieldCanvas);
            IsGameEnded(AIAttackResult, false);
            GameStatus.Text = GameEngine.Messages.GamePlayerStep;
            EventHold = false;                                                 
            



            //MessageBox.Show();
        }

        private void HandleFieldEvent(int AttackResult, List<Point> shipPoints, GameField gField, Canvas canv)
        {
            GameEngine.Colors.FieldColors colors = GetColorPalette(canv);
            
            switch(AttackResult){
                case GameEngine.AttackResults.Missed:
                case GameEngine.AttackResults.Hit:
                case GameEngine.AttackResults.AlreadyHit:
                    foreach(var point in shipPoints)
                    {
                        UpdateCellColor(point, gField, canv, colors);
                    }
                    break;
                case GameEngine.AttackResults.Killed:
                    foreach(var point in shipPoints)
                    {
                        UpdateCellColor(point, gField, canv, colors);
                    }
                    var hood = gField.GetShipNeighbourhood(shipPoints[0]);
                    PlaceAssistCells(hood, canv, gField);
                    break;
                case GameEngine.AttackResults.NoMoreShips:
                    foreach(var point in shipPoints)
                    {
                        UpdateCellColor(point, gField,canv, colors);
                    }
                    break;
                default:
                    break;
            }
        }

        private void PlaceAssistCells(List<Point> points, Canvas canv, GameField gField){
            foreach(var point in points)
            {
                if (gField.isEmptyFound(point))
                {
                    var pos = GameEngine.GetInlinePosition(point);
                    ((Rectangle)canv.Children[pos]).Fill = GameEngine.Colors.FieldNotPresents;
                }
                
            }
        }


        public GameWindow(GameField playerField, int gameMode)
        {
            InitializeComponent();
            
            EnemyField = new GameField();
            EnemyField.MakeRandomShipPlacement();
            InitFieldRects(enemyFieldCanvas, EnemyField);
            UpdateField(EnemyField, enemyFieldCanvas);
            this.GameMode = gameMode;

            solver = AIFactory.Produce(playerField, GameMode);

            PlayerField = playerField;
            InitFieldRects(playerFieldCanvas, PlayerField);
            UpdateField(PlayerField, playerFieldCanvas);
            InitGame();
        }

        async void InitGame()
        {
            Random r = new Random();
            int t = r.Next(2);
            if (t == 1)
            {
                GameStatus.Text = GameEngine.Messages.GameAIStep;
                FullscreenMessage.Show(GameEngine.Messages.GameAIFirstStep, "#FFAAAAAA", "#FF000000", 800, mainGrid, easeTimeMillis: 400);
                solver.MakeStep();
                UpdateField(PlayerField, playerFieldCanvas);
                //await Task.Delay(GameEngine.AI.WaitTimeMillis);
                Thread.Sleep(GameEngine.AI.WaitTimeMillis);
                GameStatus.Text = GameEngine.Messages.GamePlayerStep;
            }
            else
            {
                GameStatus.Text = GameEngine.Messages.GamePlayerStep;
                FullscreenMessage.Show(GameEngine.Messages.GamePlayerFirstStep, "#FFAAAAAA", "#FF000000", 800, mainGrid, easeTimeMillis: 400);
            }
        }





        //public async void Solve()
        //{
        //    while (true)
        //    {
        //        (int result, List<Point> points) = solver.MakeStep();
        //        HandleFieldEvent(result, points, EnemyField, field);
        //        if (result == GameEngine.AttackResults.NoMoreShips)
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            await Task.Delay(300);
        //        }
        //    }
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Canvas canv = field;

        //    (int AttackResult, List<Point> shipPoints) = solver.MakeStep();

        //    HandleFieldEvent(AttackResult, shipPoints, EnemyField, canv);
        //    MessageBox.Show(this, GameEngine.AttackResults.Descriptor(AttackResult));
        //}

    }
}
