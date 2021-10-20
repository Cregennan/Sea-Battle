using System;
using System.Collections.Generic;
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
    /// Interaction logic for ColorPaletteWindow.xaml
    /// </summary>
    public partial class ColorPaletteWindow
    {
        public ColorPaletteWindow()
        {
            InitializeComponent();
            ((Rectangle)colors.Children[0]).Fill = GameEngine.Colors.FieldEmpty;
            ((Rectangle)colors.Children[1]).Fill = GameEngine.Colors.FieldUnplacable;
            ((Rectangle)colors.Children[2]).Fill = GameEngine.Colors.FieldUnknown;
            ((Rectangle)colors.Children[3]).Fill = GameEngine.Colors.FieldNotPresents;
            ((Rectangle)colors.Children[4]).Fill = GameEngine.Colors.FieldDropped;
            ((Rectangle)colors.Children[5]).Fill = GameEngine.Colors.FieldShip;
            ((Rectangle)colors.Children[6]).Fill = GameEngine.Colors.FieldDestroyedShipPiece;
            ((Rectangle)colors.Children[7]).Fill = GameEngine.Colors.FieldTotalDestroyedShip;

            GameEngine.Colors.FieldColors gef = new GameEngine.Colors.GameEnemyField();
            



            ((Rectangle)colors.Children[8]).Fill = gef.Empty;
            ((Rectangle)colors.Children[9]).Fill = gef.Unplacable;
            ((Rectangle)colors.Children[10]).Fill = gef.Unknown;
            ((Rectangle)colors.Children[11]).Fill = gef.NotPresents;
            ((Rectangle)colors.Children[12]).Fill = gef.Dropped;
            ((Rectangle)colors.Children[13]).Fill = gef.Ship;
            ((Rectangle)colors.Children[14]).Fill = gef.DestroyedShipPiece;
            ((Rectangle)colors.Children[15]).Fill = gef.TotalDestroyedShip;

            gef = new GameEngine.Colors.GamePlayerField();

            ((Rectangle)colors.Children[16]).Fill = gef.Empty;
            ((Rectangle)colors.Children[17]).Fill = gef.Unplacable;
            ((Rectangle)colors.Children[18]).Fill = gef.Unknown;
            ((Rectangle)colors.Children[19]).Fill = gef.NotPresents;
            ((Rectangle)colors.Children[20]).Fill = gef.Dropped;
            ((Rectangle)colors.Children[21]).Fill = gef.Ship;
            ((Rectangle)colors.Children[22]).Fill = gef.DestroyedShipPiece;
            ((Rectangle)colors.Children[23]).Fill = gef.TotalDestroyedShip;

            ((Rectangle)colors.Children[24]).Fill = GameEngine.Colors.EditorFieldUnplacable;
            ((Rectangle)colors.Children[25]).Fill = GameEngine.Colors.EditorShipPlacementUnavailable;
            ((Rectangle)colors.Children[26]).Fill = GameEngine.Colors.EditorShipPlacementAvailable;
            ((Rectangle)colors.Children[27]).Fill = GameEngine.Colors.EditorShipProviderAvailable;
            ((Rectangle)colors.Children[28]).Fill = GameEngine.Colors.EditorShipProviderUnavailable;

        }
    }
}
