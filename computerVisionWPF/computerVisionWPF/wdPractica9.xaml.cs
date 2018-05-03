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
    /// Lógica de interacción para wdPractica9.xaml
    /// </summary>
    public partial class wdPractica9 : UserControl
    {
        Image<Gray, byte> imaO = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\rec.jpg").Convert<Gray, byte>();
        Image<Gray, byte> imaAux = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\rec.jpg").Convert<Gray, byte>();
        public wdPractica9()
        {
            InitializeComponent();
            def();
        }
        private void def()
        {

            ctlIma.Source = ToBitmapSource(imaO);


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
            OpenFileDialog fileIma = new OpenFileDialog();
            if (fileIma.ShowDialog() == true)
            {
                imaO = null;
                imaO = new Image<Rgb, byte>(fileIma.FileName).Convert<Gray, byte>();
                def();
            }

        }


        private void btnOriginal_Click(object sender, RoutedEventArgs e)
        {
            def();

        }

        private void btnRectas_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
