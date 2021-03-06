﻿using System;
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

namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdMain.xaml
    /// </summary>
    public partial class wdMain : Window
    {
        public wdMain()
        {
            InitializeComponent();
            listP.SelectionChanged += new SelectionChangedEventHandler(listChange);
            gridMain.Children.Clear();
            gridMain.Children.Add(new wdPractica1());
            maxNormal = true;
        }

        private void listChange(object sender, SelectionChangedEventArgs e)
        {
            int idx = listP.SelectedIndex;



            switch (idx)
            {
                
                case 0:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdPractica1()); break;
                case 1:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdPractica2()); break;
                case 2:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdPractica3()); break;
                case 3:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdPractica4()); break;
                case 4:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdPractica5()); break;
                case 5:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdPractica6()); break;
                case 6:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdPractica7()); break;
                case 7:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdPractica8()); break;
                case 8:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdPractica9()); break;
                case 9:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdPractica10()); break;
                case 10:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdProyectoFinal()); 
                    //MessageBox.Show("Aun no hecho");
                    break;
                case 11:
                    gridMain.Children.Clear();
                    gridMain.Children.Add(new wdExamen()); break;

                case 12:
                    wdwExampleOne vtnOne = new wdwExampleOne();
                    vtnOne.Show();

                    break;


                default:
                    break;
            }
        }


        /*
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        */
        private Boolean maxNormal;
        private void Barra_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
    }
}
