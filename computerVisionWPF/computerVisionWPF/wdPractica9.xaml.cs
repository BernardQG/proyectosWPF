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
using Emgu.Util;
using System.ComponentModel;

namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdPractica9.xaml
    /// </summary>
    public partial class wdPractica9 : UserControl
    {
        Image<Rgb, byte> ima = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\rec.jpg");
        Image<Gray, byte> imaO = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\rec.jpg").Convert<Gray, byte>();
        Image<Gray, byte> imaAux = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\rec.jpg").Convert<Gray, byte>();
        Image<Gray, byte> Grafica;


        BackgroundWorker bwObj = new BackgroundWorker();

        public wdPractica9()
        {
            InitializeComponent();
            bwObj.DoWork += new DoWorkEventHandler(bwObj_DoWork);
            bwObj.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bwObj.ProgressChanged += new ProgressChangedEventHandler(bw_ProcessC);
            bwObj.WorkerReportsProgress = true;
            def();
        }
        private void def()
        {

            ctlIma.Source = ToBitmapSource(imaO);


        }
        private void bw_ProcessC(object sender, ProgressChangedEventArgs e)
        {
            int n = e.ProgressPercentage * 100 / (imaO.Height);
            txtP.Text = n.ToString() + '%';
        }

        private void bwObj_DoWork(object sender, DoWorkEventArgs e)
        {
            acumulador();
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))
                MessageBox.Show("Error: " + e.Error.Message);
            else
            {
                ctlIma.Source = ToBitmapSource(imaAux);
                
               MessageBox.Show("Hecho");

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
        int ln;
        private void btnRectas_Click(object sender, RoutedEventArgs e)
        {
            
            if (txtL.Text == string.Empty) ln = 1;
            else ln = Int32.Parse(txtL.Text);

            if (bwObj.IsBusy != true)
            {
                bwObj.RunWorkerAsync();
            }
        }

        public int[,] acm;
        private void acumulador()
        {

            // Max = Math.Sqrt(Math.Pow(imaO.Width, 2) + Math.Pow(imaO.Height, 2));           
            int rho_value, theta_value = 180;
            // int numP=0;//Numero de puntos para que se considere linea

            System.Drawing.Bitmap b = imaO.Bitmap;
            imaAux.Bitmap = b;


            acm = new int[(int)(Math.Abs(Math.Sqrt(Math.Pow(imaO.Width, 2) + Math.Pow(imaO.Height, 2)))), theta_value];//Mascara
                                                                                                                       //int [] moda = new int[acm.GetLength(0)*acm.GetLength(1)];//Acumulador general


            for (int i = 0; i < acm.GetLength(0); i++)
                for (int j = 0; j < acm.GetLength(1); j++)
                    acm[i, j] = 0;

            // realizar transformacion lineal para cada pixel 

            for (int i = 0; i < imaO.Height; i++)
            {
                for (int j = 0; j < imaO.Width; j++)
                {
                    // iterar para todo el rango de angulos 
                    if (imaO.Data[i, j, 0] > 0)
                        for (int t = 0; t < theta_value; t++)
                        {

                            rho_value = (int)Math.Round(j * Math.Cos((Math.PI / 180) * t) + i * Math.Sin((Math.PI / 180) * t));
                            acm[Math.Abs(rho_value), Math.Abs(t)]++;
                        }
                }
            }
            //-------------------------

            //Grafica = new Image<Gray, byte>(acm.GetLength(1), acm.GetLength(0));

            /*
           for (int i = 0; i < acm.GetLength(0); i++)
                for (int j = 0; j < acm.GetLength(1); j++)
                {
                    
                    if (acm[i, j] > 0)
                    {
                        Grafica.Data[i, j, 0] = 255;                                            
                    }
                    else
                        Grafica.Data[i, j, 0] = 0;
                }
            medio=(int)arrAux.Max();*/
            //  CvInvoke.Imshow("Grafica", Grafica);
            //MessageBox.Show("Medio:"+medio.ToString());

            //-------------------------

            




            for (int l = 0; l < ln; l++)
            {
                lineasBuenas();
                for (int i = 0; i < imaO.Height; i++)
                {
                    for (int j = 0; j < imaO.Width; j++)
                    {

                        int y = (int)(Math.Round((rho - j * Math.Cos((Math.PI / 180) * theha)) / Math.Sin((Math.PI / 180) * theha)));
                        if (y == i)
                            imaAux.Data[i, j, 0] = 255;

                        int x = (int)(Math.Round((rho - i * Math.Sin((Math.PI / 180) * theha)) / Math.Cos((Math.PI / 180) * theha)));
                        if (x == j)
                            imaAux.Data[i, j, 0] = 255;
                    }

                }
                bwObj.ReportProgress(l);
            }
              
         

        }

        int theha, rho;
        private void lineasBuenas()
        {
            int max=0;
            int[] arrAux = new int[imaO.Width * imaO.Height];//Acumulador general
            int k = 0;
            for (int i = 0; i < acm.GetLength(0); i++)
                for (int j = 0; j < acm.GetLength(1); j++)
                    if (acm[i, j] > max)
                        max = acm[i, j];

            for (int r = 0; r < acm.GetLength(0); r++)
            {
                for (int t = 0; t < acm.GetLength(1); t++)
                {
                    if (acm[r, t] == max)
                    {                        
                        theha = t;
                        rho = r;
                        acm[r,t]= (int)max/ 4;                        

                    }
                }
                
            }
        }

        //end
    }
}
