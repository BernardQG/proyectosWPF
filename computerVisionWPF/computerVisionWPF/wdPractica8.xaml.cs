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
        Image<Gray, byte> imaAux2 = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\Coins.jpg").Convert<Gray, byte>();
        short[,] matIx;
        short[,] matIy;
        short[,] matD;//Guarda las direcciones


        int[,] mascara;

        public wdPractica8()
        {
            InitializeComponent();
            mascara = new int[,] { { 1, 4, 7, 4, 1 }, { 4, 16, 26, 16, 4 }, { 7, 26, 41, 26, 7 }, { 4, 16, 26, 16, 4 }, { 1, 4, 7, 4, 1 } };
            def();
        }
        private void def() {

            ctlIma.Source = ToBitmapSource(imaO);

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
            g180Mascara();

            for (int i = (int)(mascara.GetLength(0) / 2); i < imaO.Height - (int)(mascara.GetLength(0) / 2); i++)//recorre en vertical      
            {
                for (int j = (int)(mascara.GetLength(1) / 2); j < imaO.Width - (int)(mascara.GetLength(1) / 2); j++)//recorre en horizontal
                {

                    numerador = 0;
                    for (int x = 0; x < mascara.GetLength(0); x++)
                        for (int y = 0; y < mascara.GetLength(1); y++)
                        {

                            numerador += (imaO.Data[i + x - (int)(mascara.GetLength(0) / 2), j + y - (int)(mascara.GetLength(1) / 2), 0]) * mascara[x, y];


                        }
                    imaAux.Data[i, j, 0] = (byte)Math.Abs(numerador / (273));

                }

            }
            //ctlIma.Source = ToBitmapSource(imaAux);

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
            Canny();

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

        private void Canny() {
            // 1. Convuluciona la imagen con un filtro gaussiano
            Convolucion();
            // 2. Encontral el gradiente de la imagen 
            Ix(); Iy();
            I(); Io();
            // 3. Supresion de No maximos
            discretisar();
            comparacionVecinos();
            // 4. Histeresis de doble umbral
           histeresiDUmbral();
        }


        private void Ix()
        {

            matIx = new short[imaAux.Height, imaAux.Width];
            for (int i = 0; i < imaAux.Height; i++)//recorre en vertical      
            {
                for (int j = 0; j < imaAux.Width - 1; j++)//recorre en horizontal
                {
                    matIx[i, j] = (short)(imaAux.Data[i, j + 1, 0] - imaAux.Data[i, j, 0]);
                }
            }

            for (int i = 0; i < imaAux.Height; i++)//recorre en vertical      
            {
                matIx[i, imaAux.Width - 1] = (short)(imaAux.Data[i, imaAux.Width - 1, 0]);
            }
        }


        private void Iy()
        {

            matIy = new short[imaAux.Height, imaAux.Width];
            for (int i = 0; i < imaAux.Height - 1; i++)//recorre en vertical      
            {
                for (int j = 0; j < imaAux.Width; j++)//recorre en horizontal
                {
                    matIy[i, j] = (short)(imaAux.Data[i + 1, j, 0] - imaAux.Data[i, j, 0]);
                }
            }

            for (int j = 0; j < imaAux.Width; j++)//recorre en horizontal
            {
                matIy[imaAux.Height - 1, j] = (short)(imaAux.Data[imaAux.Height - 1, j, 0]);
            }
        }


        private void I()//el Gradiente
        {

            for (int i = 0; i < imaAux.Height; i++)//recorre en vertical      
            {
                for (int j = 0; j < imaAux.Width; j++)//recorre en horizontal
                {

                    imaAux.Data[i, j, 0] = (byte)(Math.Abs(Math.Sqrt(Math.Pow(matIx[i, j], 2) + Math.Pow(matIy[i, j], 2))));
                }
            }

            // ctlIma.Source = ToBitmapSource(imaAux);//el gradiente
        }

        private void Io()
        {
            matD = new short[imaAux.Height, imaAux.Width];

            for (int i = 0; i < imaAux.Height; i++)//recorre en vertical      
            {
                for (int j = 0; j < imaAux.Width; j++)//recorre en horizontal
                {
                    if (matIx[i, j] != 0)
                        matD[i, j] = (short)(Math.Tanh(matIy[i, j] / matIx[i, j]));
                    else
                        matD[i, j] = (short)(Math.Tanh(0));

                }
            }
        }


        private void discretisar() {

            //discretizar matD
            double vAux = 0;
            for (int i = 0; i < matD.GetLength(0); i++)//recorre en vertical      
            {
                for (int j = 0; j < matD.GetLength(1); j++)//recorre en horizontal
                {


                    if (matD[i, j] - 22.5 < 0) vAux = 360 - matD[i, j] - 22.5;
                    else vAux = matD[i, j] - 22.5;

                    if (vAux < 180) {
                        if (vAux >= 0 && vAux < 45) vAux = 0;
                        else if (vAux >= 45 && vAux < 90) vAux = 45;
                        else if (vAux >= 90 && vAux < 135) vAux = 90;
                        else vAux = 135;
                    }
                    else {
                        if (vAux >= 180 && vAux < 225) vAux = 180;
                        else if (vAux >= 225 && vAux < 270) vAux = 225;
                        else if (vAux >= 270 && vAux < 315) vAux = 270;
                        else vAux = 315;
                    }

                    matD[i, j] = (short)vAux;
                }
            }

        }
        private void comparacionVecinos()
        {
            System.Drawing.Bitmap b;
            b = imaAux.Bitmap;
            imaAux2.Bitmap = b;

            //short[,] matAux = new short[matD.GetLength(0), matD.GetLength(1)];
            for (int i = 1; i < matD.GetLength(0) - 1; i++)//recorre en vertical      
            {
                for (int j = 1; j < matD.GetLength(1) - 1; j++)//recorre en horizontal
                {
                    if (matD[i, j] == 0 || matD[i, j] == 180)
                    {
                        if (imaAux.Data[i, j, 0] > imaAux.Data[i, j + 1, 0] && imaAux.Data[i, j, 0] > imaAux.Data[i, j - 1, 0])
                            imaAux2.Data[i, j, 0] = imaAux.Data[i, j, 0];
                        else imaAux2.Data[i, j, 0] = 0;
                    }

                    else if (matD[i, j] == 90 || matD[i, j] == 270)
                    {
                        if (imaAux.Data[i, j, 0] > imaAux.Data[i + 1, j, 0] && imaAux.Data[i, j, 0] > imaAux.Data[i - 1, j, 0])
                            imaAux2.Data[i, j, 0] = imaAux.Data[i, j, 0];
                        else imaAux2.Data[i, j, 0] = 0;
                    }
                    else if (matD[i, j] == 135 || matD[i, j] == 312)
                    {
                        if (imaAux.Data[i, j, 0] > imaAux.Data[i - 1, j - 1, 0] && imaAux.Data[i, j, 0] > imaAux.Data[i + 1, j + 1, 0])
                            imaAux2.Data[i, j, 0] = imaAux.Data[i, j, 0];
                        else imaAux2.Data[i, j, 0] = 0;
                    }

                    else if (matD[i, j] == 45 || matD[i, j] == 225)
                    {
                        if (imaAux.Data[i, j, 0] > imaAux.Data[i - 1, j + 1, 0] && imaAux.Data[i, j, 0] > imaAux.Data[i + 1, j - 1, 0])
                            imaAux2.Data[i, j, 0] = imaAux.Data[i, j, 0];
                        else imaAux2.Data[i, j, 0] = 0;
                    }
                }

            }


            
        }

        private void histeresiDUmbral()
        {
            
            
            for (int i = 1; i < matD.GetLength(0) - 1; i++)//recorre en vertical      
            {
                for (int j = 1; j < matD.GetLength(1) - 1; j++)//recorre en horizontal
                {
                    if (imaAux2.Data[i, j,0] > 5) imaAux2.Data[i, j, 0] = 0;
                    else imaAux2.Data[i, j, 0] = 255;
                }
            }
            ctlIma.Source = ToBitmapSource(imaAux2);
        }
            //end
        }
         
}
