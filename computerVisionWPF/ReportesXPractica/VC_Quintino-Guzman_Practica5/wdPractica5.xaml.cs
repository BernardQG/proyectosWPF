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
using Microsoft.Win32;

namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdPractica5.xaml
    /// </summary>
    public partial class wdPractica5 : UserControl
    {
        Image<Rgb, byte> imaO = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\Coins.jpg");
        Image<Gray, byte> imaAux;
        Image<Gray, byte> ima;
        
        public wdPractica5()
        {
            InitializeComponent();
            eventosElementoE();
            def();
            imaAux = imaO.Convert<Gray, byte>();
        }
        private void def()//permite reiniciar los valores con nuevas imagenes, ademas de correr el algoritmo de la umbralizacion por OTSU
        {
            ima = imaO.Convert<Gray, byte>();
            OTSU();
            ctlIma.Source = ToBitmapSource(ima);
        }


        private void Erocion()
        {
            Boolean d = true;
            System.Drawing.Bitmap b;
            b = ima.Bitmap;
            imaAux.Bitmap = b;

            
            for (int i = 0; i < ima.Height; i++)//recorre en vertical            
                for (int j = 0; j < ima.Width; j++)//recorre en horizontal               
                    if (imaAux.Data[i, j, 0] == 255)
                    {
                        d = true;
                        for (int x = 0; x < 3; x++)
                            for (int y = 0; y < 3; y++)
                                if ((i + x - 1 >= 0 && i + x - 1 < ima.Height) && (j + y - 1 >= 0 && j + y - 1 < ima.Width)  && imaAux.Data[i + x - 1, j + y - 1, 0] != EE[x, y] && EE[x, y]!=0)
                                    d = false;
                        if(!d)
                         ima.Data[i, j, 0] = 0;
                    }

                    
        }

        private void Dilatacion() {

            
            System.Drawing.Bitmap b;
            b = ima.Bitmap;
            imaAux.Bitmap = b;

            for (int i = 0; i < ima.Height; i++)//recorre en vertical            
                for (int j = 0; j < ima.Width; j++)//recorre en horizontal               
                    if (imaAux.Data[i, j, 0] == 255)
                    {
                        for (int x = 0; x < 3; x++)                        
                            for (int y = 0; y < 3; y++)
                                //la condicion permite no salirce de la imagen, si el elemento estructurante requiere modificar mas alla de el tamaño de la imagen
                                if ((i + x - 1 >= 0 && i + x - 1 < ima.Height) && (j + y - 1 >= 0 && j + y - 1 < ima.Width) && EE[x, y] == 255)
                                        ima.Data[i + x - 1, j + y - 1, 0] = 255;                                
                    }
        }

        private void Contorno()
        {
            System.Drawing.Bitmap b;
            b = ima.Bitmap;
            imaAux.Bitmap = b;

            Erocion();

            for (int i = 0; i < ima.Height; i++)//recorre en vertical            
                for (int j = 0; j < ima.Width; j++)//recorre en horizontal               
                    if (imaAux.Data[i, j, 0] == 255 && imaAux.Data[i, j, 0]==ima.Data[i,j,0])
                                    imaAux.Data[i, j, 0] =0;

            ima = imaAux;
                    
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

        private void OTSU()
        {
            double[] H = new double[256];//Histograma
            double[] P = new double[256];
            double[] W_0 = new double[256];
            double[] W_1 = new double[256];
            double[] O = new double[256];

            for (int i = 0; i < H.Length; i++)
                H[i] = 0;
            //generar historama
            for (int i = 0; i < ima.Height; i++)
                for (int j = 0; j < ima.Width; j++)
                    H[ima.Data[i, j, 0]]++;
            //Normalizacion del histograma
            double N = ima.Width * ima.Height;
            for (int i = 1; i < H.Length; i++)
                P[i] = H[i] / N;

            //Probabilidad de Ocurrencia
            double sum = 0;
            for (int i = 0; i < P.Length; i++)
            {
                sum = 0;
                for (int j = 1; j < i; j++)
                    sum += P[j];
                W_0[i] = sum;

                sum = 0;
                for (int j = i + 1; j < P.Length; j++)
                    sum += P[j];
                W_1[i] = sum;
            }
            //momento acomulado
            double U = 0;
            for (int i = 0; i < P.Length; i++)
                U += i * P[i];

            //calcula las varianzas
            double O_0 = 0, O_1 = 0;
            for (int k = 1; k < P.Length; k++)
            {
                O_0 = O_1 = 0;
                for (int i = 1; i < k; i++)
                    O_0 += (i * P[i]);

                for (int i = k + 1; i < P.Length; i++)
                    O_1 += (i * P[i]);


                O[k] = W_0[k] * Math.Pow((O_0 / W_0[k] - U), 2) + W_1[k] * Math.Pow((O_1 / W_1[k] - U), 2);


            }


            //Consige el valor k del la varianza maxima
            double m = 0; int maxK = 0;
            for (int i = 0; i < H.Length; i++)
                if (O[i] > m) { m = O[i]; maxK = i; }


            humbral(maxK);
        }

        private void humbral(int h)
        {//establece el humbral en toda la imagen
            for (int i = 0; i < ima.Height; i++)//recorre en vertical
                for (int j = 0; j < ima.Width; j++)//recorre en horizontal               
                {
                    if (ima.Data[i, j, 0] < h)
                        ima.Data[i, j, 0] = 0;
                    else ima.Data[i, j, 0] = 255;
                }

        }

        private void btnOpenImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenIma = new OpenFileDialog();
            if (OpenIma.ShowDialog() == true)
            {
                imaO = null;
                imaO = new Image<Rgb, byte>(OpenIma.FileName);
                def();

            }
        }

        private void btnImagenOriginal_Click(object sender, RoutedEventArgs e)    {
            def();

        }
        //Permite cambiar el elemento estructurante.
        Byte[,] EE = {{0,0,0},{0,255,0},{0,0,0}};
        private void eventosElementoE(){
            r0.MouseDown += (s, a) => { if (EE[0, 0] == 0) { EE[0, 0] = 255; r0.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7592F3")); } else { EE[0, 0] = 0; r0.Fill = Brushes.WhiteSmoke; }  };
            r1.MouseDown += (s, a) => { if (EE[0, 1] == 0) { EE[0, 1] = 255; r1.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7592F3")); } else { EE[0, 1] = 0; r1.Fill = Brushes.WhiteSmoke; } };
            r2.MouseDown += (s, a) => { if (EE[0, 2] == 0) { EE[0, 2] = 255; r2.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7592F3")); } else { EE[0, 2] = 0; r2.Fill = Brushes.WhiteSmoke; } };
            r3.MouseDown += (s, a) => { if (EE[1, 0] == 0) { EE[1, 0] = 255; r3.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7592F3")); } else { EE[1, 0] = 0; r3.Fill = Brushes.WhiteSmoke; } };
            r5.MouseDown += (s, a) => { if (EE[1, 2] == 0) { EE[1, 2] = 255; r5.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7592F3")); } else { EE[1, 2] = 0; r5.Fill = Brushes.WhiteSmoke; } };
            r6.MouseDown += (s, a) => { if (EE[2, 0] == 0) { EE[2, 0] = 255; r6.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7592F3")); } else { EE[2, 0] = 0; r6.Fill = Brushes.WhiteSmoke; } };
            r7.MouseDown += (s, a) => { if (EE[2, 1] == 0) { EE[2, 1] = 255; r7.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7592F3")); } else { EE[2, 1] = 0; r7.Fill = Brushes.WhiteSmoke; } };
            r8.MouseDown += (s, a) => { if (EE[2, 2] == 0) { EE[2, 2] = 255; r8.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7592F3")); } else { EE[2, 2] = 0; r8.Fill = Brushes.WhiteSmoke; } };

        }

        private void btnDilatacion_Click(object sender, RoutedEventArgs e)
        {
            Dilatacion();
            ctlIma.Source = ToBitmapSource(ima);
        }

        private void btnErocion_Click(object sender, RoutedEventArgs e)
        {
            Erocion();
            ctlIma.Source = ToBitmapSource(ima);
        }

        private void btnApertura_Click(object sender, RoutedEventArgs e)
        {
            Erocion();
            Dilatacion();
            ctlIma.Source = ToBitmapSource(ima);
        }

        private void btnCerradura_Click(object sender, RoutedEventArgs e)
        {
            Dilatacion();
            Erocion();           
            ctlIma.Source = ToBitmapSource(ima);
        }

        private void btnContorno_Click(object sender, RoutedEventArgs e)
        {
            Image<Gray, byte> help = imaO.Convert<Gray, byte>(); ;
            System.Drawing.Bitmap b;
            b = ima.Bitmap;
            help.Bitmap = b;
            Contorno();
            ctlIma.Source = ToBitmapSource(ima);
            ima = help;
        }
    }
}
