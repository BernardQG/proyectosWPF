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
    /// Lógica de interacción para wdProyectoFinal.xaml
    /// </summary>
    public partial class wdProyectoFinal : UserControl
    {
        Image<Gray, byte> imaLeft = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\ProyectoFinal\ggate-l.jpg").Convert<Gray, byte>();
        Image<Gray, byte> imaRight = new Image<Rgb, byte>(Environment.CurrentDirectory + @"\Imagenes\ProyectoFinal\ggate-r.jpg").Convert<Gray, byte>();
        Image<Gray, byte> imaResult;
        Boolean dec = true;
        public wdProyectoFinal()
        {
            InitializeComponent();
            imaResult = new Image<Gray, byte>(imaLeft.Width, imaRight.Height);
            def();
        }
        #region "Fuciones Basicas"
        public void def()//default
        {
            if (dec)
            {
                ctlIma.Visibility = Visibility.Collapsed;
                gridLyR.Visibility = Visibility.Visible;
                ctlImaLefh.Source = ToBitmapSource(imaLeft);
                ctlImaRight.Source = ToBitmapSource(imaRight);
                dec = false;
            }
            else
            {
                ctlIma.Visibility = Visibility.Visible;
                gridLyR.Visibility = Visibility.Collapsed;
                ctlIma.Source = ToBitmapSource(imaResult);
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
                imaLeft = new Image<Rgb, byte>(fileIma.FileName).Convert<Gray, byte>();
                ctlIma.Source = ToBitmapSource(imaLeft);
            }

        }

        private void btnOpenImagenRight_Click(object sender, RoutedEventArgs e)
        {
            if (fileIma.ShowDialog() == true)
            {
                imaRight = null;
                imaRight = new Image<Rgb, byte>(fileIma.FileName).Convert<Gray, byte>();
                ctlIma.Source = ToBitmapSource(imaRight);
            }

        }
        #endregion

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            //def();
            run();
        }
        private void run()
        {
            ctlImaLefh.Source = ToBitmapSource(contrasteEcualizacion(imaLeft));
            ctlImaRight.Source = ToBitmapSource(contrasteEcualizacion(imaRight));
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
        /*
        private void humbral(int h)//Toleracia binaria
        {



            for (int i = 0; i < imaO.Height; i++)//recorre en vertical      

                for (int j = 0; j < imaO.Width; j++)//recorre en horizontal
                {
                    if (imaAux.Data[i, j, 0] > Int32.Parse(txtH.Text)) imaAux.Data[i, j, 0] = 255;
                    else imaAux.Data[i, j, 0] = 0;

                }
        }*/


        private void Set3Ddim(double[,,] Dis3D, int Width, int Height, int dis)
        {
            Dis3D = new double[dis, Height, Width];
            for (int d = 0; d < dis; d++)
                for (int i = 0; i < Height; i++)
                    for (int j = 0; j < Width; j++)
                        Dis3D[d, i, j] = 0xFFFFFFFF;

        }
        private double DesviacionM(Image<Gray, byte> imaAux, int x, int y, int Kernel)
        {
            double n = 0, Sum = 0, SumS = 0;
            int YmasnYMatrix, XmasnXMatrix;
            double W;
            for (int nYMatrix = -Kernel; nYMatrix <= Kernel; nYMatrix++)
            {
                for (int nXMatrix = -Kernel; nXMatrix <= Kernel; nXMatrix++)
                {
                    YmasnYMatrix = y + nYMatrix;
                    XmasnXMatrix = x + nXMatrix;
                    if ((0 <= YmasnYMatrix) && (YmasnYMatrix < imaAux.Height) && (0 <= XmasnXMatrix) && (XmasnXMatrix < Width)
                    && ((Kernel + .5) * (Kernel + .5)) >= (nXMatrix * nXMatrix + nYMatrix * nYMatrix))
                    {
                        W = imaAux.Data[YmasnYMatrix, XmasnXMatrix, 0];
                        Sum += W;
                        SumS += W * W;
                        n++;
                    }
                }
            }
            W = Sum / n;
            return Math.Sqrt(SumS / n - W * W);
        }


        private double Simil(Image<Gray, byte> Left, Image<Gray, byte> Right, int x, int y, int d, int Kernel)
        {
            double Peso, n = 0, dif = 0, Sum = 0;
            int YmasnYMatrix, XmasnXMatrix, XmasnXMatrixD;

            for (int nYMatrix = -Kernel; nYMatrix <= Kernel; nYMatrix++)
            {
                for (int nXMatrix = -Kernel; nXMatrix <= Kernel; nXMatrix++)
                {
                    YmasnYMatrix = y + nYMatrix;
                    XmasnXMatrix = x + nXMatrix;
                    XmasnXMatrixD = nXMatrix + x + d;
                    if ((0 <= YmasnYMatrix) && (YmasnYMatrix < Height) && (0 <= XmasnXMatrix) && (XmasnXMatrix < Width)
                    && (0 <= XmasnXMatrixD) && (XmasnXMatrixD < Width) && ((Kernel + .5) * (Kernel + .5)) >= (nXMatrix * nXMatrix +
                    nYMatrix * nYMatrix))
                    {
                        Peso = 1 / (1 + (nXMatrix * nXMatrix + nYMatrix * nYMatrix) / (Kernel + .5));
                        dif = Peso * (Left.Data[YmasnYMatrix, XmasnXMatrix, 0] - Right.Data[YmasnYMatrix, XmasnXMatrix + d, 0]);
                        Sum += dif * dif;
                        n += Peso;
                    }
                }
            }
            Sum /= n;
            return Sum;
        }

        double SubPixel(int x, int y, int dis, int K, double[,,] Dis3D)
        {
            int m = 2 * K + 1;
            double[,] Data = new double[m, 2];
            for (int i = 0; i < m; i++)
            {
                Data[i, 0] = (dis - (K - i));
                Data[i, 1] = Dis3D[dis - (K - i), y, x];
            }
            double[,] Temp = LeastSqrtMatrix(Data, 2);
            double[] Soluciones = LinearSysSolve(Temp);
            return (-Soluciones[1] / (2 * Soluciones[2]));
        }
        private double[,] LeastSqrtMatrix(double[,] Data, int n)
        {
            double[,] Temp = new double[n + 1, n + 2];
            double sum;
            int m = Data.Length;
            for (int i = 0; i <= 2 * n; i++)
            {
                sum = 0;
                for (int j = 0; j < m; j++)
                {
                    sum += Math.Pow(Data[j, 0], i);
                }
                for (int j = i; j >= 0; j--)
                {
                    if (j <= n && (i - j) <= n)
                    {
                        Temp[j, i - j] = sum;
                    }
                }
            }
            for (int i = 0; i <= n; i++)
            {
                sum = 0;
                for (int j = 0; j < m; j++)
                {
                    sum += Data[j, 1] * Math.Pow(Data[j, 0], i);
                }
                Temp[i, n + 1] = sum;
            }
            return Temp;
        }

        private double[] LinearSysSolve(double[,] Matrix)
        {
            double[] Temp = new double[Matrix.GetLength(0)];
            int p;
            double sum;
            for (int i = 0; i < Matrix.GetLength(1) - 1; i++)
            {
                p = 0;
                for (int j = i; j < Matrix.GetLength(0); j++)
                {
                    p = j;
                    if (Matrix[p, i] != 0) break;
                }
                /*
                if (p != i)
                {
                    swap(Matrix[p], Matrix[i]);
                }*/
                for (int j = i + 1; j < Height; j++)
                {
                    int m = (int)(Matrix[j, i] / Matrix[i, i]);
                    for (int n = 0; n < Width; n++)
                    {
                        Matrix[j, n] = Matrix[j, n] - m * Matrix[i, n];
                    }
                }
            }
            Temp[Matrix.GetLength(0) - 1] = Matrix[Matrix.GetLength(0) - 1, Matrix.GetLength(1) - 1] / Matrix[Matrix.GetLength(0) - 1, Matrix.GetLength(1) - 2];
            for (int i = Matrix.GetLength(0) - 2; i >= 0; i--)
            {
                sum = 0;
                for (int j = i + 1; j <= Height - 1; j++)
                {
                    sum += Matrix[i, j] * Temp[j];
                }
                Temp[i] = (Matrix[i, Matrix.GetLength(1) - 1] - sum) / Matrix[i, i];
            }
            return Temp;
        }
        //------------------------------------------------------

        private double[,] SteroMatch(Image<Gray, byte> Left, Image<Gray, byte> Right, int dismin, int dismax, int K)
        {
            double S = 0;
            int Width, Height, Kt = 0;
            Height = Left.Height;
            Width = Left.Width;
            double[,,] Dis3D = new double[0, 0, 0];
            int[,] DisMap = new int[imaResult.Height, imaResult.Width];
            double[,] SubDisMap = new double[imaResult.Height, imaResult.Width]; ;
            int Th_s = 20;
            Set3Ddim(Dis3D, Width, Height, 1 + dismax - dismin);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Kt = K;
                    do
                    {
                        S = DesviacionM(Left, x, y, Kt);
                        if (S < Th_s) Kt++;
                    } while (S < Th_s);
                    for (int d = dismin; d <= dismax; d++)
                    {
                        if (x + d >= 0 && x + d < Width)
                        {
                            Dis3D[d - dismin, y, x] = Simil(Left, Right, x, y, d, Kt);
                            if ((Dis3D[d - dismin, y, x] < Dis3D[(DisMap[y, x] - dismin), y, x]) || (Dis3D[dismin, y, x] == Dis3D[(DisMap[y, x] - dismin), y, x]) && Math.Abs(d) < Math.Abs(DisMap[y, x]))
                                DisMap[y, x] = d;
                        }
                    }
                }
            }
            //------------Sub-Pixel---------------------------------------
            double SP;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int DIS = DisMap[y, x];
                    if (DIS - 2 > dismin && DIS + 2 < dismax)
                    {
                        SP = SubP(x, y, DIS - dismin, 2, Dis3D);
                        SubDisMap[y, x] = SP;
                    }
                    else
                    {
                        SubDisMap[y, x] = DIS;
                    }
                }
            }
            //------------Filtro iterativo de mediana condicional----------
            bool Cambio = true;
            double[,] SubDisMapF = MedianaIFth2D(SubDisMap, 1, Cambio, dismin, dismax);
            int n = 0;
            do
            {
                SubDisMapF = MedianaIFth2D(SubDisMapF, 2, Cambio, dismin, dismax);
                n++;
            } while (Cambio);
            return SubDisMapF;
        }
        double SubP(int x, int y, int dis, int K, double[,,] Dis3D)//subPixel
        {
            int m = 2 * K + 1;
            double[,] Data = new double[m, 2];
            for (int i = 0; i < m; i++)
            {
                Data[i, 0] = (dis - (K - i));
                Data[i, 1] = Dis3D[(dis - (K - i)), y, x];
            }
            double[,] Temp = LeastSqrtMatrix(Data, 2);
            double[] Soluciones = LinearSysSolve(Temp);
            return (-Soluciones[1] / (2 * Soluciones[2]));
        }
        double[,] MedianaIFth2D(double[,] bMap, int Kernel, Boolean Change, int dismin, int dismax)
        {
            int Height = bMap.GetLength(0);
            int Width = bMap.GetLength(1);
            int YmasnYMatrix, XmasnXMatrix;
            double[,] Temp = new double[Height, Width];
            List<double> Lista = new List<double>();
            double Sum;
            Change = false;
            for (int y = 0; y < Height; y++)
            {
                for (int x = -dismin; x < Width - dismax; x++)
                {
                    Sum = 0;
                    Lista.Clear();
                    for (int nYMatrix = -Kernel; nYMatrix <= Kernel; nYMatrix++)
                    {
                        for (int nXMatrix = -Kernel; nXMatrix <= Kernel; nXMatrix++)
                        {
                            YmasnYMatrix = y + nYMatrix;
                            XmasnXMatrix = x + nXMatrix;
                            if ((0 <= YmasnYMatrix) && (YmasnYMatrix < Height) && (0 - dismin <= XmasnXMatrix) &&
                           (XmasnXMatrix < Width - dismax))
                            {
                                Lista.Add(bMap[YmasnYMatrix, XmasnXMatrix]);
                                Sum += bMap[YmasnYMatrix, XmasnXMatrix];
                            }
                        }
                    }
                    List<double> lista = new List<double>();
                    lista.Clear();
                    for (int i = 0; i < Lista.Count(); i++)
                    {
                        lista.Add(Lista[i]);
                    };
                    lista.Sort();
                    Lista.Clear();
                    lista.Reverse();
                    Lista = lista;


                    double Mediana = Lista[Lista.Count / 2];
                    if (Math.Abs(bMap[y, x] - Lista[Lista.Count / 2]) > 1.5)
                    {
                        Temp[y, x] = Mediana;//(Sum-Buffer[y][x]+Lista[Lista.size()/2])/Lista.size();
                        Change = true;
                    }
                    else
                    {
                        Temp[y, x] = bMap[y, x];
                    }
                }
            }
            return Temp;
        }
    }
}
