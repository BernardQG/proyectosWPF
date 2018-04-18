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

namespace GuarderiaCADI
{
    /// <summary>
    /// Lógica de interacción para vntGuarderia.xaml
    /// </summary>
    public partial class vntGuarderia : Window
    {

        private Boolean maxNormal;
        public vntGuarderia()
        {
            InitializeComponent();
            
            maxNormal = true;
        }        

        private void btnMini_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaxi_Click(object sender, RoutedEventArgs e)
        {
            if (maxNormal) { this.WindowState = WindowState.Maximized; maxNormal = false; }
            else { this.WindowState = WindowState.Normal; maxNormal = true; }

        }

        private void btnPower_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GridTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ListMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(15));
            int index = ListMenu.SelectedIndex;
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, 180 + (60 * index), 0, 0);
            GridBase.Children.Clear();
            switch (index)
            {
                case 0:     GridBase.Children.Add(new cntPrincipal());        break;
                case 1:     GridBase.Children.Add(new cntGrupos());            break;
                default:
                    break;
            }

        }
    }
}
