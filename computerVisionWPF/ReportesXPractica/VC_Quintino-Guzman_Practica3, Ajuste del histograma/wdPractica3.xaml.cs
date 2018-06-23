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

namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdPractica3.xaml
    /// </summary>
    public partial class wdPractica3 : UserControl
    {
        private Image<Bgr, byte> imaO = new Image<Bgr, byte>(Environment.CurrentDirectory+ @"\im.jpg"); //lee la imagen por default a color       
        private Image<Gray, byte> Ima;//para metodo 1
        private Image<Gray, byte> Ima_2;//metodo 2
        public wdPractica3()
        {
            InitializeComponent();
            //cargar las imagenes default
            def();
            //eventos que al dar clik en una imagen muestra su histograma
            ImaOriginal.MouseDown += (ee, ss) => { HistogramViewer.Show(imaO); };
            Ima1.MouseDown += (ee, ss) => { HistogramViewer.Show(imaO.Convert<Gray, byte>());         };
            Ima2.MouseDown += (ee, ss) => { HistogramViewer.Show(Ima_2);         };
           Ima3.MouseDown += (ee, ss) => { HistogramViewer.Show(Ima);            };
           
           }

        private void def() //cargar las imagenes 
        {
            Ima = imaO.Convert<Gray, byte>();
            Ima_2 = imaO.Convert<Gray, byte>();

            ImaOriginal.Source = ToBitmapSource(imaO);
            Ima1.Source = ToBitmapSource(Ima);
            Ima2.Source = ToBitmapSource(Ima_2);
            Ima3.Source = ToBitmapSource(Ima);
            
            contrasteLineal();
            contrasteEcualizacion();
          
        }

       public int[] H = new int[256];

       private void contrasteEcualizacion()//Metodo 2
        {
            //genera el arreglo del histograma 0 255
            for (int i = 0; i < H.Length; i++)
                H[i] = 0;
            for (int i = 0; i < Ima_2.Height; i++)//recorre en vertical
                for (int j = 0; j < Ima_2.Width; j++)//recorre en horizontal                
                    H[Ima_2.Data[i, j, 0]]++;
                      
            //Ajuste de la imagen
            for (int i = 0; i < Ima_2.Height; i++)//recorre en vertical
                for (int j = 0; j < Ima_2.Width; j++)//recorre en horizontal                
                    Ima_2.Data[i, j, 0] = (byte)(fU(Ima_2.Data[i, j, 0]) * 255 / (Ima_2.Width * Ima_2.Height));
            Ima2.Source = ToBitmapSource(Ima_2);

        }
        private int fU(int k){ //distribucion de probabilidad acomulado                      
            int sum = 0;
            for (int i = 0; i <=k; i++)            
                sum += H[i];              
            return sum;            
        }

        private void contrasteLineal()//Metodo1
        {
            byte minV = 255, maxV = 0;
            //ciclos para obtener los valores minimos y maximos
            for (int i = 0; i < Ima.Height; i++)//recorre en vertical
            {
                for (int j = 0; j < Ima.Width; j++)//recorre en horizontal
                {
                    if (Ima.Data[i, j, 0] > maxV)
                        maxV = Ima.Data[i, j, 0];
                    if (Ima.Data[i, j, 0] < minV)
                        minV = Ima.Data[i, j, 0];
                }
            }
            txtLinea.Text = minV + ", " + maxV; //solo muestra los valores mminimos y maximos        

            for (int i = 0; i < Ima.Height; i++)//recorre en vertical
                for (int j = 0; j < Ima.Width; j++)//recorre en horizontal               
                    Ima.Data[i, j, 0] = (byte)(255 * ((Ima.Data[i, j, 0] - minV)) / (maxV - minV));


            Ima3.Source = ToBitmapSource(Ima);
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
 

        private void OpenImagen_Click(object sender, RoutedEventArgs e)//abrea cualquier imagen para procesarla 
        {
            OpenFileDialog OpenIma = new OpenFileDialog();
            if (OpenIma.ShowDialog() == true)
            {
                imaO = null;
                imaO = new Image<Bgr, byte>(OpenIma.FileName);
                def();
                
            }
        }
    }
}
