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
using Emgu.CV.UI;
using Emgu.CV.Util;
using Microsoft.Win32;
using System.ComponentModel;

namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdPractica6.xaml
    /// </summary>
    public partial class wdPractica6 : UserControl
    {


        //  Image<Gray, byte> imaO = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\FindWally.jpg").Convert<Gray,byte>();
        //Image<Gray, byte> imaW = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\Wally.jpg").Convert<Gray,byte>();
        Image<Gray, byte> imaO = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\P6\1E.jpg").Convert<Gray, byte>();
        Image<Gray, byte> imaW = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\P6\1O.jpg").Convert<Gray, byte>();
        Image<Gray, byte> imaAux = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\p6\1E.jpg").Convert<Gray, byte>();

        BackgroundWorker bwObj = new BackgroundWorker();
        Boolean b = true;
        public wdPractica6()
        {
            InitializeComponent();
            def();
            bwObj.DoWork += new DoWorkEventHandler(bwObj_DoWork);
            bwObj.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bwObj.ProgressChanged += new ProgressChangedEventHandler(bw_ProcessC);
            bwObj.WorkerReportsProgress = true;
            
            ctlIma.MouseDown += (s, a) => {
                
                if (b) { b = false; ctlIma.Source = ToBitmapSource(imaO); }
                else { b = true; ctlIma.Source = ToBitmapSource(imaAux); }
               
            };

            




        }

        private void bw_ProcessC(object sender, ProgressChangedEventArgs e)
        {
            int n = e.ProgressPercentage * 100 / (imaO.Height);
            txtP.Text = n.ToString()+'%';
        }

        private void bwObj_DoWork(object sender, DoWorkEventArgs e)
        {
            runObjeto();
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))
                MessageBox.Show("Error: " + e.Error.Message);
            else
            {
                ctlIma.Source = ToBitmapSource(imaAux);
                txtP.Text = "100"+'%';
                //MessageBox.Show("Hecho");

            }
            

        }

        private void runObjeto()
        {
            double numerador = 0, denominador = 0, denominador2 = 0;
            System.Drawing.Bitmap b;
            b = imaO.Bitmap;
            imaAux.Bitmap = b;

            double mediaObjeto = mediaImaW();
            double mediaEsenario = mediaImaO();
            
            for (int i = 0; i < imaO.Height; i++)//recorre en vertical      
            {
                for (int j = 0; j < imaO.Width; j++)//recorre en horizontal
                {
                    
                    numerador = denominador = denominador2 = 0;
                    for (int x = 0; x < imaW.Height; x++)
                        for (int y = 0; y < imaW.Width; y++)
                        {
                            if ((i + x - (int)(imaW.Height / 2) >= 0 && i + x - (int)(imaW.Height / 2) < imaO.Height) && (j + y - (int)(imaW.Width / 2) >= 0 && j + y - (int)(imaW.Width / 2) < imaO.Width))
                            {
                                numerador += (imaO.Data[i + x - (int)(imaW.Height / 2), j + y - (int)(imaW.Width / 2), 0] - mediaEsenario) * (imaW.Data[x, y, 0] - mediaObjeto);
                                denominador += Math.Pow(imaO.Data[i + x - (int)(imaW.Height / 2), j + y - (int)(imaW.Width / 2), 0] - mediaEsenario, 2);
                                denominador2 += Math.Pow(imaW.Data[x, y, 0] - mediaObjeto, 2);
                            }
                        }
                   imaAux.Data[i, j, 0] = (byte)Math.Abs((numerador / Math.Sqrt(denominador * denominador2)) * 255);

                  //  if (((numerador / Math.Sqrt(denominador * denominador2))) > 0.6) imaAux.Data[i, j, 0] = 255;
                   // else imaAux.Data[i, j, 0] = 0;

                    bwObj.ReportProgress(i);
                }
                
            }
        }

  


     
        private double mediaImaO() {
            double sum=0;
            for (int i = 0; i < imaO.Height; i++)//recorre en vertical            
                for (int j = 0; j < imaO.Width; j++)//recorre en horizontal               
                    sum += imaO.Data[i,j,0];

            return sum / (imaO.Height* imaO.Width);
        }

        private double mediaImaW()
        {
            double sum = 0;
            for (int i = 0; i < imaW.Height; i++)//recorre en vertical            
                for (int j = 0; j < imaW.Width; j++)//recorre en horizontal               
                    sum += imaW.Data[i, j, 0];

            return sum / (imaW.Height * imaW.Width);
        }

        //---------------------------------------------------------------------------------------------------
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
        private void def()//permite reiniciar los valores con nuevas imagenes, ademas de correr el algoritmo de la practica
        {
            txtP.Text = "0" + '%';
            ctlIma.Source = ToBitmapSource(imaO);
            ctlImaW.Source = ToBitmapSource(imaW);
            
        }
        OpenFileDialog OpenIma = new OpenFileDialog();
        private void btnOpenImagen_Click(object sender, RoutedEventArgs e)
        {
            
            if (OpenIma.ShowDialog() == true)
            {
                imaO = null;
                imaO = new Image<Rgb, byte>(OpenIma.FileName).Convert<Gray,byte>();
                def();
            }
        }

        private void btnOpenImagenW_Click(object sender, RoutedEventArgs e)
        {
            if (OpenIma.ShowDialog() == true)
            {
                imaW = null;
                imaW = new Image<Rgb, byte>(OpenIma.FileName).Convert<Gray, byte>();
                def();
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (bwObj.IsBusy != true)
            {
                bwObj.RunWorkerAsync();
            }
            
        }

       
    }
}
