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
    /// Lógica de interacción para wdExamen.xaml
    /// </summary>
    public partial class wdExamen : UserControl
    {

        Image<Gray, byte> imaO = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\ima.jpg").Convert<Gray,byte>();
        Image<Gray, byte> imaEntropy,  imaResultado, imaVariance;
        public wdExamen()
        {
            InitializeComponent();
            ctlIma.Source = ToBitmapSource(imaO);
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            bgw.ProgressChanged += new ProgressChangedEventHandler(bgw_ProcessC);
            bgw.WorkerReportsProgress = true;
        }
        BackgroundWorker bgw = new BackgroundWorker();

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
                
                CvInvoke.Imshow("Entropy", imaEntropy);
                CvInvoke.Imshow("Variance", imaVariance);
                
                CvInvoke.Imshow("Resultado", imaResultado);
                txtP.Text = "100" + '%';

            }
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            run();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (bgw.IsBusy != true)
            {
                bgw.RunWorkerAsync();
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

     
        public int tam =17;//Es es tamaño de la nueva imagen integral
        double[] arrPs = new double[511]; //Ps
        double[] arrPd = new double[511]; //Pd

        public void imagenIntegral(int x, int y)//Representación intermedia de la imagen
        {

            histogramaSumasYRestas(x, y);//Metodo


            
            double nPs = 0, nPd = 0;//N para cada histograma            
            double sumEPs = 0, sumEPd = 0;//Entropy            
            
            double sumU = 0;//Media  

            for (int i = 0; i <= 510; i++)
            {
                nPs += arrPs[i];
                nPd += arrPd[i];
            }

            for (int i = 0; i <= 510; i++)
            {
                

                //Normalización
                arrPs[i] = (arrPs[i] / nPs);
                arrPd[i] = (arrPd[i] / nPd);
                sumU += i * arrPs[i];

                //Entropy
                if (arrPs[i] != 0)
                    sumEPs += arrPs[i] * Math.Log10(arrPs[i]);
                if (arrPd[i] != 0)
                    sumEPd += arrPd[i] * Math.Log10(arrPd[i]);

                
                
            }
            sumU = sumU / 2;
            double sumVyRPs = 0, sumVyRPd = 0; //Variance y Correlation   
            for (int i = 0; i <= 510; i++)
            {
                //Variance y Correlation
                sumVyRPs += Math.Pow((i - 2 * sumU), 2) * arrPs[i];
                sumVyRPd += Math.Pow(i - 255, 2) * arrPd[i];
            }

            imaEntropy.Data[y, x, 0] = (byte)(((-sumEPs - sumEPd) / Math.Log10(nPd * nPs)) * 255);
            
            imaVariance.Data[y, x, 0] = (byte)(((sumVyRPs + sumVyRPd) / 2) / 255);


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

        public void textureAnalysis()
        {

            imaEntropy = new Image<Gray, byte>(imaO.Width, imaO.Height);
            imaVariance = new Image<Gray, byte>(imaO.Width, imaO.Height);
            
            //Estableciendo el tamaño de la imagen integral


            for (int h = 0; h < imaO.Height; h++)
            {
                for (int w = 0; w < imaO.Width; w++)
                {
                    imagenIntegral(w, h);

                }
                bgw.ReportProgress(h);
            }
        }

        private void run()
        {
            textureAnalysis();
            imaResultado = new Image<Gray, byte>(imaO.Width, imaO.Height);
            System.Drawing.Bitmap b = imaO.Bitmap;
            imaResultado.Bitmap = b; int  mayor=0;

            for (int h = 0; h < imaO.Height; h++)
            {
                for (int w = 0; w < imaO.Width; w++)
                {
                    if (imaEntropy.Data[h, w, 0] > mayor)
                        mayor = imaEntropy.Data[h, w, 0];
                   

                }
                
            }

            for (int h = 0; h < imaO.Height; h++)
            {
                for (int w = 0; w < imaO.Width; w++)
                {
                    if (!(imaEntropy.Data[h, w, 0] >= mayor-16  && imaVariance.Data[h,w,0]> 5))
                    /*aux = imaEntropy.Data[h, w, 0];
                    aux2 = (aux - imaHomogeneity.Data[h, w, 0]);

                    if (!(aux2 > aux - 10 && aux2 < aux + 10))*/
                        imaResultado.Data[h, w, 0] = 0;

                }

            }
        }

    }
}
