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
    /// Interaction logic for PlayerWonWindow.xaml
    /// </summary>
    public partial class PlayerWonWindow
    {
        public PlayerWonWindow(bool PlayerWon = true)
        {
            InitializeComponent();
            if (PlayerWon)
            {
                first.Text = GameEngine.Messages.EndPlayerWonTitle;
                second.Text = GameEngine.Messages.EndPlayerWonText;
                win.Title = GameEngine.Messages.GamePlayerWins;
            }
            else
            {
                first.Text = GameEngine.Messages.EndPlayerLostTitle;
                second.Text = GameEngine.Messages.EndPlayerLostText;
                win.Title = GameEngine.Messages.GameAIWins;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
