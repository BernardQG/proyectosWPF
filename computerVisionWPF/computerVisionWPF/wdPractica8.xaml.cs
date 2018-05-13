using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Emgu.CV.UI;
using Emgu.CV.Util;
using System.ComponentModel;

using Emgu.CV.CvEnum;

using System.Drawing;


namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdPractica8.xaml
    /// </summary>
    public partial class wdPractica8 : UserControl
    {

        Image<Gray, byte> imaO = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\P8\placa.jpg").Convert<Gray, byte>();
        Image<Gray, byte> imaAux = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\P8\placa.jpg").Convert<Gray, byte>();
        Image<Gray, byte> imaAux2 = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\P8\placa.jpg").Convert<Gray, byte>();
        int[,] matIx;
        int[,] matIy;
        float[,] matD;//Guarda las direcciones
        int divMat = 0;


        int[,] mascara;

        public wdPractica8()
        {
            InitializeComponent();
            mascara = new int[,] { { 1, 4, 7, 4, 1 }, { 4, 16, 26, 16, 4 }, { 7, 26, 41, 26, 7 }, { 4, 16, 26, 16, 4 }, { 1, 4, 7, 4, 1 } };
            divMat = 273;
           // mascara = new int[,] { { 1, 2, 1 }, { 2, 4,2 }, {1, 2, 1}};
            //divMat = 16;            
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
                    imaAux.Data[i, j, 0] = (byte)Math.Abs(numerador / (divMat));

                }

            }
            CvInvoke.Imshow("Convolucion", imaAux);//Convolucion
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

       


        private void Ix()
        {

            matIx = new int[imaAux.Height, imaAux.Width];
            for (int i = 0; i < imaAux.Height; i++)//recorre en vertical      
            {
                for (int j = 0; j < imaAux.Width - 1; j++)//recorre en horizontal
                {
                    matIx[i, j] = (imaAux.Data[i, j + 1, 0] - imaAux.Data[i, j, 0]);
                }
            }

            for (int i = 0; i < imaAux.Height; i++)//recorre en vertical      
            {
                matIx[i, imaAux.Width - 1] = (imaAux.Data[i, imaAux.Width - 1, 0]);
            }
         
        }


        private void Iy()
        {

            matIy = new int[imaAux.Height, imaAux.Width];
            for (int i = 0; i < imaAux.Height - 1; i++)//recorre en vertical      
            {
                for (int j = 0; j < imaAux.Width; j++)//recorre en horizontal
                {
                    matIy[i, j] = (imaAux.Data[i + 1, j, 0] - imaAux.Data[i, j, 0]);
                }
            }

            for (int j = 0; j < imaAux.Width; j++)//recorre en horizontal
            {
                matIy[imaAux.Height - 1, j] =(imaAux.Data[imaAux.Height - 1, j, 0]);
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
            CvInvoke.Imshow("Gradiente", imaAux);//el gradiente               
        }

        private void Io()
        {

            System.Drawing.Bitmap b;
            b = imaAux.Bitmap;            
            Image<Gray, byte> iM = new Image<Gray, byte>(b);
            
            matD = new float[imaAux.Height, imaAux.Width];

            for (int i = 0; i < imaAux.Height; i++)//recorre en vertical      
            {
                for (int j = 0; j < imaAux.Width; j++)//recorre en horizontal
                {
                    if (matIx[i, j] != 0)
                    
                        matD[i, j] = (float)(Math.Atan(matIy[i, j]/ matIx[i, j])*(180/Math.PI));
                    //Tangent = (float)(Math.Atan(DerivativeY[i, j] / DerivativeX[i, j]) * 180 / Math.PI); //rad to degree
                    else
                        matD[i, j] = 90F;


                    iM.Data[i, j, 0] =(byte)(matD[i, j]);
                }
            }
            CvInvoke.Imshow("Direeciones", iM);//el gradiente               

        }

        private void Canny()
        {
            // 1. Convuluciona la imagen con un filtro gaussiano
            Convolucion();
            // 2. Encontral el gradiente de la imagen 
            Ix(); Iy();
            I(); Io();
            // 3. Supresion de No maximos
            discretisar();
            comparacionVecinos();
            // 4. Histeresis de doble umbral
            histeresiDUmbral(5,15);
        }

        private void discretisar() {

            //discretizar matD
            float vAux = 0;
            for (int i = 0; i < matD.GetLength(0); i++)//recorre en vertical      
            {
                for (int j = 0; j < matD.GetLength(1); j++)//recorre en horizontal
                {

                    vAux = matD[i, j];
                    //Horizontal
                    if ((vAux > -22.5 && vAux <= 22.5) || (vAux > 157.5 && vAux <= -157.5)) vAux = 0;
                    //Vertical
                    else if ((vAux > -112.5 && vAux <= -67.5)|| (vAux > 67.5 && vAux <= 112.5)) vAux = 1;
                    //Diagonal +45
                    else if ((vAux > -67.5 && vAux <= -22.5) || (vAux > 112.5 && vAux <= 157.5)) vAux = 2;
                    //Diagonal -45
                    else if ((vAux > -157.5 && vAux <= -112.5) || (vAux > 67.5 && vAux <= 22.5)) vAux = 3;
                    
                    

                    matD[i, j] = (float)vAux;
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
                    //Horizontal
                    if (matD[i, j] == 0 )
                    {
                        if (imaAux.Data[i, j, 0] < imaAux.Data[i, j + 1, 0] || imaAux.Data[i, j, 0] < imaAux.Data[i, j - 1, 0])
                            imaAux2.Data[i, j, 0] = 0;                        
                    }
                    //Vertical
                    else if (matD[i, j] == 1)
                    {
                        if (imaAux.Data[i, j, 0] < imaAux.Data[i + 1, j, 0] || imaAux.Data[i, j, 0] < imaAux.Data[i - 1, j, 0])
                            imaAux2.Data[i, j, 0] = 0;
                       

                    }
                    //Diagonal +45
                    else if (matD[i, j] == 2)
                    {

                        if (imaAux.Data[i, j, 0] < imaAux.Data[i - 1, j + 1, 0] || imaAux.Data[i, j, 0] < imaAux.Data[i + 1, j - 1, 0])
                          imaAux2.Data[i, j, 0] = 0;

                        
                    }
                    //Diagonal -45
                    else
                    {
                        if (imaAux.Data[i, j, 0] < imaAux.Data[i - 1, j - 1, 0] || imaAux.Data[i, j, 0] < imaAux.Data[i + 1, j + 1, 0])
                            imaAux2.Data[i, j, 0] = 0;

                    }

                    
                }
                

            }
            
            CvInvoke.Imshow("Supresion", imaAux2);//el gradiente       


        }

        private void histeresiDUmbral(int U1, int U2)
        {                                 
            for (int i = 0; i <imaAux2.Height; i++)//recorre en vertical      
            {
                for (int j = 0; j < imaAux2.Width; j++)//recorre en horizontal
                {
                    if (imaAux2.Data[i, j, 0] >= U2)               
                        imaAux2.Data[i, j, 0] = 255;
                  
                }
            }
            for(int m =0;m<3;m++)
            UmbralEntreU1y2(U1, U2);


            ctlIma.Source = ToBitmapSource(imaAux2);
            
        }
        private void UmbralEntreU1y2(int U1, int U2) {

            for (int i = 1; i < imaAux2.Height - 1; i++)//recorre en vertical      
            {
                for (int j = 1; j < imaAux2.Width- 1; j++)//recorre en horizontal
                {
                                                        
                    if (imaAux2.Data[i, j, 0] >= U1 && imaAux2.Data[i, j, 0] < U2)
                    {
                        
                        for (int x = 0; x < 3; x++)
                            for (int y = 0; y < 3; y++)
                            {
                                
                                if (imaAux2.Data[i + x - 1, j + y - 1, 0] == 255)
                                    imaAux2.Data[i, j, 0] = 255;


                            }
                       
                    }                                  
                }
                
            }
            


        }

        private void btnSaveImagen_Click(object sender, RoutedEventArgs e)
        {
            imaAux2.Save(Environment.CurrentDirectory + @"\Imagenes\Canny.jpg");
        }


        //end
    }
         
}
