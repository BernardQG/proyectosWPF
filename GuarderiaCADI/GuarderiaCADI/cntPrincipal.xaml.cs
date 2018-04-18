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
    /// Lógica de interacción para cntPrincipal.xaml
    /// </summary>
    public partial class cntPrincipal : UserControl
    {
        public cntPrincipal()
        {
            InitializeComponent();
            GridIma.Children.Clear();
            GridIma.Children.Add(new cntMostradorImagenesXTiempo());
        }
    }
}
