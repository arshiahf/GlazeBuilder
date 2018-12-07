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
            //this.MaterialDatabase = new MaterialDatabase("Materials.csv", this);
            this.GlazeDatabase = new GlazeDatabase();
            Cones.ItemsSource = GlazeDatabase.Cones.Keys;
        }

        private MaterialDatabase MaterialDatabase { get; set; }
        private GlazeDatabase GlazeDatabase { get; set; }

        public void SubmitMaterial_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
