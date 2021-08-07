using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor2D
{
    public enum TiposColisiones { BoundingRectagle, BoundingCircle };
    class NCEngine
    {
        //Donde se dibuja la pantalla
        private Bitmap lienzo;

        //Tamaño del lienzo
        private int ancho;
        private int alto;

        //Lista de Sprites
        private List<NCSprite> listaSprites = new List<NCSprite>();

        private IColisionable colisionador;
        private TiposColisiones tipoColision;

            //Constructor
        public NCEngine(int Ancho, int Alto, TiposColisiones TipoColision)
        {
            ancho = Ancho;
            alto = Alto;

            lienzo = new Bitmap(ancho, alto);

            switch (TipoColision)
            {
                case TiposColisiones.BoundingRectagle:
                    tipoColision = TipoColision;
                    colisionador = new NCBoundingRectangle();
                    break;
                case TiposColisiones.BoundingCircle:
                    tipoColision = TipoColision;
                    colisionador = new NCBoundingCircle();
                    break;
            }

            InitPruebas();
        }

            //Getters & Setters
        public Bitmap Canvas { get { return lienzo; } }

        public TiposColisiones TipoColision
        {
            get { return tipoColision; }

            set
            {
                switch (value)
                {
                    case TiposColisiones.BoundingRectagle:
                        tipoColision = value;
                        colisionador = new NCBoundingRectangle();
                        break;
                    case TiposColisiones.BoundingCircle:
                        TipoColision = value;
                        colisionador = new NCBoundingCircle();
                        break;
                }
            }
        }

        //Temporal para pruebas
        public void InitPruebas()
        {
            /*//Carga el fondo de color Verde
            for(int x = 0; x < lienzo.Width; x++)
            {
                for(int y = 0; y < lienzo.Height; y++)
                    lienzo.SetPixel(x, y, Color.DarkGreen);
            }*/

            //Carga el fondo con una imagen
            lienzo = new Bitmap("FotoFondo.png");
        }

        public void AgregarSprite(NCSprite Sprite)
        {
            if (Sprite != null)
            {
                Sprite.ColocarCanvas(lienzo);
                listaSprites.Add(Sprite);
            }
        }

        public void CicloJuego()
        {
            /*//Recorre Todos los Sprites y los Manda a Dibujar
            foreach(NCSprite sprite in listaSprites)
            {
                sprite.Movimiento();
                sprite.AvanzarAnimacion();
                sprite.DibujarSprite();
            }*/

            //Tapa los sprites y limpia el Canvas. A la primera tiene que estar inicializado
            foreach (NCSprite sprite in listaSprites)
                sprite.PintarFondo();

            //Mueve posicion, copia porcion de fondo, avanza el cuadro de animacion y dibuja el sprite
            foreach (NCSprite sprite in listaSprites)
            {
                sprite.Movimiento();
                sprite.CopiarFondo();
                sprite.AvanzarAnimacion();
            }

            foreach (NCSprite sprite in listaSprites)
                sprite.DibujarSprite();

            VerificarColisiones();
        }

        //Inicializa con los fondos cargados de los primeros sprites
        public void InicializarEngine()
        {
            foreach (NCSprite sprite in listaSprites)
                sprite.CopiarFondo();
        }

        public void VerificarColisiones()
        {
            //Variables auxiliares para el recorrido
            int n = 0;
            int m = 0;
            
            /*int x1 = 0;
            int x2 = 0;
            int y1 = 0;
            int y2 = 0;

            int x1an = 0;
            int x2an = 0;
            int y1al = 0;
            int y2al = 0;*/

            //Compara todos los sprites buscando colisiones
            for (n = 0; n <listaSprites.Count -1; n++)
            {
                if (listaSprites[n].Colisionable == true)
                {
                    //Console.WriteLine("Sprite {0} es Colisionable", n);
                    for (m = n + 1; m < listaSprites.Count; m++)
                    {
                        if (listaSprites[m].Colisionable == true)
                        {
                            /*//Puntos limites de los sprites a comparar
                            x1 = listaSprites[n].X;
                            x2 = listaSprites[m].X;
                            y1 = listaSprites[n].Y;
                            y2 = listaSprites[m].Y;

                            x1an = x1 + listaSprites[n].Ancho;
                            x2an = x2 + listaSprites[m].Ancho;
                            y1al = y1 + listaSprites[n].Alto;
                            y2al = y2 + listaSprites[m].Alto;

                            if (((x1 >= x2 && x1 < x2an) || (x1an >= x2 && x1an < x2an)) && 
                                ((y1 >= y2 && y1 < y2al) || (y1al >= y2 && y1al < y2al)))
                            {
                                listaSprites[n].Colisionado = true;
                                listaSprites[m].Colisionado = true;
                                Console.WriteLine("{0} contra {1}",n ,m);
                            }*/

                            if (colisionador.DetectarColision(listaSprites[n], listaSprites[m]))
                            {
                                listaSprites[n].Colisionado = true;
                                listaSprites[m].Colisionado = true;
                                //Console.WriteLine("{0} contra {1}",n ,m);
                            }
                            else
                                listaSprites[n].Colisionado = false;
                        }
                    }
                }
            }
        }
    }
}
