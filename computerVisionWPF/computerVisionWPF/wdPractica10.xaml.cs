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
    /// Lógica de interacción para wdPractica10.xaml
    /// </summary>
    public partial class wdPractica10 : UserControl
    {
        Image<Rgb, byte> ima = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\P10\Imagen1.png");
        Image<Gray, byte> imaO, imaMean, imaVariance, imaEnergy, imaCorrelation,  imaEntropy, imaContrast, imaHomogeneity;

        BackgroundWorker bgw = new BackgroundWorker();

        public wdPractica10()
        {

            InitializeComponent();
            imaO = new Image<Gray, byte>(ima.Convert<Gray, byte>().Bitmap);
            ctlIma.Source = ToBitmapSource(imaO);

            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            bgw.ProgressChanged += new ProgressChangedEventHandler(bgw_ProcessC);
            bgw.WorkerReportsProgress = true;

        }
        #region "BackgroundWorker Metódos"
        private void bgw_ProcessC(object sender, ProgressChangedEventArgs e)
        {
            int n = e.ProgressPercentage * 100 / (imaO.Height);
            txtP.Text = n.ToString() + '%';
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))
                MessageBox.Show("Error: " + e.Error.Message);
            else
            {
                CvInvoke.Imshow("Mean", imaMean);
                CvInvoke.Imshow("Variance", imaVariance);
                CvInvoke.Imshow("Energy", imaEnergy);                
                CvInvoke.Imshow("Correlation", imaCorrelation);
                CvInvoke.Imshow("Entropy", imaEntropy);
                CvInvoke.Imshow("Contrast", imaContrast);
                CvInvoke.Imshow("Homogeneity", imaHomogeneity);//Imagen Homogenuidad
                txtP.Text = "100" + '%';

            }
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            textureAnalysis();
        }
        #endregion
        #region "Fuciones Basicas"
        private void btnOpenImagen_Click(object sender, RoutedEventArgs e)
        {
            txtP.Text = "0" + '%';
            OpenFileDialog fileIma = new OpenFileDialog();
            if (fileIma.ShowDialog() == true)
            {
                imaO = null;
                imaO = new Image<Rgb, byte>(fileIma.FileName).Convert<Gray, byte>();
                ctlIma.Source = ToBitmapSource(imaO);
            }
        }
        /*
        private void btnOriginal_Click(object sender, RoutedEventArgs e)
        {
            ctlIma.Source = ToBitmapSource(imaO);
        }*/
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
        #endregion

        public int tam;//Es es tamaño de la nueva imagen integral
        double[] arrPs = new double[511]; //Ps
        double[] arrPd = new double[511]; //Pd

        public void textureAnalysis()
        {
            imaMean = new Image<Gray, byte>(imaO.Width, imaO.Height);
            imaVariance = new Image<Gray, byte>(imaO.Width, imaO.Height);
            imaEnergy = new Image<Gray, byte>(imaO.Width, imaO.Height);            
            imaCorrelation = new Image<Gray, byte>(imaO.Width, imaO.Height);
            imaEntropy = new Image<Gray, byte>(imaO.Width, imaO.Height);
            imaContrast = new Image<Gray, byte>(imaO.Width, imaO.Height);
            imaHomogeneity = new Image<Gray, byte>(imaO.Width, imaO.Height);
            //Estableciendo el tamaño de la imagen integral
            tam = (int)(imaO.Width * 0.1);
            if (tam % 2 == 0) tam++;

            tam = 15;
            int N = tam * tam;

            for (int h = 0; h < imaO.Height; h++)
            {
                for (int w = 0; w < imaO.Width; w++)
                {
                    imagenIntegral(w, h, N);

                }
                bgw.ReportProgress(h);
            }          
        }

    

        public void imagenIntegral(int x, int y, int N)//Representación intermedia de la imagen
        {
                      
            histogramaSumasYRestas(x, y);//Metodo
              
                        
            double sumPs = 0, sumPd = 0;
            double nPs = 0, nPd = 0;//N para cada histograma
            double sumU = 0;//Media                        
            double sumEPs = 0, sumEPd = 0;//Entropy
            double sumC = 0;//Contrast
            double sumH = 0;//Caracteristica Homogeneity


            for (int i = 0; i <= 510; i++)
            {
                nPs += arrPs[i];
                nPd += arrPd[i];
            }

            for (int i = 0; i <=510; i++)
            {
                //Normalización
                arrPs[i] =  (arrPs[i] / nPs);
                arrPd[i] = (arrPd[i] / nPd);

                //Media/Mean
                sumU += i * arrPs[i];                
                //energy feature              
                sumPs += arrPs[i]* arrPs[i];
                sumPd += arrPd[i]* arrPd[i];
                //Entropy
                if (arrPs[i] != 0)                
                    sumEPs += arrPs[i] * Math.Log10(arrPs[i]);
                if (arrPd[i] != 0)
                    sumEPd += arrPd[i] * Math.Log10(arrPd[i]);               

                //Contrast
                sumC += (i-255)*(i-255) * arrPd[i];
                //Homogeneity feature
                sumH +=(1 / (1+(i-255)*(i-255))) * arrPd[i];
            }
            sumU = sumU / 2;
            double sumVyRPs = 0, sumVyRPd=0; //Variance y Correlation            
            for (int i = 0; i <= 510; i++)
            {
                //Variance y Correlation
                sumVyRPs += Math.Pow((i - 2 * sumU), 2) * arrPs[i];
                sumVyRPd += Math.Pow(i - 255, 2) * arrPd[i];
            }

            imaMean.Data[y, x, 0] = (byte)(sumU);
            imaVariance.Data[y, x, 0] = (byte)(((sumVyRPs + sumVyRPd) /2)/255);
            imaCorrelation.Data[y, x, 0] = (byte)(((sumVyRPs - sumVyRPd) / 2) / 255);
            imaEnergy.Data[y, x, 0] = (byte)((sumPs * sumPd) * (Math.Log10(510*2)) *255);            
            imaEntropy.Data[y, x, 0] = (byte)(((-sumEPs-sumEPd)/ Math.Log10(nPd*nPs)) *255);
            imaContrast.Data[y, x, 0] = (byte)(sumC/255);
            imaHomogeneity.Data[y, x, 0] = (byte)(sumH*255);
            

        }


        public void histogramaSumasYRestas(int x, int y)
        {
            arrPs.DefaultIfEmpty(); //Ps
            arrPd.DefaultIfEmpty(); //Pd
            for (int i = y - (int)tam / 2; i <= y + (int)tam / 2; i++)
                for (int j = x - (int)tam / 2; j <= x + (int)tam / 2; j++)
                {


                    if ((i >= 0 && j >= 0) && (i < imaO.Height && j < imaO.Width))
                        for (int k = -1; k < 3; k++)
                        {
                            if (k < 2)
                            {
                                if (i - 1 >= 0 && (j - 1 >= 0 && j + 1 < imaO.Width))
                                {
                                    arrPs[imaO.Data[i, j, 0] + imaO.Data[i - 1, j + k, 0]]++;
                                    arrPd[imaO.Data[i, j, 0] - imaO.Data[i - 1, j + k, 0] + 255]++;
                                }
                            }
                            else
                            {
                                if (j + 1 < imaO.Width)
                                {
                                    arrPs[imaO.Data[i, j, 0] + imaO.Data[i, j + 1, 0]]++;
                                    arrPd[imaO.Data[i, j, 0] - imaO.Data[i, j + 1, 0] + 255]++;
                                }
                            }
                        }
                }

        }
        private void btnFeactures_Click(object sender, RoutedEventArgs e)
        {
            if (bgw.IsBusy != true)
            {
                bgw.RunWorkerAsync();
            }

        }


    }
}
