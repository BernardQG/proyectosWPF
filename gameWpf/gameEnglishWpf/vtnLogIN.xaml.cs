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

namespace gameEnglishWpf
{
    /// <summary>
    /// Lógica de interacción para vtnLogIN.xaml
    /// </summary>
    public partial class vtnLogIN : Window
    {
        public vtnLogIN()
        {
            InitializeComponent();
        }

        private void btnContinuar_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            MainWindow G = new MainWindow();
            G.Show();

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Rectangulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
