using SourceChord.FluentWPF;
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
using System.Windows.Shapes;

namespace Sea_Battle
{
    /// <summary>
    /// Interaction logic for ManualPlaceShipsWindow.xaml
    /// </summary>
    public partial class ManualPlaceShipsWindow
    {   
        GameField gField = new GameField();
        
        int[] ShipCounts = new int[4]{ 4,3,2,1 };

        int GameMode;

        int currentDirection = 0;

        List<Rectangle> ShipProviders;



        public ManualPlaceShipsWindow(int gm)
        {
            this.GameMode = gm;
            InitializeComponent();

            ShipProviders = new List<Rectangle>(){ FourShipProvider , ThreeShipProvider, TwoShipProvider, OneShipProvider};
            PlaceProvidersHorizontally();


            InitFieldRects(field, gField);
            UpdateField(gField, field);


        }

        void UpdateShipProviderColors()
        {
            for(int i = 0; i<4; i++)
            {
                if (gField.Sizes[3 - i] == 0)
                {
                    ShipProviders[i].Fill = GameEngine.Colors.EditorShipProviderUnavailable;
                    ShipProviders[i].Stroke = GameEngine.Colors.EditorShipProviderUnavailable;
                }
                else
                {
                    ShipProviders[i].Fill = GameEngine.Colors.EditorShipProviderAvailable;
                    ShipProviders[i].Stroke = GameEngine.Colors.EditorShipProviderAvailable;
                }
            }
        }

        void PlaceProvidersHorizontally()
        {
            for(int i = 0; i<4; i++)
            {
                ShipProviders[i].Height = 30;
                ShipProviders[i].Width = 150 - (i + 1) * 30;
                Canvas.SetLeft(ShipProviders[i], GameEngine.Editor.LeftMargin);
                int topMargin = GameEngine.Editor.TopMargin + (30 + GameEngine.Editor.TopMargin) * i;
                Canvas.SetTop(ShipProviders[i], topMargin);
            }
        }

        void PlaceProvidersVertically()
        {
            for(int i = 0; i<4; i++)
            {
                ShipProviders[i].Width = 30;
                ShipProviders[i].Height = 150 - (i + 1) * 30;
                Canvas.SetTop(ShipProviders[i], GameEngine.Editor.TopMargin);
                int leftMargin = GameEngine.Editor.LeftMargin + (30 + GameEngine.Editor.LeftMargin) * i;
                Canvas.SetLeft(ShipProviders[i], leftMargin);

            }
        }



        void ToggleShipPlacementDirection()
        {
            if (currentDirection == 0)
            {
                currentDirection = 1;
                PlaceProvidersVertically();
            }
            else
            {

                currentDirection = 0;
                PlaceProvidersHorizontally();
            }
        }


        void UpdateCellColor(Point p, GameField gField, Canvas field)
        {
            var pos = GameEngine.GetInlinePosition(p);
            switch (gField.GetFState(p))
                {
                    case GameEngine.FieldStates.Ship:
                        ((Button)field.Children[pos]).Background = GameEngine.Colors.FieldShip;
                        break;
                    case GameEngine.FieldStates.Unplacable:
                        ((Button)field.Children[pos]).Background = GameEngine.Colors.EditorFieldUnplacable;
                        break;
                    case GameEngine.FieldStates.Dropped:
                        ((Button)field.Children[pos]).Background = GameEngine.Colors.FieldDropped;
                        break;
                    case GameEngine.FieldStates.Unknown:
                        ((Button)field.Children[pos]).Background = GameEngine.Colors.FieldUnknown;
                        break;
                    case GameEngine.FieldStates.TotalDestroyedShip:
                        ((Button)field.Children[pos]).Background = GameEngine.Colors.FieldTotalDestroyedShip;
                        break;
                    case GameEngine.FieldStates.DestroyedShipPiece:
                        ((Button)field.Children[pos]).Background = GameEngine.Colors.FieldDestroyedShipPiece;
                        break;
                    case GameEngine.FieldStates.NotPresents:
                        ((Button)field.Children[pos]).Background = GameEngine.Colors.FieldNotPresents;
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
            UpdateShipProviderColors();
        }
        void InitFieldRects(Canvas canv, GameField gField)
        {
            for (int i = 0; i < 100; i++)
            {
                Button btn = new Button();
                btn.Width = 30;
                btn.Height = 30;
                btn.Background = GameEngine.Colors.GetColorFromRGBA("#FFF4F4F5");
                btn.AllowDrop = true;
                btn.DragEnter +=  (object sender, DragEventArgs e) => Btn_DragEnter(sender, e, gField, field);
                btn.DragLeave += (object sender, DragEventArgs e) => Btn_DragLeave(sender, e, gField, field);
                btn.DragOver += (object sender, DragEventArgs e) => Btn_DragOver(sender, e, gField, field);
                btn.Drop += (object sender, DragEventArgs e) => Btn_Drop(sender, e, gField, field);
                btn.MouseDown += (object sender, MouseButtonEventArgs e) => Btn_MouseDown(sender, e, gField, canv);
                btn.MouseDoubleClick += (object sender, MouseButtonEventArgs e) => Btn_MouseDoubleClick(sender, e, gField, field);
                //rect.Stroke = GameEngine.Colors.GetColorFromRGBA("#FF000000");
                //rect.StrokeThickness = 1;
                //rect.MouseDoubleClick += (object sender, MouseButtonEventArgs e) => Rect_MouseDown(sender, e, gField);
                Canvas.SetLeft(btn, (i % 10) * 30);
                Canvas.SetTop(btn, (i / 10) * 30);
                canv.Children.Add(btn);
            }
        }

        private void Btn_MouseDown(object sender, MouseButtonEventArgs e, GameField gField, Canvas canv)
        {
            Button btn = (Button)sender;
            int position1d = canv.Children.IndexOf(btn);
            Point p = GameEngine.Get2DPosition(position1d);
            int state = gField.GetFState(p);
            if (state != GameEngine.FieldStates.Ship)
            {
                return;
            }
            //List<Point> shipPoints = gField.GetAllShipParts(p);
            //int count = shipPoints.Count;

            //ValueTuple<int, Point> data = (count, p);




            DragDrop.DoDragDrop((Button)sender, new DataObject(DataFormats.Serializable, p), DragDropEffects.Move);
        }

        private void Btn_MouseDoubleClick(object sender, MouseButtonEventArgs e, GameField gField, Canvas field)
        {
            //(int length, int direction) = (ValueTuple<int, int>)e.Data.GetData(DataFormats.Serializable);
            //Vector v =  GameEngine.DirectionVectors.GetDirectionVector(direction);
            Button btn = (Button)sender;




            Point pos = GameEngine.Get2DPosition(field.Children.IndexOf(btn));

            gField.RemoveShip(pos);

            UpdateField(gField, field);


        }

        private void Btn_Drop(object sender, DragEventArgs e, GameField gField, Canvas field)
        {
            int direction = currentDirection;
            int length = (int)e.Data.GetData(DataFormats.Serializable);
            Vector v =  GameEngine.DirectionVectors.GetDirectionVector(direction);
            Button btn = (Button)sender;

            Point pos = GameEngine.Get2DPosition(field.Children.IndexOf(btn));

            gField.PlaceShip(length, pos, direction);

            UpdateField(gField, field);

        }

        private void Btn_DragOver(object sender, DragEventArgs e, GameField gField, Canvas field)
        {
            int direction = currentDirection;
            int length = (int)e.Data.GetData(DataFormats.Serializable);
        }

        private void Btn_DragLeave(object sender, DragEventArgs e, GameField gField, Canvas field)
        {
            int direction = currentDirection;
            int length = (int)e.Data.GetData(DataFormats.Serializable);
            UpdateField(gField, field);
        }

        private void Btn_DragEnter(object sender, DragEventArgs e, GameField gField, Canvas field)
        {



            int direction = currentDirection;
            int length = (int)e.Data.GetData(DataFormats.Serializable);
            Vector v =  GameEngine.DirectionVectors.GetDirectionVector(direction);
            Button btn = (Button)sender;

            Point pos = GameEngine.Get2DPosition(field.Children.IndexOf(btn));
            
            Point t = pos;
            
            List<Point> shipPoints = new List<Point>(){ pos };
            for(int i = 0; i < length -1; i++)
            {
                t.Offset(v.X, v.Y);
                if (gField.isPointInField(t)){
                    shipPoints.Add(t);
                }
            }

            SolidColorBrush color = gField.TryPlaceShip(length, pos, direction) ? GameEngine.Colors.EditorShipPlacementAvailable : GameEngine.Colors.EditorShipPlacementUnavailable;

            foreach(var point in shipPoints){
                ((Button)field.Children[GameEngine.GetInlinePosition(point)]).Background = color;
            }
                
        }


        

        private void FourShipProvider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //LENGTH, DIRECTION
            var data = 4;
            if (gField.Sizes[data - 1] != 0)
            {
                DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable, data), DragDropEffects.Move);
            }
        }

        private void ThreeShipProvider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var data = 3;
            if (gField.Sizes[data - 1] != 0)
            {
                DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable, data), DragDropEffects.Move);
            }

        }

        private void TwoShipProvider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var data = 2;
            if (gField.Sizes[data - 1] != 0)
            {
                DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable, data), DragDropEffects.Move);
            }
        }

        private void OneShipProvider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var data = 1;
            if (gField.Sizes[data - 1] != 0)
            {
                DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable, data), DragDropEffects.Move);
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Space)
            //{
            //   ToggleShipPlacementDirection();
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gField.MakeRandomShipPlacement();
            UpdateField(gField, field);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ToggleShipPlacementDirection();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            gField.Clear();
            UpdateField(gField, field);
        }

        private void StartGameClick(object sender, RoutedEventArgs e)
        {
            if (gField.Sizes[0] == 0 && gField.Sizes[1] == 0 && gField.Sizes[2] == 0 && gField.Sizes[3] == 0)
            {
                GameWindow game = new GameWindow(gField, GameMode);
                this.Hide();
                game.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBoxResult r = AcrylicMessageBox.Show(this, GameEngine.Messages.EditorNoEnoughShips);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Tutorial t = new Tutorial();
            t.ShowDialog();
        }
    }
}
