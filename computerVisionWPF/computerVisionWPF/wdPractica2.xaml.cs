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

namespace computerVisionWPF
{
    /// <summary>
    /// Lógica de interacción para wdPractica2.xaml
    /// </summary>
    public partial class wdPractica2 : UserControl
    {
        private Image<Bgr, byte> imaC = new Image<Bgr, byte>(Environment.CurrentDirectory + @"\Imagenes\P2\lena.jpg"); //lee la imagen por default a color       
        private Image<Gray, byte> imaG; //sirve para guardar la vercion a escala de grises de la imagen a color
        private Image ImaO = new Image();// Un componente grafico tipo imagen
        public wdPractica2()
        {
            InitializeComponent();

            ImaO.Source = ToBitmapSource(imaC.Convert<Gray,byte>());//Para ver la imagen en scala de grises en inicio            
            
            ImaO.MouseDown += new MouseButtonEventHandler(MouseI);//para que reaccione cuando se da click en la imagen
            efectoLampara(200, 200);//puntos de inicio donde aparecera el centro de la lampara          

            gridIma.Children.Add(ImaO);//agrega el componete a la interfaz            
            
        }
        private void efectoLampara(int x, int y)//método para hacer el efecto lampara en cualquier punto en cualquier imagen
        {
            int h;//radio del circulo

            imaG = imaC.Convert<Gray, byte>();
            for (int i = 0; i < imaC.Height; i++)//recore en vertical
            {

                for (int j = 0; j < imaC.Width; j++)//recore en horizontal
                {

                    h = 155 - (int)(Math.Abs(Math.Sqrt(Math.Pow((j - x), 2) + Math.Pow(i - y, 2))) * 2);
                    if (imaG.Data[i, j, 0] +  h <= 255)
                    {
                        if (imaG.Data[i, j, 0] +  h >= 0)
                            imaG.Data[i, j, 0] = (byte)(imaG.Data[i, j, 0] + h);
                        else imaG.Data[i, j, 0] = 0;

                    }
                    else imaG.Data[i, j, 0] = 255;



                }

            }
            ImaO.Source = ToBitmapSource(imaG);
        }
        private void MouseI(object sender, MouseButtonEventArgs e)
        {
            Size s = ImaO.RenderSize;//Obtiene la el tamaño de la componente tipo de imagen
            efectoLampara((int)(e.GetPosition(this).X *(imaC.Width/s.Width)), (int)(e.GetPosition(this).Y* (imaC.Height /s.Height)));//establece proporcionalmente la pocicion de el mouse       
        }
        
        private void OpenImagen_Click(object sender, RoutedEventArgs e)//metodo para cargar cualquier imagen
        {
            OpenFileDialog OpenIma = new OpenFileDialog();
            if(OpenIma.ShowDialog()==true){
                imaC = null;
                imaC = new Image<Bgr, byte>(OpenIma.FileName);           
                efectoLampara(100,100);
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
    }
}
