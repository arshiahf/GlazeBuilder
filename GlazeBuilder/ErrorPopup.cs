using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GlazeBuilder
{
    public partial class ErrorPopup : Window
    {
        public ErrorPopup(string message)
        {
            Window error = new Window
            {
                Width = 350,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            Grid grid = new Grid();
            for (int i = 0; i < 3; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < 1; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            TextBlock errorText = new TextBlock();
            errorText.ContentStart.InsertTextInRun(message);
            errorText.Width = 250;
            errorText.Height = 150;
            errorText.TextAlignment = TextAlignment.Center;
            errorText.TextWrapping = TextWrapping.Wrap;
            Grid.SetRowSpan(errorText, 2);
            Grid.SetRow(errorText, 1);

            Button errorAck = new Button();
            errorAck.Click += errorAck_Click;
            errorAck.Width = 100;
            errorAck.Height = 30;
            errorAck.Content = "OK";
            Grid.SetRow(errorAck, 2);

            void errorAck_Click(object sender, RoutedEventArgs e)
            {
                error.Close();
            }

            grid.Children.Add(errorText);
            grid.Children.Add(errorAck);
            error.Content = grid;

            error.Show();
            error.Focus();
        }
    }
}
