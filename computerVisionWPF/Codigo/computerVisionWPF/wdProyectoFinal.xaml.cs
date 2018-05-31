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
using System.ComponentModel;

namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdProyectoFinal.xaml
    /// </summary>
    public partial class wdProyectoFinal : UserControl
    {
        Image<Gray, byte> imaLeft = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\ProyectoFinal\imL.png").Convert<Gray, byte>();
        Image<Gray, byte> imaRight = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\ProyectoFinal\imR.png").Convert<Gray, byte>();
        Image<Gray, byte> imaResult;
        Boolean dec = true;
        BackgroundWorker bgw = new BackgroundWorker();

        public wdProyectoFinal()
        {
            InitializeComponent();
            imaLeft = contrasteEcualizacion(imaLeft);
            imaRight = contrasteEcualizacion(imaRight);
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            bgw.ProgressChanged += new ProgressChangedEventHandler(bgw_ProcessC);
            bgw.WorkerReportsProgress = true;
            imaResult = new Image<Gray, byte>(imaLeft.Width, imaRight.Height);
            ctlIma.MouseDown += new MouseButtonEventHandler(lineaDisparidad);          
            def();
        }

        private void lineaDisparidad(object sender, MouseButtonEventArgs e)//Dibuja una linea en la imagen de resultado y muestra la disparidad
        {
            int dim = 60;//dimensiones de la disparidad
            double mayor = 0, conv;//con es el factor de conversion para poderlo graficar
            System.Drawing.Bitmap b = imaResult.Bitmap;
            Image<Gray, byte> imaAux = new Image<Gray, byte>(b);
            Image<Gray, byte> imaDis = new Image<Gray, byte>(imaResult.Width, dim,new Gray(0));                       
            double [,] disM= new double[dim, imaResult.Width];
            int pY = (int)(e.GetPosition(ctlIma).Y*(imaResult.Height / ctlIma.ActualHeight));
            if (pY < imaResult.Height)
            {

                for (int l = 0; l < imaResult.Width; l++)
                    for (int dis = 0; dis < dim; dis++)
                        if (Dis3D[pY, l, dis]>mayor) { mayor = Dis3D[pY, l, dis]; }
                conv = 255 / mayor;

                for (int l = 0; l < imaResult.Width; l++)
                {
                    imaAux.Data[pY, l, 0] = 255;
                    for (int dis = 0; dis < dim; dis++)
                    {
                        imaDis.Data[dis, l, 0] = (byte)(Dis3D[pY, l, dis] *conv);
                        disM[dis, l] = Dis3D[pY, l, dis];


                    }
                }
                ctlIma.Source = ToBitmapSource(imaAux);
                CvInvoke.Imshow("Disparidad", imaDis);
            }
            
        }
        #region "Fuciones Basicas"
        public void def()//default
        {
            if (dec)
            {
                gridRes.Visibility = Visibility.Collapsed;
                gridLyR.Visibility = Visibility.Visible;
                ctlImaLefh.Source = ToBitmapSource(imaLeft);
                ctlImaRight.Source = ToBitmapSource(imaRight);
                dec = false;
            }
            else
            {
                gridRes.Visibility = Visibility.Visible;
                gridLyR.Visibility = Visibility.Collapsed;
                ctlIma.Source = ToBitmapSource(imaResult);
                ctlImaLefh2.Source = ToBitmapSource(imaLeft);
                ctlImaRight2.Source = ToBitmapSource(imaRight);
                dec = true;

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
        OpenFileDialog fileIma = new OpenFileDialog();
        private void btnOpenImagenLeft_Click(object sender, RoutedEventArgs e)
        {

            if (fileIma.ShowDialog() == true)
            {
                imaLeft = null;
                imaLeft = contrasteEcualizacion(new Image<Rgb, byte>(fileIma.FileName).Convert<Gray, byte>());               
                
                gridRes.Visibility = Visibility.Collapsed;
                gridLyR.Visibility = Visibility.Visible;
                ctlImaLefh.Source = ToBitmapSource(imaLeft);
                ctlImaRight.Source = ToBitmapSource(imaRight);
                if(dec) dec = false;

            }

        }

        private void btnOpenImagenRight_Click(object sender, RoutedEventArgs e)
        {
            if (fileIma.ShowDialog() == true)
            {
                imaRight = null;
                imaRight = contrasteEcualizacion(new Image<Rgb, byte>(fileIma.FileName).Convert<Gray, byte>());

                gridRes.Visibility = Visibility.Collapsed;
                gridLyR.Visibility = Visibility.Visible;
                ctlImaLefh.Source = ToBitmapSource(imaLeft);
                ctlImaRight.Source = ToBitmapSource(imaRight);
                if (dec) dec = false; 
            }

        }
        #endregion


        #region "BackgroundWorker Metódos"
        private void bgw_ProcessC(object sender, ProgressChangedEventArgs e)
        {
            int n = e.ProgressPercentage * 100 / (imaResult.Height);
            txtP.Text = n.ToString() + '%';
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))
                MessageBox.Show("Error: " + e.Error.Message);
            else
            {
                def();
                txtP.Text = "100" + '%';

            }
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            
            SSD(imaLeft, imaRight, 60, 7);
        }
        #endregion
        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if(!dec)
            run();
        }
        private void run()
        {

            if (bgw.IsBusy != true)            
                bgw.RunWorkerAsync();
            
         

        }
        
        private Image<Gray, byte> contrasteEcualizacion(Image<Gray, byte> Ima)//Metodo
        {
            int[] H = new int[256];
            //genera el arreglo del histograma 0 255
            for (int i = 0; i < H.Length; i++) H[i] = 0;
            for (int i = 0; i < Ima.Height; i++)//recorre en vertical
                for (int j = 0; j < Ima.Width; j++)//recorre en horizontal                
                    H[Ima.Data[i, j, 0]]++;

            //Ajuste de la imagen
            for (int i = 0; i < Ima.Height; i++)//recorre en vertical
                for (int j = 0; j < Ima.Width; j++)//recorre en horizontal                
                    Ima.Data[i, j, 0] = (byte)(fU(Ima.Data[i, j, 0], H) * 255 / (Ima.Width * Ima.Height));

            return Ima;
        }
        private int fU(int k, int[] H)//Este metodo pertenece al metodo "contrasteEcualizacion"
        { //distribucion de probabilidad acomulado                      
            int sum = 0;
            for (int i = 0; i <= k; i++)
                sum += H[i];
            return sum;
        }
        //---------------
        private Image<Gray, byte> contrasteLineal(Image<Gray, byte> Ima)//Metodo1
        {
            byte minV = 255, maxV = 0;
            //ciclos para obtener los valores minimos y maximos
            for (int i = 0; i < Ima.Height; i++)//recorre en vertical            
                for (int j = 0; j < Ima.Width; j++)//recorre en horizontal
                {
                    if (Ima.Data[i, j, 0] > maxV)
                        maxV = Ima.Data[i, j, 0];
                    if (Ima.Data[i, j, 0] < minV)
                        minV = Ima.Data[i, j, 0];
                }

            for (int i = 0; i < Ima.Height; i++)//recorre en vertical
                for (int j = 0; j < Ima.Width; j++)//recorre en horizontal   
                {
                    if ((maxV - minV) != 0)
                        Ima.Data[i, j, 0] = (byte)(255 * ((Ima.Data[i, j, 0] - minV)) / (maxV - minV));
                    else
                        Ima.Data[i, j, 0] = 255;
                }

            return Ima;
        }
        //---------------
        private Image<Gray, byte> filtroMedia(Image<Gray, byte> Ima)
        {

            System.Drawing.Bitmap b = Ima.Bitmap;
            Image<Gray, byte> imaAux = new Image<Gray, byte>(b);

            int tam = 3, k;
            double sum = 0;

            for (int i = 0; i < Ima.Height; i++)//recorre en vertical     
            {
                for (int j = 0; j < Ima.Width; j++)//recorre en horizontal                                   
                {
                    sum = 0; k = 0; 
                    for (int x = i - (int)(tam / 2); x <= i + (int)(tam / 2); x++)
                        for (int y = j - (int)(tam / 2); y <= j + (int)(tam / 2); y++)
                            if ((x >= 0 && x < Ima.Height) && (y >= 0 && y < Ima.Width))
                            {
                                sum += Ima.Data[i, j, 0];
                                k++;
                            }
                    sum /= k;
                         imaAux.Data[i, j, 0] = (byte)(sum);

                }
                bgw.ReportProgress(i);
            }

            return imaAux;

        }

        //============SSD===============
        //Sum of Squared Dierences
        private double[,,] Dis3D;
        private void SSD(Image<Gray, byte> Left, Image<Gray, byte> Right,int d, int dMascara)
        {
            Dis3D = new double[Left.Height, Left.Width, d + 1];

            double Min = 10000; int auxDis=0;           

            if (dMascara % 2 == 0) dMascara++;
            for (int y = 0; y <Left.Height; y++)
            {
                for (int x = 0; x <Left.Width; x++)
                {
                    Min = 10000;
                    for (int dis = 0; dis <= d; dis++)
                    {
                        Dis3D[y, x, dis] = imagenIntegral(Left, Right, y, x, dis, dMascara);
                        if (Dis3D[y, x, dis] < Min )
                        {
                            Min = Dis3D[y, x, dis];
                            auxDis = dis;
                        }
                    }                   
                    
                    imaResult.Data[y, x, 0] = (byte)(auxDis);                   


                }
                bgw.ReportProgress(y);
            }

            //imaResult = contrasteLineal(filtroMedia(imaResult));
            imaResult = contrasteLineal(imaResult);
            //   imaResult = filtroMedia(imaResult);

        }
        private double imagenIntegral(Image<Gray, byte> Left, Image<Gray, byte> Right, int y, int x,int d, int dMascara)//dMascara es dimension de la sub ventanta
        {
            double sum = 0;

            for (int n = y-(int)(dMascara/2); n <= y + (int)(dMascara / 2); n++)            
                for (int m = x-(int)(dMascara/2); m <= x + (int)(dMascara / 2); m++)
                {
                    if (((n >= 0 && n < Left.Height) && (m >= 0 && m < Left.Width)) && ((n >= 0 && n < Right.Height) && (m - d >= 0 && m - d < Right.Width)))
                        sum += Math.Pow((Left.Data[n,m,0] - Right.Data[n,m-d,0]),2);
                    
                }

            return sum / (Math.Pow(dMascara, 2));
        }

        
    }
}
