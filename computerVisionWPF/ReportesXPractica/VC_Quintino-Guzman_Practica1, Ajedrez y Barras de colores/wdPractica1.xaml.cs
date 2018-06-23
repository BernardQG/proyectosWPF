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
    /// Lógica de interacción para wdPractica1.xaml
    /// </summary>
    public partial class wdPractica1 : UserControl
    {
        private Image imagenBase = new Image();//crea un componente tipo imagen
        private Boolean Unouotro = true;//Para poder cambiar de la imagen de ajedrez a la de colores
        public wdPractica1()
        {
            InitializeComponent();
            imagenBase.Width = imagenBase.Height = 512;//asigna el tamaño de la imagen
            imagenBase.Cursor = Cursors.Hand;//permite cambiar el cursor cuando esta sobre la imagen, la cambia a una mano
            imagenBase.MouseDown += (s, a) => {//crea un evento del tipo mousedown y asigna su comportamiento
                if (Unouotro) { Unouotro = false; imaColores();  }
                else { Unouotro = true; imaAjedrez(); }

            };
            imaAjedrez();//por default inicia con la imagen de ajedres
        }
        

        private void imaColores() {//metodo para crear la imagen con colores
            Image<Rgb, byte> imagenModificada = new Image<Rgb, byte>(512, 512);
            bool dr, dg, db;
            dr = dg = db = false;            
            byte  r, g, b;
            r = g = b = 255;
            
            for (int i = 0; i < imagenModificada.Height; i++)//recore en vertical
            {
                
                for (int j = 0; j < imagenModificada.Width; j++)//recore en horizontal
                {

                    if (j % 64 == 0 && j != 0)
                    {                                           
                       if (db) {
                            db = false; b = 255;
                            if (dr)
                            {
                                dr = false; r = 255;
                                if (dg)
                                {
                                    dg = false; g = 255;

                                }
                                else { dg = true; g = 0; }
                            }
                            else { dr = true; r = 0; }
                        }
                        else { db = true; b = 0; }
                        
                        
                    }



                    imagenModificada.Data[i, j, 0] = r;
                    imagenModificada.Data[i, j, 1] = g;
                    imagenModificada.Data[i, j, 2] = b;
                }
                dr = dg = db = false;
                r = g = b = 255;



            }


            imagenBase.Source = ToBitmapSource(imagenModificada);
            gridP.Children.Clear();
            gridP.Children.Add(imagenBase);
        }
        private void imaAjedrez()//metodo para crear la imagen de ajedres
        {
            Boolean siono = true;
            Image<Gray, byte> imagenModificada = new Image<Gray, byte>(512, 512);
            int k = 1;
            byte valuePixel = 255;
            for (int i = 0; i < imagenModificada.Height; i++)
            {
                for (int j = 0; j < imagenModificada.Width; j++)
                {
                    if (j % 64 == 0 && j != 0)
                    {
                        if (valuePixel == 0)
                            valuePixel = 255;
                        else valuePixel = 0;
                    }
                    imagenModificada.Data[i, j, 0] = valuePixel;
                }

                
                if (i % 64 == 0 && i != 0)
                {
                    if (k % 2 == 0) siono = true;
                    else siono = false;

                    k++;
                }
                if (siono) valuePixel = 255;
                else valuePixel = 0;

            }


            imagenBase.Source = ToBitmapSource(imagenModificada);
            gridP.Children.Clear();
            gridP.Children.Add(imagenBase);
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
