using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using Microsoft.Win32;



namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdPractica8.xaml
    /// </summary>
    public partial class wdPractica8 : UserControl
    {

        Image<Gray, byte> imaO = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\Coins.jpg").Convert<Gray, byte>();
        Image<Gray, byte> imaAux = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\Coins.jpg").Convert<Gray, byte>();

        int[,] mascara;

        public wdPractica8()
        {
            InitializeComponent();
            mascara = new int[,] { { 1, 4,7, 4, 1 },{ 4, 16, 26, 16, 4 },{ 7, 26, 41, 26, 7 },{ 4, 16, 26,16, 4 },{ 1, 4, 7, 4, 1 }};
            def();
        }
        private void def() {

            ctlIma.Source = ToBitmapSource(imaO);
            g180Mascara();
        }


        private void g180Mascara()
        {
            int[,] aux = new int[mascara.GetLength(0), mascara.GetLength(1)];
            int jj = 0, ii = 0;
            ii = mascara.GetLength(0) - 1;

            for (int i = 0; i < mascara.GetLength(0); i++)//recorre en vertical      
            {

                jj = mascara.GetLength(1) - 1;
                for (int j = 0; j < mascara.GetLength(1); j++)//recorre en horizontal
                {
                    aux[i, j] = mascara[ii, jj];
                    jj--;
                }
                ii--;
            }
            mascara = aux;
        }
        private void Convolucion()
        {

            double numerador = 0;
            System.Drawing.Bitmap b;
            b = imaO.Bitmap;
            imaAux.Bitmap = b;


            for (int i = (int)(mascara.GetLength(0) / 2); i < imaO.Height - (int)(mascara.GetLength(0) / 2); i++)//recorre en vertical      
            {
                for (int j = (int)(mascara.GetLength(1) / 2); j < imaO.Width - (int)(mascara.GetLength(1) / 2); j++)//recorre en horizontal
                {

                    numerador = 0;
                    for (int x = 0; x < mascara.GetLength(0); x++)
                        for (int y = 0; y < mascara.GetLength(1); y++)
                        {

                            numerador += (imaO.Data[i + x - (int)(mascara.GetLength(0) / 2), j + y - (int)(mascara.GetLength(1) / 2), 0])*mascara[x,y];


                        }
                    imaAux.Data[i, j, 0] = (byte)Math.Abs(numerador / (273));

                }

            }
            ctlIma.Source = ToBitmapSource(imaAux);
            b = imaAux.Bitmap;
            imaO.Bitmap = b;


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

        private void btnOriginal_Click(object sender, RoutedEventArgs e)
        {
            ctlIma.Source = ToBitmapSource(imaO);
        }

        private void btnCanny_Click(object sender, RoutedEventArgs e)
        {
            Convolucion();
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

        private void btnIx_Click(object sender, RoutedEventArgs e)
        {
            Ix();
        }

        private void btnIy_Click(object sender, RoutedEventArgs e)
        {
            Iy();
        }

        private void btnIu_Click(object sender, RoutedEventArgs e)
        {
            Iu();
        }

        private void btnIo_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Ix()
        {

            
            System.Drawing.Bitmap b;
            b = imaO.Bitmap;
            imaAux.Bitmap = b;


            for (int i = (int)(mascara.GetLength(0) / 2); i < imaO.Height - (int)(mascara.GetLength(0) / 2); i++)//recorre en vertical      
            {
                for (int j = (int)(mascara.GetLength(1) / 2); j < imaO.Width - (int)(mascara.GetLength(1) / 2)-1; j++)//recorre en horizontal
                {


                    imaAux.Data[i, j, 0] = (byte)(Math.Abs(imaO.Data[i, j+1, 0] - imaO.Data[i, j, 0]));

                }

            }
            ctlIma.Source = ToBitmapSource(imaAux);
            b = imaAux.Bitmap;
            imaO.Bitmap = b;


        }


        private void Iy()
        {


            System.Drawing.Bitmap b;
            b = imaO.Bitmap;
            imaAux.Bitmap = b;


            for (int i = (int)(mascara.GetLength(0) / 2); i < imaO.Height - (int)(mascara.GetLength(0) / 2)-1; i++)//recorre en vertical      
            {
                for (int j = (int)(mascara.GetLength(1) / 2); j < imaO.Width - (int)(mascara.GetLength(1) / 2); j++)//recorre en horizontal
                {


                    imaAux.Data[i, j, 0] = (byte)(Math.Abs(imaO.Data[i+1, j, 0] - imaO.Data[i, j, 0]));

                }

            }
            ctlIma.Source = ToBitmapSource(imaAux);
            b = imaAux.Bitmap;
            imaO.Bitmap = b;


        }


        private void Iu()
        {


            System.Drawing.Bitmap b;
            b = imaO.Bitmap;
            imaAux.Bitmap = b;


            for (int i = (int)(mascara.GetLength(0) / 2); i < imaO.Height - (int)(mascara.GetLength(0) / 2)-1; i++)//recorre en vertical      
            {
                for (int j = (int)(mascara.GetLength(1) / 2); j < imaO.Width - (int)(mascara.GetLength(1) / 2) - 1; j++)//recorre en horizontal
                {


                    imaAux.Data[i, j, 0] = (byte)(Math.Abs(Math.Sqrt(Math.Pow(imaO.Data[i+1, j, 0] - imaO.Data[i, j, 0],2) + Math.Pow(imaO.Data[i, j + 1, 0] - imaO.Data[i, j, 0], 2))));

                }

            }
            ctlIma.Source = ToBitmapSource(imaAux);
            b = imaAux.Bitmap;
            imaO.Bitmap = b;


        }

    }
}
