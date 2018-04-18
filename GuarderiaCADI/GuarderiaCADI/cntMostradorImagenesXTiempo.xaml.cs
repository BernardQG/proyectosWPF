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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace GuarderiaCADI
{
    /// <summary>
    /// Lógica de interacción para cntMostradorImagenesXTiempo.xaml
    /// </summary>
    public partial class cntMostradorImagenesXTiempo : UserControl
    {
        public cntMostradorImagenesXTiempo()
        {


            
            int i = 0;
            InitializeComponent();
            
            Storyboard on = (Storyboard)FindResource("on");
            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 3);
            Timer.Tick += (s, a) => {

                

                switch (i)
                {

                    case 0:  Ima.Source = new BitmapImage(new Uri("/FotosGuarderia/Gua2.jpeg",UriKind.Relative));
                        ImaBack.Source = new BitmapImage(new Uri("/FotosGuarderia/Gua0.jpeg", UriKind.Relative)); i++; break;
                    case 1:  Ima.Source = new BitmapImage(new Uri("/FotosGuarderia/Gua0.jpeg", UriKind.Relative));
                        ImaBack.Source = new BitmapImage(new Uri("/FotosGuarderia/Gua1.jpg", UriKind.Relative)); i++; break;
                    case 2:  Ima.Source = new BitmapImage(new Uri("/FotosGuarderia/Gua1.jpg", UriKind.Relative));
                        ImaBack.Source = new BitmapImage(new Uri("/FotosGuarderia/Gua2.jpeg", UriKind.Relative)); i =0; break;
                    default: break;
                }
                on.Begin();




            };
            Timer.Start();
        }
    }
}
