using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Sea_Battle
{
    class FullscreenMessage
    {
        public static async void Show(String message, String backgroundRGBA, String textRGBA,  int stayTimeMillis, Grid element, int easeTimeMillis = 500,  int z = 100, int fontSize = 24)
        {

                Grid main = new Grid();
                Panel.SetZIndex(main, z);


                Brush background = (SolidColorBrush)new BrushConverter().ConvertFrom(backgroundRGBA);
                main.Background = background;


                Grid second = new Grid();
                main.Children.Add(second);
                second.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                second.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            
                TextBlock text = new TextBlock();
                text.Text = message;
                text.FontSize = fontSize;
                text.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(600);
                second.Children.Add(text);    

                element.Children.Add(main);
                
                
                DoubleAnimation easein = new DoubleAnimation();
                easein.From = 0;
                easein.To = 1;
                easein.Duration = TimeSpan.FromMilliseconds(easeTimeMillis);
                easein.EasingFunction = new CubicEase();
                text.BeginAnimation(TextBlock.OpacityProperty, easein);
                main.BeginAnimation(Grid.OpacityProperty, easein);
                await Task.Delay(easeTimeMillis);
                

                await Task.Delay(stayTimeMillis);
                
                DoubleAnimation easeout = new DoubleAnimation();
                easein.From = 1;
                easein.To = 0;
                easein.Duration = TimeSpan.FromMilliseconds(easeTimeMillis);
                text.BeginAnimation(TextBlock.OpacityProperty, easeout);
                main.BeginAnimation(Grid.OpacityProperty, easeout);
                await Task.Delay(2 * easeTimeMillis);
                
                element.Children.Remove(main);



        }
    }
}
