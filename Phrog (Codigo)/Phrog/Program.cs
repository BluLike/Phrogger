using System;
using System.Collections;
using System.Threading;
using System.Linq;

namespace Phrog
{
    class MainClass
    {
        public static void Main()
        {
            Console.SetWindowSize(79, 24);
            Console.SetBufferSize(79, 24);
            Console.CursorVisible = false;


            Bienvenida bienvenida = new Bienvenida();
            Reglas reglas= new Reglas();
            Partida partida = new Partida();
            Fin fin = new Fin();
            int pantalla = 0;

            //Este loop sirve para que el jugador vuelva al menú principal despues de abnadonar la partida o morir
            do
            {
                bienvenida.Lanzar();
                if (!bienvenida.Salir)
                {
                    reglas.Lanzar();

                    do
                    {
                        pantalla++;
                        partida.Lanzar();

                    } while (!partida.Salir);

                    partida.Salir = false;
                    fin.Lanzar(pantalla);
                }
            } while (!bienvenida.Salir);
        }
    }


    public class Fin
    {
        int nivel = 0;

        public void Lanzar(int Lvl)
        {
            nivel = Lvl;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("¡Has llegado al nivel {0}!", nivel);
            Console.ReadLine();
        }
    }

    public class Bienvenida
    {
        public bool Salir { get; set; }
        public void Lanzar()
        {

            Console.SetWindowSize(79, 24);
            Console.SetBufferSize(79, 24);
            Console.Clear();
            ConsoleKey usuario;

            Console.SetCursorPosition(10, 5);
            Console.WriteLine("████  ███    ███    ████   ████  █████  ███     ");
            Console.SetCursorPosition(10, 6);
            Console.WriteLine("█     █  █  █   █  █      █      █      █  █    ");
            Console.SetCursorPosition(10, 7);
            Console.WriteLine("███   ███   █   █  █  ██  █  ██  ███    ███     ");
            Console.SetCursorPosition(10, 8);
            Console.WriteLine("█     █  █  █   █  █   █  █   █  █      █  █    ");
            Console.SetCursorPosition(10, 9);
            Console.WriteLine("█     █   █  ███    ███    ███   █████  █   █   ");

            Console.SetCursorPosition(20, 12);

            Console.WriteLine("         *PULSA  [INTRO]         ");
            Console.SetCursorPosition(20, 13);
            Console.WriteLine("          PARA  EMPEZAR         ");



            Console.SetCursorPosition(20, 19);
            Console.WriteLine("         *PULSA  [ESC]   ");
            Console.SetCursorPosition(20, 20);
            Console.WriteLine("          PARA  SALIR            ");


            //Si el usuario le da a Escape, se activa la variable salir (que toma efecto en la clase Juego. Si pulsa R, se va al Bienvenida.Reglas)
            usuario = Console.ReadKey(true).Key;

            if (usuario == ConsoleKey.Escape)
            {
                Salir = true;
            }

        }
        
    }


    //********************************************************************************************

    public class Reglas
    {
        public void Lanzar()
        {
            //Debería hacer esto cuando ya tenga el juego
            Console.Clear();
            Console.SetWindowSize(43, 24);
            Console.SetBufferSize(43, 24);

            Console.SetCursorPosition(16, 1);
            Console.WriteLine("R E G L A S");

            Console.SetCursorPosition(5, 4);
            Console.WriteLine("█ = Tú");

            Console.SetCursorPosition(3, 5);
            Console.WriteLine("-------------------------------------");

            Console.SetCursorPosition(12,6);
            Console.WriteLine("Cosas que te matan:");

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(2, 7);
            Console.WriteLine("     ");

            Console.SetCursorPosition(2, 8);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" █");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("█");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("█ ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(" = Coche");

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(2, 9);
            Console.WriteLine("     ");

            Console.SetCursorPosition(2, 10);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ███");
            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.Write("█");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(" = Camión");

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(2, 11);
            Console.WriteLine("     ");

            Console.SetCursorPosition (5, 12);
            Console.BackgroundColor= ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("■");
            Console.BackgroundColor=ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" = Árbol (sí, te mata también, \n                               por tonto)");

            Console.SetCursorPosition(5, 15);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(" ");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(" = Rio");


            Console.SetCursorPosition(3, 17);
            Console.WriteLine("-------------------------------------");

            Console.SetCursorPosition(18,18);
            Console.WriteLine("CONTROLES");

            Console.SetCursorPosition(5, 20);
            Console.WriteLine("WASD - Movimiento \n   Escape - Salir");

            Console.SetCursorPosition(11, 23);
            Console.Write("PULSA INTRO PARA EMPEZAR");
            Console.ReadLine();
        }
    }

    public class Partida
    {
        bool salir = false, lvlup = false;

        public bool Salir
        {
            get { return salir; }
            set { salir = value; }
        }
        ConsoleKeyInfo input;
        public void Lanzar()
        {
            lvlup = false;
            Console.SetWindowSize(43, 24);
            Console.SetBufferSize(43, 24);

            Cuadro cuadro = new Cuadro();
            Linea[] linea = new Linea[11];
            BloqueCoches[] bloqueCoches = new BloqueCoches[8];
            BloqueArboles[] bloqueArboles = new BloqueArboles[8];
            Puente[] puente = new Puente[8];

            //Es necesario saber el valor Y de los ríos para calcular su colisión con el jugador
            int[] posRios= new int[16];

            //Se crean valores independientes para cada Grupo de Sprites para tener una correcta notación de sus correspondientes arrays.
            int rand, numcoches = 0, numarboles = 0, numtroncos = 0;
            Random rng = new Random();
            
         
            //Este for crea 11 líneas en total aleatorias, dejando la primera y la última como básicas verdes
            for (int i = 0; i < linea.Length; i++)
            {
                if (i >= 10)
                {
                    linea[i] = new Linea(i);
                }
                else if (i == 0)
                {
                    linea[i] = new Linea(i);
                }
                else
                {
                    rand = (rng.Next(0, 3));
                    if (rand == 0)
                    {
                       linea[i] = new Linea(i);
                        bloqueArboles[numarboles] = new BloqueArboles(1 + (2 * i));
                        numarboles++;
                    }
                    if (rand == 1)
                    {
                        linea[i] = new Carretera(i);
                        bloqueCoches[numcoches] = new BloqueCoches(1 + (2 * i));
                        numcoches++;
                    }
                    if (rand == 2)
                    {
                        linea[i] = new Rio(i);
                        puente[numtroncos] = new Puente(1 + (2 * i));
                        posRios[numtroncos] = 1 + (2 * i);
                        numtroncos++;
                        
                    }
                }
            }
            //Aqui recorro todo el array de las posiciones del río sirviendome de la variable numtroncos, que es igual que el largo del array para adivinar cuales son
            //todas las posiciones Y que son rio.
            for (int i = numtroncos+1; i <= numtroncos*2; i++)
            {
                posRios[i] = posRios[(i - numtroncos) - 1] + 1;
            }


            Rana rana = new Rana();

            Console.Clear();
            //se genera el Encuadre solamente una vez
            cuadro.Encuadrar();

            for (int i = 0; i < linea.Length; i++)
            {
                linea[i].Dibujar();
            }

            //------------------------------------------------LOOP-----------------------------
            do
            {

                //linea[v].Dibujar();
                for (int i = 0; i < linea.Length; i++)
                {
                    linea[i].Dibujar();
                }
                for (int i = 0; i < numarboles; i++)
                {
                    bloqueArboles[i].Dibujar();

                    //Colisión de la Rana con los Árboles
                    for (int j = 0; j < 6; j++)
                    {
                        if (bloqueArboles[i] != null && bloqueArboles[i].Arboles[j] != null && bloqueArboles[i].Arboles[j].ColisionaCon(rana))
                        {
                                rana.Death();
                        }
                    }
                }
                for (int i = 0; i < numtroncos; i++)
                {
                    puente[i].Dibujar();
                }
                for (int i = 0; i < numcoches; i++)
                {
                    bloqueCoches[i].MoverDerecha();
                    bloqueCoches[i].Dibujar();

                    //Colisión de la Rana con los Coches
                    for (int j = 0; j < 6; j++)
                    {
                        if (bloqueCoches[i].Coches[j] != null && bloqueCoches[i].Coches[j].ColisionaCon(rana))
                        {
                            rana.Death();
                        }
                    }
                    
                }

                //Aqui establezco que si la rana se encuentra en una posición registrada en el Array de posiciones del río, es que está en el río.
                //Además, si la rana está en X=19, 20 y 21 significaría que está en el puente, así que no triggerea el Game Over
                if (Array.Exists(posRios, element => element == rana.PosY)&&rana.PosX!=19&& rana.PosX != 20 && rana.PosX != 21)
                {
                    rana.Death();
                }

                //CONDICIÓN PARA PASAR DE NIVEL
                if (rana.PosY <= 1)
                {
                    lvlup = true;
                }

                Console.BackgroundColor = ConsoleColor.Black;
                rana.Dibujar();



                //Esto es para que no aparezca la letra pulsada
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Black;

                if (Console.KeyAvailable)
                {
                    input = Console.ReadKey();
                    if (input.Key == ConsoleKey.W)
                    {
                        rana.MoverArriba();
                    }
                    if (input.Key == ConsoleKey.A)
                    {
                        rana.MoverIzquierda();
                    }
                    if (input.Key == ConsoleKey.S)
                    {
                        rana.MoverAbajo();
                    }
                    if (input.Key == ConsoleKey.D)
                    {
                        rana.MoverDerecha();
                    }
                    if (input.Key == ConsoleKey.Escape)
                    {
                        salir = true;
                    }
                }
                Thread.Sleep(50);

                //Si en algún momento la rana se murió, cuando ya se haya renderizado todo el jugador puede ver dónde murió antes de salir del juego.
                if (rana.Muerte)
                {
                    Console.ReadLine();
                    salir = true;
                }
            } while (salir == false && lvlup == false);
        }
    }



    //********************************************************************************************
    public class Cuadro
    {
        public void Encuadrar()
        {
            //EL CUADRO MIDE 38 DE LARGO CON MARGEN DE 2 Y 24 DE ALTO, SIN MARGEN
            //PARTE DE ARRIBA
            Console.SetCursorPosition(3, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("╔");

            for (int i = 0; i < 35; i++)
            {
                Console.Write("═");
            }
            Console.Write("╗");

            //PARTE DEL MEDIO
            for (int i = 0; i < 22; i++)
            {
                Console.SetCursorPosition(3, i + 1);
                Console.Write("║");
                Console.SetCursorPosition(39, i + 1);
                Console.Write("║");
            }

            //PARTE DE ABAJO
            Console.SetCursorPosition(3, 23);
            Console.Write("╚");
            for (int i = 0; i < 35; i++)
            {
                Console.Write("═");
            }
            Console.Write("╝");
        }
    }
    //---------------------------------------------------------------------------------------
        class Linea
    {
        protected string tipo;
        protected int y;
        protected ConsoleColor color;

        public int PosY
        {
            get { return y; }
            set { y = value; }
        }
        public Linea()
        {
            tipo = "linea";
            y = 1;
            color = ConsoleColor.Green;
        }
        
        public Linea(int lineanumero)
        {
            tipo = "linea";
            y = 1 + (2 * lineanumero);
            color = ConsoleColor.Green;
        }

        public virtual void Dibujar()
        {
            Console.BackgroundColor = color;

            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(4, y + i);
                for (int j = 0; j < 35; j++)
                {
                    Console.Write(" ");
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
    class Carretera : Linea
    {
        public Carretera()
        {
            tipo = "carretera";
            y = 1;
            color = ConsoleColor.Gray;
        }
        public Carretera(int lineanumero)
        {
            tipo = "carretera";

            y = 1 + (2 * lineanumero);
            color = ConsoleColor.DarkGray;
        }
    }
    class Rio : Linea
    {
        public Rio()
        {
            tipo = "rio";
            y = 1;
            color = ConsoleColor.Blue;
        }
        public Rio(int lineanumero)
        {
            tipo = "rio";

            y = 1 + (2 * lineanumero);
            color = ConsoleColor.Blue;
        }
    }
    //---------------------------------------------------------------------------------
    abstract class Sprite
    {
        protected int x, y;
        protected string imagen;
        protected ConsoleColor color;

        public int PosY { get { return y; } set { y = value; } }
        public int PosX { get { return x; } set { x = value; } }
        public void MoverA(int nuevaX, int nuevaY)
        {
            x = nuevaX;
            y = nuevaY;
        }

        public virtual void Dibujar()
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(imagen);
            Console.CursorVisible = false;
        }

        public virtual void MoverDerecha()
        {
            x++;
            if (x >= 38) x = 38;
        }

        public virtual void MoverIzquierda()
        {
            x--;
            if (x <= 4) x = 4;
        }
        public void MoverArriba()
        {
            y--;
            if (y <= 1) y = 1;
        }
        public void MoverAbajo()
        {
            y++;
            if (y >= 22) y = 22;
        }

        protected abstract int DevolverLongitud();
        public bool ColisionaCon(Sprite sprite)
        {
               if (this.y != sprite.y)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < this.DevolverLongitud(); i++)
                {
                    for (int j = 0; j < sprite.DevolverLongitud(); j++)
                    {
                        if ((this.x + i) == (sprite.x + j))
                            return true;
                    }
                }
            }
            return false;
        }
    }
    class Rana : Sprite
    {
        bool muerte = false;

        public bool Muerte
        {
            get { return muerte; }
            set { muerte = value; }
        }

        public Rana()
        {
            imagen = "█";
            x = 20;
            y = 22;
            color = ConsoleColor.White;
        }
        protected override int DevolverLongitud()
        {
            return 1;
        }
        public void Death()
        {
            imagen = "X";
            color = ConsoleColor.Red;
            muerte = true;
        }
    }
    class Coche : Sprite
    {
        
        public Coche()
        {
            Random random = new Random();
            int rand = random.Next(0, 4);
            if (rand == 0)
            {
                color = ConsoleColor.Blue;
            }
            if (rand == 1)
            {
                color = ConsoleColor.Green;
            }
            if (rand == 2)
            {
                color = ConsoleColor.Red;
            }
            if (rand == 3)
            {
                color = ConsoleColor.Yellow;
            }
        }
        protected override int DevolverLongitud()
        {
            return 3;
        }
        public Coche(int h, int v)
        {
            x = h;
            y = v;
            Random random = new Random();
            int rand = random.Next(0, 4);
            if (rand == 0)
            {
                color = ConsoleColor.Blue;
            }
            if (rand == 1)
            {
                color = ConsoleColor.Green;
            }
            if (rand == 2)
            {
                color = ConsoleColor.Red;
            }
            if (rand == 3)
            {
                color = ConsoleColor.Yellow;
            }
        }
        public override void Dibujar()
        {
            Console.SetCursorPosition(x, y);

            Console.ForegroundColor = ConsoleColor.Black;
            //CON ESTOS IFS, NOS ASEGURAMOS QUE LOS COCHES NO SE DIBUJEN FUERA O POR ENCIMA DEL CUADRO
            if (x>5 && x < 39)
            {
                Console.Write("█");
            }
            if (x+1 > 5 && x+1 < 39)
            {
                Console.ForegroundColor = color;
                Console.Write("█");
            }
            if (x + 2 > 5 && x + 2 < 39)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("█");
            }
            
           
        }

        public override void MoverDerecha()
        {
            x++;
            if (x >= 38) x = 1;
        }


    }
    
    class Camion : Coche
    {
        public Camion()
        {
            Random random = new Random();
            int rand = random.Next(0, 4);
            if (rand == 0)
            {
                color = ConsoleColor.Blue;
            }
            if (rand == 1)
            {
                color = ConsoleColor.Green;
            }
            if (rand == 2)
            {
                color = ConsoleColor.Red;
            }
            if (rand == 3)
            {
                color = ConsoleColor.Yellow;
            }
        }
        public Camion(int h, int v)
        {
            x = h;
            y = v;
            Random random = new Random();
            int rand = random.Next(0, 4);
            if (rand == 0)
            {
                color = ConsoleColor.Blue;
            }
            if (rand == 1)
            {
                color = ConsoleColor.Green;
            }
            if (rand == 2)
            {
                color = ConsoleColor.Red;
            }
            if (rand == 3)
            {
                color = ConsoleColor.Yellow;
            }
        }
        public override void Dibujar()
        {
            Console.SetCursorPosition(x, y);

            Console.ForegroundColor = ConsoleColor.Black;

            if (x > 6 && x < 39)
            {
                Console.Write("█");
            }  
            if (x+1 > 6 && x+1 < 39)
            {
                Console.Write("█");
            }

            if (x+2 > 6 && x+2 < 39)
            {
                Console.Write("█");
            }
            if (x+3 > 6 && x+3 < 39)
            {
                Console.ForegroundColor = color;
                Console.Write("█");
            }   
        }
        protected override int DevolverLongitud()
        {
            return 4;
        }
    }

    class BloqueCoches
    {
        int x = 1, i = 0;

        Random random = new Random();


        public BloqueCoches(int v)
        {
            Coches = new Coche[6];

            /*Cursor en x=1, crear coche o camion, avanzar 3/4/5, crear otro hasta llegar al ultimo visible */
            do
            {
                Console.SetCursorPosition(x, v);
                int rand = random.Next(0, 4);
                if (rand <= 2)
                {
                    Coches[i] = new Coche(x, v);
                    x = x + 3;
                }
                else
                {
                    Coches[i] = new Camion(x, v);
                    x = x + 4;
                }
                i++;
                rand = random.Next(0, 3);
                if (rand == 0)
                {
                    x = x + 3;
                }
                if (rand == 1)
                {
                    x = x + 4;
                }
                if (rand == 2)
                {
                    x = x + 5;
                }

            } while (x < 39);
        }

        public Coche[] Coches { get; set; }
        public void Dibujar()
        {
            for (int j = 0; j < i; j++)
            {
                Coches[j].Dibujar();
            }
        }
        public void MoverDerecha()
        {
            for (int j = 0; j < i; j++)
            {
                Coches[j].MoverDerecha();
            }
        }
    }
    //---------------------------------------------------------------------------------------------
    class Arbol : Sprite
    {
        public Arbol()
        {
            imagen = "█";
            color = ConsoleColor.DarkGreen;
        }
        public Arbol(int h,int v)
        {
            x = h;
            y = v;
            imagen = "■";
            color = ConsoleColor.DarkGreen;
        }
        protected override int DevolverLongitud()
        {
            return 1;
        }

        public override void Dibujar()
        {
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = color;
            Console.WriteLine(imagen);
        }
    }
    class BloqueArboles
    {
        int k = 0;
        Random random = new Random();
        
        public BloqueArboles(int v)
        {
            k = 0;
             Arboles = new Arbol[10];
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    int rand = random.Next(4, 38);
                    Console.SetCursorPosition(rand, v+j);
                    Arboles[k] = new Arbol(rand, v+j);
                    k++;
                }
            }
                   
        }
        public void Dibujar()
        {
            for (int i = 0; i < k; i++)
            {
               Arboles[i].Dibujar();
            }
        }

        public Arbol[] Arboles { get; set; }
    }
    class Tronco : Sprite
    {
        public Tronco()
        {
            imagen = "█";
            color = ConsoleColor.DarkRed;
        }
        public Tronco(int h, int v)
        {
            x = h;
            y = v;
            imagen = "█";
            color = ConsoleColor.DarkRed;
        }
        protected override int DevolverLongitud()
        {
            return 1;
        }

        public override void Dibujar()
        {
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = color;
            Console.WriteLine(imagen);
        }
    }
    class Puente
    {
        int k = 0;

        public Puente(int v)
        {
            k = 0;
            Troncos = new Tronco[12];
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 2; i++)
                {

                    Console.SetCursorPosition(19+j, v);
                    Troncos[k] = new Tronco(19+j, v);
                    k++;
                    Console.SetCursorPosition(19+j, v + 1);
                    Troncos[k] = new Tronco(19+j, v + 1);
                    k++;
                }
            }        
        }
        public void Dibujar()
        {
            for (int i = 0; i < k; i++)
            {
                Troncos[i].Dibujar();
            }
        }

        public Tronco[] Troncos { get; set; }
    }
}
