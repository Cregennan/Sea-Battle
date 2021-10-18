using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
    public partial class GameWindow : Window
    {
        GameField EnemyField = new GameField();
        
        ISolver solver;
        void UpdateCellColor(Point p, GameField gField, Canvas field)
        {
            var pos = GameEngine.GetInlinePosition(p);
            switch (gField.GetFState(p))
                {
                    case GameEngine.FieldStates.Ship:
                        ((Rectangle)field.Children[pos]).Fill = GameEngine.Colors.FieldShip;
                        break;
                    case GameEngine.FieldStates.Unplacable:
                        ((Rectangle)field.Children[pos]).Fill = GameEngine.Colors.FieldUnplacable;
                        break;
                    case GameEngine.FieldStates.Dropped:
                        ((Rectangle)field.Children[pos]).Fill = GameEngine.Colors.FieldDropped;
                        break;
                    case GameEngine.FieldStates.Unknown:
                        ((Rectangle)field.Children[pos]).Fill = GameEngine.Colors.FieldUnknown;
                        break;
                    case GameEngine.FieldStates.TotalDestroyedShip:
                        ((Rectangle)field.Children[pos]).Fill = GameEngine.Colors.FieldTotalDestroyedShip;
                        break;
                    case GameEngine.FieldStates.DestroyedShipPiece:
                        ((Rectangle)field.Children[pos]).Fill = GameEngine.Colors.FieldDestroyedShipPiece;
                        break;
                    case GameEngine.FieldStates.NotPresents:
                        ((Rectangle)field.Children[pos]).Fill = GameEngine.Colors.FieldNotPresents;
                        break;
                    default:
                        break;
                }
        }

        void UpdateField(GameField gField, Canvas field)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var p = new Point(j, i);
                    UpdateCellColor(p, gField, field);
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
                rect.MouseDown += (object sender, MouseButtonEventArgs e) => Rect_MouseDown(sender, e, gField);
                Canvas.SetLeft(rect, (i % 10) * 30);
                Canvas.SetTop(rect, (i / 10) * 30);
                canv.Children.Add(rect);
            }
        }

        private void Rect_MouseDown(object sender, MouseButtonEventArgs e, GameField gField)
        {
            Canvas canv = (Canvas)((Rectangle)sender).Parent;
            Rectangle rect = (Rectangle)sender;
            int numberInField = canv.Children.IndexOf(rect);
            Point fieldPosition  = GameEngine.Get2DPosition(numberInField);

            (int AttackResult, List<Point> shipPoints) = gField.PerformAttack(fieldPosition);
            HandleFieldEvent(AttackResult, shipPoints, gField, canv);
            MessageBox.Show(GameEngine.AttackResults.Descriptor(AttackResult));
        }

        private void HandleFieldEvent(int AttackResult, List<Point> shipPoints, GameField gField, Canvas canv)
        {
            switch(AttackResult){
                case GameEngine.AttackResults.Missed:
                case GameEngine.AttackResults.Hit:
                case GameEngine.AttackResults.AlreadyHit:
                    foreach(var point in shipPoints)
                    {
                        UpdateCellColor(point, gField, canv);
                    }
                    break;
                case GameEngine.AttackResults.Killed:
                    foreach(var point in shipPoints)
                    {
                        UpdateCellColor(point, gField, canv);
                    }
                    var hood = gField.GetShipNeighbourhood(shipPoints[0]);
                    PlaceAssistCells(hood, canv, gField);
                    break;
                case GameEngine.AttackResults.NoMoreShips:
                    foreach(var point in shipPoints)
                    {
                        UpdateCellColor(point, gField,canv);
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


        public GameWindow()
        {
            InitializeComponent();
                
            InitFieldRects(field, EnemyField);
            EnemyField.MakeRandomShipPlacement();
           
            solver = AIFactory.Produce(EnemyField, GameEngine.AI.GameModeIntellectual);
            UpdateField(EnemyField, field);
            Solve();
        }
        public async void Solve()
        {
            while (true)
            {
                (int result, List<Point> points) = solver.MakeStep();
                HandleFieldEvent(result, points, EnemyField, field);
                if (result == GameEngine.AttackResults.NoMoreShips)
                {
                    break;
                }
                else
                {
                    await Task.Delay(300);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas canv = field;

            (int AttackResult, List<Point> shipPoints) = solver.MakeStep();

            HandleFieldEvent(AttackResult, shipPoints, EnemyField, canv);
            MessageBox.Show(this, GameEngine.AttackResults.Descriptor(AttackResult));
        }

    }
}
