using System;
using System.Collections.Generic;
using System.ComponentModel;
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


namespace iaViajeroWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] matDistancia, matCiudades;
        Boolean start = false;
        //List<int> listaCiudades = new List<int>();
        ulong[,] convinacionesCiudades;//arreglo de rutas posibles
        int[] C;//arreglo de diferentes ciudades, nombradas por numero
        int cCantidad=5;//almacena el valor de la cantidad de ciudades
        int cBase = 1;//Ciudad punto de inico y final
        ulong cantidadGuardad;

        BackgroundWorker bw = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("\tHola usuario!");
            Console.WriteLine("-------------------------------");
            
            this.Show();
            canvas1.Width = canvas1.ActualHeight;

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))            
                txtProgreso.Text = "Error: " + e.Error.Message;           
             else            
                txtProgreso.Text = "Done!";
            
        }

     
        
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            
            recorrido(C, 0);//funcion genera rutas
        }

        private void mejorRuta() {
            ulong p,p2;//pocicion
            ulong sum=0, menor = 10000;
            ulong pMejorRuta=0;//pocicion para conseguir la mejor ruta en la matriz
            for (ulong i = 0; i < cantidadGuardad; i++)               
            {
                sum = 0;
                p = convinacionesCiudades[i, 0] - 1;                
                sum +=(ulong)matDistancia[cBase - 1, p];
                for (int x = 0; x < (cCantidad - 1) - 1; x++)
                {
                    p = convinacionesCiudades[i, x] - 1;
                    p2 = convinacionesCiudades[i, x+1] - 1;
                    sum += (uint)matDistancia[p, p2];
                }
                p = convinacionesCiudades[i, cCantidad-2] - 1;
                sum += (uint)matDistancia[p, cBase - 1];
                if (sum<menor) {
                    menor = sum;
                    pMejorRuta = i;
                }

            }
            ulong m= pMejorRuta;
            Console.Write("\t\tMejor ruta: ");
            //for (ulong m = 0; m < cantidadGuardad; m++)
            //{
                Console.Write(cBase.ToString()+" ");
                p = convinacionesCiudades[m, 0] - 1;
                drawL(new Point(matCiudades[(uint)(cBase - 1), 0], matCiudades[(uint)(cBase - 1), 1]), new Point((uint)(matCiudades[p, 0]), (uint)(matCiudades[p, 1])));
                for (int x = 0; x < (cCantidad - 1) - 1; x++)
                {
                    Console.Write(convinacionesCiudades[m, x].ToString() + " ");
                    p = convinacionesCiudades[m, x] - 1;
                    p2 = convinacionesCiudades[m, x + 1] - 1;
                    drawL(new Point(matCiudades[p, 0], matCiudades[p, 1]), new Point(matCiudades[p2, 0], matCiudades[p2, 1]));

                }
                Console.Write(convinacionesCiudades[m, cCantidad - 2].ToString() + " ");
                Console.Write(cBase.ToString() + " \n\n");
                p = convinacionesCiudades[m, cCantidad - 2] - 1;
                drawL(new Point(matCiudades[p, 0], matCiudades[p, 1]), new Point(matCiudades[cBase - 1, 0], matCiudades[cBase - 1, 1]));
            //System.Threading.Thread.Sleep(2000);
            Console.WriteLine("\t\tDistancia total: {0}", menor);

            //}

        }
        private void recorrido(int[] V, int k)//genera rutas recursivamente
        {
            int au;
            if (k < (cCantidad - 1))
                for (int i = 0; i < (cCantidad - 1); i++)
                {
                    au = V[k]; V[k] = V[i]; V[i] = au;
                    recorrido(V, k + 1);
                    au = V[k]; V[k] = V[i]; V[i] = au;
                }
            else guardarC(V);
        }

        private void guardarC(int[] V)
        {//guarda e imprime las ruta como vector
            Boolean existe = false;
            int ex;
            //el ciclo revisa si existe en el arreglo de rutas la nueva ruta
            for (ulong m = 0; m < cantidadGuardad; m++)
            {
                ex = 0;
                for (int x = 0; x < (cCantidad - 1); x++)
                    if (convinacionesCiudades[m, x] == (uint)V[x])
                    {
                        ex++;
                        if (!(ex < (cCantidad - 1)))
                            existe = true;
                    }
            }

            if (!existe)
            {
                //si no existe a ruta nueva, se agrega al arreglo de rutas
                for (int x = 0; x < (cCantidad - 1); x++)
                {
                    convinacionesCiudades[cantidadGuardad, x] = (uint)V[x];
                    //Console.Write("{0,5}", convinacionesCiudades[cantidadGuardad, x]);
                }
                //Console.WriteLine();
                cantidadGuardad++;
            }        

        }



        private void dEntreCiudades()
        {
            canvas1.Children.Clear();
            Random r = new Random();
            if(!start) matCiudades = new int[cCantidad, 2];
            Console.WriteLine("\n===============================");
            Console.WriteLine("Cordenadas de la ciudades\n");
            Console.WriteLine("> No. ciudades: " + cCantidad);

            //llena las ubicaciones de la ciudades aleatoriamente
            for (int i = 0; i < cCantidad; i++)
            {
                if (!start) {
                    matCiudades[i, 0] = r.Next(20, (int)canvas1.ActualHeight - 20);
                    matCiudades[i, 1] = r.Next(20, (int)canvas1.ActualHeight - 20);
                }
                drawP(matCiudades[i, 0], matCiudades[i, 1], i + 1);
                Console.WriteLine("- Ciudad {0}: [{1,3}, {2,3}]", (i + 1), matCiudades[i, 0], matCiudades[i, 1]);
            }


            Console.WriteLine("-------------------------------");
            //Calcula la distancia entre dos ciudades
            matDistancia = new int[cCantidad, cCantidad];
            Console.WriteLine("\nDistancia entre ciudades\n");

            for (int i = 0; i < cCantidad; i++)
            {
                for (int j = 0; j < cCantidad; j++)
                {
                    matDistancia[i, j] = (int)(Math.Sqrt(Math.Pow(matCiudades[i, 0] - matCiudades[j, 0], 2) + Math.Pow(matCiudades[i, 1] - matCiudades[j, 1], 2)));
                    Console.Write("{0,5}", matDistancia[i, j]);
                }
                Console.WriteLine();
            }
            
        

            
        }

        //-----------------------------------------------------------------------
        //------------Botones----------------------------------------------------
        //Boton para cerrar la aplicacion
        private void close_Click(object sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
        //Boton para mover la ventana      
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) { this.DragMove(); }
        //Botn reordenar la pocicion de las ciudades y calcula las distancias

        int help = 5;
        private void iniciar() {
            int cO = 1, cC = 5;
            cantidadGuardad = 0;
           
            if (txtCantidadDCiurades.Text != string.Empty)
            {
                
                cC = Int32.Parse(txtCantidadDCiurades.Text);
                if (cC!=help) { this.start = false;  help = cC; }
                if (cC < 0) cC = 5;
            }
            else            
                cC = 5;               
            
            //--------------------------------------------------
            if (txtCiudadOrigen.Text != string.Empty)
            {                
                cO = Int32.Parse(txtCiudadOrigen.Text);
                if (cO > 0 && cO <= cC)
                    cBase = cO;
                else cBase = 1;
                
            }
            else
                cBase = 1;

            cCantidad = cC;
        }
        
     
        private void reOdenar_Click(object sender, RoutedEventArgs e)
        {

            iniciar();
            this.start = false;
            dEntreCiudades();
            this.start = true;
        }
        //Botn que genera las diferentes futas
        private void run_Click(object sender, RoutedEventArgs e)
        {

            iniciar();
            if (start)
            {
                
                start = true;
                dEntreCiudades();
                runn();
            }
            else {
                
                this.start = false;
                dEntreCiudades();
                start = true;
                runn();
                
            }
        }
        //Calcula el numero de convinaciones diferentes de las rutas
        private ulong factorialN(uint n)
        {
            if (n > 0) return n * factorialN(n - 1);
            else return 1;
        }

        private void runn() {           

            
            C = new int[cCantidad];
            int k = 0;

            Console.WriteLine("-------------------------------");
            Console.WriteLine("\nGenerar pernutaciones de rutas entre ciudades:\n\t> Iniciado.");
            Console.WriteLine("\t> Pernutaciones calculadas: " + factorialN((uint)cCantidad));
            convinacionesCiudades = new ulong[factorialN((uint)cCantidad), cCantidad];

            for (int i = 0; i < cCantidad; i++)
            {
                if ((i + 1) != cBase) { C[k] = i + 1; k++; }              
            }
           

            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }

            Console.WriteLine("\t> Completado.");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Calculando la ruta mas corta:\n\t> Iniciado.");

            if (cCantidad > 1)
                mejorRuta();
            Console.WriteLine("\t> Completado.");
        }
        //-----------------------------------------------------------------------

        //Dibuja el linea en el camvas
        private void drawL(Point pO, Point pF)
        {
           Line l = new Line();
           l.Stroke = Brushes.Cyan;            
           l.StrokeThickness = 2;
           l.X1 = pO.X+5;
           l.Y1 = pO.Y+5;
           l.X2 = pF.X+5;
           l.Y2 = pF.Y+5;
           
           canvas1.Children.Add(l);
        }

        //Dibuja el punto en el camvas
        private void drawP(int x, int y, int k)
        {
           
            Ellipse e = new Ellipse();
            SolidColorBrush cB = new SolidColorBrush();
           
            if (k == cBase) cB = Brushes.Red;
            else cB = Brushes.Purple;            
            e.Stroke = cB;
            e.Fill = cB;
            
            e.Width = 10;
            e.Height = 10;
            e.Margin = new Thickness(x, y, 0, 0);
            TextBlock txt = new TextBlock();
            txt.Text = k.ToString();
            txt.FontSize = 15;
            txt.Margin = new Thickness(x + 12, y - 5, 0, 0);
            canvas1.Children.Add(e);
            canvas1.Children.Add(txt);
        }
    }
}
