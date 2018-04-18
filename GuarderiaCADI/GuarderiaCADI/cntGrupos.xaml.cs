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

namespace GuarderiaCADI
{
    /// <summary>
    /// Lógica de interacción para cntGrupos.xaml
    /// </summary>
    public partial class cntGrupos : UserControl
    {
        public cntGrupos()
        {
            InitializeComponent();
        }
        
        private void btnLactAB_Click(object sender, RoutedEventArgs e)
        {
            
            GridGrupoElegido.Children.Add(new cntGrupoE());
            GridGrupoElegido.Children.Clear();
            GridGrupoElegido.Children.Add(new cntGrupoE());


        }
    }
}
