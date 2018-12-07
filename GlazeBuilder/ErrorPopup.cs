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
                Height = 250
            };

        Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

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
