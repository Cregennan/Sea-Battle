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
    /// Interaction logic for InitWindow.xaml
    /// </summary>
    public partial class InitWindow
    {
        public int GameMode;
        public InitWindow()
        {
           
            
            InitializeComponent();
            //ColorPaletteWindow win = new ColorPaletteWindow();
            //this.Close();
            //win.Show();
            //return;



            ComboBoxItem easy = new ComboBoxItem();
            easy.Content = GameEngine.Messages.IntroGameModeEasy;
            ComboBoxItem hard = new ComboBoxItem();
            hard.Content = GameEngine.Messages.IntroGameModeHard;
            gamemode.Items.Add(easy);
            gamemode.Items.Add(hard);
            gamemode.SelectedItem = easy;



            //ManualPlaceShipsWindow window = new ManualPlaceShipsWindow(GameEngine.AI.GameModeIntellectual);
            //this.Close();
            //window.Show();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (gamemode.Text)
            {
                case GameEngine.Messages.IntroGameModeEasy:
                    GameMode = GameEngine.AI.GameModeRandom;
                    break;
                case GameEngine.Messages.IntroGameModeHard:
                    GameMode = GameEngine.AI.GameModeIntellectual;
                    break;
            }
            ManualPlaceShipsWindow window = new ManualPlaceShipsWindow(GameMode);
            this.Close();
            window.Show();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ColorPaletteWindow win = new ColorPaletteWindow();
            win.ShowDialog();
        }
    }
}
