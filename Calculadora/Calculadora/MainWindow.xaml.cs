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

namespace Calculadora
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string num="";
        double aux=0, count=0;
        int op=0;
        public MainWindow()
        {
            InitializeComponent();
            b0.Click += (ss, ee) => { num += "0"; txtN.Text = num; };
            b1.Click += (ss, ee) => { num += "1"; txtN.Text = num; };
            b2.Click += (ss, ee) => { num += "2"; txtN.Text = num; };
            b3.Click += (ss, ee) => { num += "3"; txtN.Text = num; };
            b4.Click += (ss, ee) => { num += "4"; txtN.Text = num; };
            b5.Click += (ss, ee) => { num += "5"; txtN.Text = num; };
            b6.Click += (ss, ee) => { num += "6"; txtN.Text = num; };
            b7.Click += (ss, ee) => { num += "7"; txtN.Text = num; };
            b8.Click += (ss, ee) => { num += "8"; txtN.Text = num; };
            b9.Click += (ss, ee) => { num += "9"; txtN.Text = num; };

            
            bP.Click += (ss, ee) => { op = 1;  operacion();  };
            bD.Click += (ss, ee) => { op = 2;  operacion(); };
            bM.Click += (ss, ee) => { op = 3;  operacion(); };
            bMn.Click += (ss, ee) => { op = 4; operacion(); };

            bI.Click += (ss, ee) => { operacion(); txtN.Text = count.ToString(); num = count.ToString(); };
        }

        public void operacion() {
            txtN.Text = "";
            switch (op) {
                case 1: count = aux * double.Parse(num);  aux = double.Parse(num);   break;
                case 2: count = aux / double.Parse(num); aux = double.Parse(num); break;
                case 3: count = aux + double.Parse(num); aux = double.Parse(num); break;
                case 4: count = aux - double.Parse(num); aux = double.Parse(num); break;
                default: break;
            }
            num = "";
        }
    }
}
