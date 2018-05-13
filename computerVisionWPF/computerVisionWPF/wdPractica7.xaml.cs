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
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Microsoft.Win32;
using System.ComponentModel;

namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdPractica7.xaml
    /// </summary>
    public partial class wdPractica7 : UserControl
    {
        Image<Gray, byte> imaO = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\P7\Coins.jpg").Convert<Gray, byte>();
        Image<Gray, byte> imaAux = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\P7\Coins.jpg").Convert<Gray, byte>();

        int[,] mascara;
        int n=5;//Es el tamaño de la mascara de la comvolucion simple

        public wdPractica7()
        {
            InitializeComponent();
            dMascara();
            def();
        }
        private void def()//permite reiniciar los valores con nuevas imagenes, ademas de correr el algoritmo de la practica
        {            
            ctlIma.Source = ToBitmapSource(imaO);           
        }


        private void g180Mascara()
        {
            int [,] aux = new int [mascara.GetLength(0), mascara.GetLength(1)];
            int jj=0, ii=0;
            ii = mascara.GetLength(0)-1;
            
             for (int i = 0; i < mascara.GetLength(0); i++)//recorre en vertical      
             {

                 jj = mascara.GetLength(1)-1;
                 for (int j = 0; j < mascara.GetLength(1); j++)//recorre en horizontal
                 {
                     aux[i,j]= mascara[ii,jj];
                     jj--;                  
                 }
                 ii--;                
             }
            mascara = aux;
        }
         private void Convolucion() {

            double numerador = 0;
            System.Drawing.Bitmap b;
            b = imaO.Bitmap;
            imaAux.Bitmap = b;           
                       

            for (int i = (int)(n / 2); i < imaO.Height - (int)(n / 2); i++)//recorre en vertical      
            {
                for (int j = (int)(n / 2); j < imaO.Width - (int)(n / 2); j++)//recorre en horizontal
                {

                    numerador = 0;
                    for (int x = 0; x < n; x++)
                        for (int y = 0; y < n; y++)
                        {
                            
                                numerador += (imaO.Data[i + x - (int)(n / 2), j + y - (int)(n/ 2), 0]);

                            
                        }
                    imaAux.Data[i, j, 0] = (byte)Math.Abs(numerador/(n*n));
                   
                }

            }
            ctlIma.Source = ToBitmapSource(imaAux);


        }

        private void deteccionDBordes()
        {

            double numerador = 0;
            System.Drawing.Bitmap b;
            b = imaO.Bitmap;
            imaAux.Bitmap = b;


            for (int i = 0; i < imaO.Height; i++)//recorre en vertical      
            {
                for (int j = 0; j < imaO.Width; j++)//recorre en horizontal
                {

                    numerador = 0;
                    for (int x = 0; x < mascara.GetLength(0); x++)
                        for (int y = 0; y < mascara.GetLength(1); y++)
                        {
                            if ((i + x - (int)(mascara.GetLength(1) / 2) >= 0 && i + x - (int)(mascara.GetLength(1) / 2) < imaO.Height) && (j + y - (int)(mascara.GetLength(0) / 2) >= 0 && j + y - (int)(mascara.GetLength(0) / 2) < imaO.Width))
                            {
                                numerador += (imaO.Data[i + x - (int)(mascara.GetLength(1) / 2), j + y - (int)(mascara.GetLength(0) / 2), 0]) * (mascara[x, y]);

                            }
                        }
                    imaAux.Data[i, j, 0] = (byte)Math.Abs(numerador / (mascara.GetLength(0) * mascara.GetLength(1)));

                }

            }
            if (txtH.Text != string.Empty)
                humbral();
            ctlIma.Source = ToBitmapSource(imaAux);


        }
        private void humbral() {

            

            for (int i = 0; i < imaO.Height; i++)//recorre en vertical      
            
                for (int j = 0; j < imaO.Width; j++)//recorre en horizontal
                {
                    if (imaAux.Data[i, j, 0] > Int32.Parse(txtH.Text)) imaAux.Data[i, j, 0] = 255;
                    else imaAux.Data[i, j, 0] = 0;

                }
            

        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        public static BitmapSource ToBitmapSource(IImage image)//un metodo que requiere el emgucv
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop
                  .Imaging.CreateBitmapSourceFromHBitmap(
                  ptr,
                  IntPtr.Zero,
                  Int32Rect.Empty,
                  System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }

        private void btnOpenImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenIma = new OpenFileDialog();
            if (OpenIma.ShowDialog() == true)
            {
                imaO = null;
                imaO = new Image<Rgb, byte>(OpenIma.FileName).Convert<Gray, byte>();
                def();
            }
        }

        private void btnOriginal_Click(object sender, RoutedEventArgs e)
        {
            ctlIma.Source = ToBitmapSource(imaO);
        }

        private void btnConvolucion_Click(object sender, RoutedEventArgs e)
        {
            Convolucion();
        }

        private void btnBordes_Click(object sender, RoutedEventArgs e)
        {

            mascara = list.ElementAt(cBox.SelectedIndex);
            g180Mascara();
            deteccionDBordes();
        }
        private List<int[,]> list = new List<int[,]>();
        private void dMascara() {
            
            list.Add(new int[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } });            
            list.Add(new int[,] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } });
            list.Add(new int[,] { { -1, -1, 0 }, { -1, 0, 1 }, { 0, 1, 1 } });
            list.Add(new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } });
            list.Add(new int[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } });            
            list.Add(new int[,] { { -2, -1, 0 }, { -1, 0,1 }, { 0, 1, 2 } });
        }
    }
}
