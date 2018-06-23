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
using Emgu.CV.CvEnum;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdPractica4.xaml
    /// </summary>
    public partial class wdPractica4 : UserControl
    {
        Image<Rgb, byte> imaO = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\rice.png");
        Image<Gray, byte> ima;
        Boolean unaUOtra = false;       
      
        public wdPractica4()
        {
            InitializeComponent();
            ctlIma.MouseDown += (s, a) => {//permite ver la imagen original o la imagen modificada, dandole clic cambia por una u otra
                if (unaUOtra) { ctlIma.Source = ToBitmapSource(imaO.Convert<Gray,byte>()); unaUOtra = false; }
                else{ ctlIma.Source = ToBitmapSource(ima); unaUOtra = true; }
            };
            def(); //corre el algoritmo de la practica    
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
                for (int j = i+1; j < P.Length; j++)
                    sum += P[j];
                W_1[i] = sum;
            }
            //momento acomulado
            double U = 0;
            for (int i = 0; i < P.Length; i++)            
                U+= i * P[i];

            //calcula las varianzas
            double O_0 = 0, O_1 = 0;            
            for (int k = 1; k < P.Length; k++)   {
                O_0 = O_1 = 0;
                for (int i = 1; i < k; i++)                
                    O_0 += (i * P[i]);                
                
                for (int i = k + 1; i < P.Length; i++)                
                    O_1 += (i * P[i]);


                O[k] = W_0[k] * Math.Pow((O_0 / W_0[k] - U), 2) + W_1[k] * Math.Pow((O_1 / W_1[k] - U), 2);           
                

            }
           

            //Consige el valor k del la varianza maxima
            double m = 0;  int maxK=0;
            for (int i = 0; i < H.Length; i++)
                   if (O[i] > m) {  m = O[i];   maxK = i; }

            
            txtH.Text = maxK.ToString();
            humbral(maxK);
        }
      
        private void humbral(int h) {//establece el humbral en toda la imagen
            for (int i = 0; i < ima.Height; i++)//recorre en vertical
                for (int j = 0; j < ima.Width; j++)//recorre en horizontal               
                {
                    if (ima.Data[i, j, 0] < h)
                        ima.Data[i, j, 0] = 0;
                    else ima.Data[i, j, 0] = 255;
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

        private void btnOpenImagen_Click(object sender, RoutedEventArgs e)//metodo para abrir cualquier imagen
        {
            OpenFileDialog OpenIma = new OpenFileDialog();
            if (OpenIma.ShowDialog() == true)
            {
                imaO = null;
                imaO = new Image<Rgb, byte>(OpenIma.FileName);                
                def();

            }
        }

        private void def()//permite reiniciar los valores con nuevas imagenes, ademas de correr el algoritmo de la practica
        {
            ima = imaO.Convert<Gray, byte>();
            ctlIma.Source = ToBitmapSource(ima);            
            OTSU();
        }
    }
}
