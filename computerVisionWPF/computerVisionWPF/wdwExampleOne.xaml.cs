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
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Microsoft.Win32;

namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdwExampleOne.xaml
    /// </summary>
    public partial class wdwExampleOne : Window
    {
        public wdwExampleOne()
        {
            InitializeComponent();
        }
        
        private void LoadIma_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Cargando...");
            OpenFileDialog openIma = new OpenFileDialog();
            if (openIma.ShowDialog() == true)
            {
                gambar = new Image<Rgb, byte>(openIma.FileName);
                imaO.Source = ToBitmapSource(gambar);

                Image<Gray, byte> gambarAbu = gambar.Convert<Gray, byte>();
                imaG.Source = ToBitmapSource(gambarAbu);


            }
        }
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        public static BitmapSource ToBitmapSource(IImage image)
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
        Image<Rgb, byte> gambar;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Image<Bgr, byte> ima = new Image<Bgr, byte>(@"c:\users\wquintino\documents\visual studio 2017\Projects\computerVisionWPF\computerVisionWPF\Resources\Image\ima1.jpg");
            //imaO.Source = ToBitmapSource(ima);
            //Image<Gray, byte> gambarAbu = ima.Convert<Gray, byte>();
            //imaG.Source = ToBitmapSource(gambarAbu);
            gambar = new Image<Rgb, byte>(@"c:\users\wquintino\documents\visual studio 2017\Projects\computerVisionWPF\computerVisionWPF\Resources\Image\ima1.jpg");
            imaO.Source = ToBitmapSource(gambar);
            Image < Gray, byte> gambarAbu = gambar.Convert<Gray, byte>();
            imaG.Source = ToBitmapSource(gambarAbu);
        }
        int k=0;
        private void btnD_Click(object sender, RoutedEventArgs e)
        {
            k =  - 30;
            Brillo();
        }

        private void btnH_Click(object sender, RoutedEventArgs e)
        {
            k =  30;
            Brillo();
        }
        
        private void Brillo() {
            //Image<Rgb, byte> gambar = new Image<Rgb, byte>(@"c:\users\wquintino\documents\visual studio 2017\Projects\computerVisionWPF\computerVisionWPF\Resources\Image\ima1.jpg");
            
            for (int i = 0; i < gambar.Height; i++)//recore en vertical
            {

                for (int j = 0; j < gambar.Width; j++)//recore en horizontal
                {


                    if (k > 0) {

                        if (gambar.Data[i, j, 0] + k <= 255)
                                gambar.Data[i, j, 0] = (byte)(gambar.Data[i, j, 0] + k);                        
                        
                        else  gambar.Data[i, j, 0] = 255; 
                        if (gambar.Data[i, j, 1] + k <= 255)
                            gambar.Data[i, j, 1] = (byte)(gambar.Data[i, j, 1] + k);

                        else  gambar.Data[i, j, 1] = 255;
                        if (gambar.Data[i, j, 2] + k <= 255)
                            gambar.Data[i, j, 2] = (byte)(gambar.Data[i, j, 2] + k);

                        else  gambar.Data[i, j,2] = 255; 
                    }
                    else {
                      
                            if (gambar.Data[i, j, 0] + k >= 0) gambar.Data[i, j, 0] = (byte)(gambar.Data[i, j, 0] + k);
                            else gambar.Data[i, j, 0] = 0;
                        if (gambar.Data[i, j, 1] + k >= 0) gambar.Data[i, j, 1] = (byte)(gambar.Data[i, j, 1] + k);
                        else gambar.Data[i, j, 1] = 0;
                        if (gambar.Data[i, j, 2] + k >= 0) gambar.Data[i, j, 2] = (byte)(gambar.Data[i, j, 2] + k);
                        else gambar.Data[i, j, 2] = 0;



                    }



                }
                


            }
            imaO.Source = ToBitmapSource(gambar);
        }
    }
}
