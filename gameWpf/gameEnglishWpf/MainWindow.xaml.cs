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

namespace gameEnglishWpf
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean maxNormal;
        public MainWindow()
        {
            InitializeComponent();
            maxNormal = true;
        }

        private void btnMaxi_Click(object sender, RoutedEventArgs e)
        {
            if (maxNormal) { this.WindowState = WindowState.Maximized; maxNormal = false; }
            else { this.WindowState = WindowState.Normal; maxNormal = true; }

        }

        private void btnMini_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnPower_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

        private void ListMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListMenu.SelectedIndex;
            switch (index)
            {
                case 4: Application.Current.Shutdown(); break;
                
                default:
                    break;
            }
        }

       
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
