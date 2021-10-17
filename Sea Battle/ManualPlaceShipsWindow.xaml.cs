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
    public partial class ManualPlaceShipsWindow : Window
    {   
        GameField gField = new GameField();
        
        int[] ShipCounts = new int[4]{ 4,3,2,1 };


        int currentDirection = 0;

        List<Rectangle> ShipProviders;


        public ManualPlaceShipsWindow()
        {

            InitializeComponent();

            ShipProviders = new List<Rectangle>(){ FourShipProvider , ThreeShipProvider, TwoShipProvider, OneShipProvider};
            PlaceProvidersHorizontally();


            InitFieldRects(field, gField);
            UpdateField(gField, field);


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
                btn.MouseDoubleClick += (object sender, MouseButtonEventArgs e) => Btn_MouseDoubleClick(sender, e, gField, field);
                //rect.Stroke = GameEngine.Colors.GetColorFromRGBA("#FF000000");
                //rect.StrokeThickness = 1;
                //rect.MouseDoubleClick += (object sender, MouseButtonEventArgs e) => Rect_MouseDown(sender, e, gField);
                Canvas.SetLeft(btn, (i % 10) * 30);
                Canvas.SetTop(btn, (i / 10) * 30);
                canv.Children.Add(btn);
            }
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

            SolidColorBrush color = gField.TryPlaceShip(length, pos, direction) ? GameEngine.Colors.CreatorShipPlacementAvailable : GameEngine.Colors.CreatorShipPlacementUnavailable;

            foreach(var point in shipPoints){
                ((Button)field.Children[GameEngine.GetInlinePosition(point)]).Background = color;
            }
                
        }


        

        private void FourShipProvider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //LENGTH, DIRECTION
            var data = 4;
            DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable, data), DragDropEffects.Move);
        }

        private void ThreeShipProvider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var data = 3;
            DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable, data), DragDropEffects.Move);
        }

        private void TwoShipProvider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var data = 2;
            DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable, data), DragDropEffects.Move);
        }

        private void OneShipProvider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var data = 1;
            DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable, data), DragDropEffects.Move);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
               ToggleShipPlacementDirection();
            }
        }
    }
}
