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
        Image<Rgb, byte> ima = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\lena.jpg");
        Image<Gray, byte> imaO, imaEnergy;

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
            if(!(e.Error == null))
                MessageBox.Show("Error: " + e.Error.Message);
            else
            {
                CvInvoke.Imshow("Energy", imaEnergy);//Imagen Energia
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
            OpenFileDialog fileIma = new OpenFileDialog();
            if (fileIma.ShowDialog() == true)
            {
                imaO = null;
                imaO = new Image<Rgb, byte>(fileIma.FileName).Convert<Gray, byte>();
                ctlIma.Source = ToBitmapSource(imaO);
            }
        }

        private void btnOriginal_Click(object sender, RoutedEventArgs e)
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
        #endregion

        public int tam;//Es es tamaño de la nueva imagen integral
        double[] arrPs = new double[511]; //Ps
        double[] arrPd = new double[511]; //Pd
        double energy;
        public void textureAnalysis()
        {

            imaEnergy = new Image<Gray, byte>(imaO.Width, imaO.Height);
            //Estableciendo el tamaño de la imagen integral
            tam = (int)(imaO.Width * 0.1);
            if (tam % 2 == 0) tam++;
            
            for (int h = 0; h < imaO.Height; h++)
            {
                for (int w = 0; w < imaO.Width; w++)
                {
                    imagenIntegral(w, h);
                    imaEnergy.Data[h, w, 0] = (byte)(energy * 255);
                }
                bgw.ReportProgress(h);
            }

            
            //ctlIma.Source = ToBitmapSource(imaEnergy);

        }



        public void imagenIntegral(int x, int y)//Representación intermedia de la imagen
        {
                      
            histogramaSumasYRestas(x, y);//Metodo
              
                        
            double sumPs = 0, sumPd = 0;
            double nPs = 0, nPd = 0; 

            for (int i = 0; i <= 510; i++)
            {
                nPs += arrPs[i];
                nPd += arrPd[i];
            }

            for (int i = 0; i <=510; i++)
            {
                //Normalización
                arrPs[i] = arrPs[i] / nPs;
                arrPd[i] = arrPd[i] / nPd;
                //energy feature
                sumPs += arrPs[i] * arrPs[i];
                sumPd += arrPd[i] * arrPd[i];

                

            }
            energy = sumPs * sumPd;
            
        }

        private void btnFeactures_Click(object sender, RoutedEventArgs e)
        {
            if (bgw.IsBusy != true)
            {
                bgw.RunWorkerAsync();
            }

        }

        public void histogramaSumasYRestas(int x, int y)
        {
            arrPs.DefaultIfEmpty(); //Ps
            arrPd.DefaultIfEmpty(); //Pd
            for (int i = y - (int)tam / 2; i < y + (int)tam / 2; i++)
                for (int j = x - (int)tam / 2; j < y + (int)tam / 2; j++)
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

        
    }
}
