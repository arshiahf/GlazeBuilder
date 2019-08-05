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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GlazeBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GlazeDatabase = new GlazeDatabase();
            Cones.ItemsSource = GlazeDatabase.Cones.Keys;
            MaterialsList.ItemsSource = GlazeDatabase.MaterialDatabase.Materials.Keys;
        }

        private MaterialDatabase MaterialDatabase { get; set; }
        private GlazeDatabase GlazeDatabase { get; set; }

        public void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (!MaterialsView.Items.Contains(MaterialsList.SelectedValue))
            {
                MaterialsView.Items.Add(MaterialsList.SelectedValue);
            }
        }

        public void RemoveMaterial_Click(object sender, RoutedEventArgs e)
        {
            MaterialsView.Items.Remove(MaterialsView.SelectedValue);
        }
    }
}
