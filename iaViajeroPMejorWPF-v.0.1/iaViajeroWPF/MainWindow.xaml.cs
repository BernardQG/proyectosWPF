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
        List<int> listaAbierta = new List<int>();
        Stack<int> listaCerrada = new Stack<int>();
        int[] C;//arreglo de diferentes ciudades, nombradas por numero
        int cCantidad=5;//almacena el valor de la cantidad de ciudades
        int cBase = 1;//Ciudad punto de inico
        int cMeta = 5;//Ciudad punto de final        

        BackgroundWorker bw = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("\tHola usuario!");
            Console.WriteLine("-------------------------------");
            
            this.Show();
            canvas1.Width = canvas1.ActualHeight;

            // bw.DoWork += (s, a) => { avanzar(); };


        }

        private void avanzar() {
            //Console.WriteLine(">" + listaCerrada.Peek());
            int posicionA=cBase-1, dMenor=10000, aux=0;
            while (listaCerrada.Peek()!=cMeta) {
                dMenor = 10000;
                for (int i=0;i<cCantidad;i++) {
                    if (matDistancia[posicionA,i]!=0 && matDistancia[posicionA, i]<dMenor && revisarSiExiste(i+1)) {
                        aux = i;
                        dMenor = matDistancia[posicionA, i];
                    }
                }
                listaAbierta.Remove(aux+1);
                listaCerrada.Push(aux+1);
                
                posicionA = aux;

            }

            foreach (int item in listaCerrada)
            {
                Console.Write(" " + item);
            }

        }
        private Boolean revisarSiExiste(int n) {
            Boolean existe = false;
            foreach (int item in listaAbierta)
            {
                if (n == item) existe = true;
            }
            return existe;
        }

        private void runn()
        {


            C = new int[cCantidad];
            

            Console.WriteLine("-------------------------------");
            Console.WriteLine("Algoritmo primero mejor");


            //for (int i = 0; i < cCantidad; i++) if ((i + 1) != cBase) { C[k] = (i + 1); k++; }
            listaAbierta.Clear();
            listaCerrada.Clear();
            listaCerrada.Push(cBase);
            for (int i = 0; i < cCantidad; i++) if ((i + 1) != cBase) { listaAbierta.Add(i + 1); }

            Console.WriteLine("\t> Iniciado.");
            Console.Write("\t>\t");
            avanzar();
            ruta();
            
            
            Console.WriteLine("\n\t> Completado.");
            Console.WriteLine("-------------------------------");
            
        }
        //-----------------------------------------------------------------------

        private void ruta()
        {
            int aux=0, i=0;   
            Console.Write("\t Dibujando ruta ");
            foreach (int item in listaCerrada)
            {

                if (i != 0)
                {
                    drawL(new Point(matCiudades[aux, 0], matCiudades[aux, 1]), new Point((matCiudades[item - 1, 0]), (matCiudades[item - 1, 1])), Brushes.Cyan,2);
                }                
                aux = item - 1;
                i++;
            }
        

        

        }
        //-------------------------------------------------------------------------

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
                
                Console.WriteLine("- Ciudad {0}: [{1,3}, {2,3}]", (i + 1), matCiudades[i, 0], matCiudades[i, 1]);
            }
          

            Console.WriteLine("-------------------------------");
            //Calcula la distancia entre dos ciudades
            matDistancia = new int[cCantidad, cCantidad];
            //Console.WriteLine("\nDistancia entre ciudades\n");

            for (int i = 0; i < cCantidad; i++)
            {
                for (int j = 0; j < cCantidad; j++)
                {
                    if (i%2==j%2 || j%4==0 || i%4==0) {

                        matDistancia[i, j] = (int)(Math.Sqrt(Math.Pow(matCiudades[i, 0] - matCiudades[j, 0], 2) + Math.Pow(matCiudades[i, 1] - matCiudades[j, 1], 2)));
                    }
                    else {
                        matDistancia[i, j] = 0;
                    }


                    Console.Write("{0,5}", (int)matDistancia[i, j]);
                }
                Console.WriteLine();
            }
            drawGrafo();

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
            int cO = 1, cM=5, cC = 5;            
           
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

            //--------------------------------------------------
            if (txtCiudadDestino.Text != string.Empty)
            {
                cM = Int32.Parse(txtCiudadDestino.Text);
                if (cM > 0 && cM <= cC)
                    cMeta = cM;
                else cMeta = cC;

            }
            else
                cMeta = cC;

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
                
            }
            else {
                
                this.start = false;
                dEntreCiudades();
                start = true;
                
                
            }
            runn();
        }
        //Brushes.Cyan
        private void drawGrafo() {
            for (int i = 0; i < cCantidad; i++)
            {
                for (int j = 0; j < cCantidad; j++)
                {
                    if(i!=j && matDistancia[i,j]!=0)
                    drawL(new Point(matCiudades[i, 0], matCiudades[i, 1]), new Point((matCiudades[j, 0]), (matCiudades[j, 1])), Brushes.DarkGray,0.2);
                }

            }
            for (int i = 0; i < cCantidad; i++)
            {
                drawP(matCiudades[i, 0], matCiudades[i, 1], i + 1);
            }
        }

        //Dibuja el linea en el camvas
        private void drawL(Point pO, Point pF, Brush b, double sT)
        {
           Line l = new Line();
            
           l.Stroke = b;            
           l.StrokeThickness = sT;
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
