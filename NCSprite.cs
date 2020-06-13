using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor2D
{
    public class NCSprite
    {
        //Posicion Sprite
        private int posX;
        private int posY;

        //Tamaño Sprite
        private int ancho;
        private int alto;

        //Imagen de Sprites
        private Bitmap imagen;

        //Info de Animación
        private int cuadros;                //Cantidad de cuadros de Animación
        private int cuadroActual;           //Animación que se está mostrando
        private int animaciones;            //Cantidad de animaciones del Sprite
        private int animacionActual;        //Animación que se esta mostrando

        private bool activo;                //Check Sprite hace ciclo de animación
        private bool visible;               //Check Sprite se dibuja

        //Dibujo del sprite
        private Bitmap canvas;              //Donde se dibuja el sprite
        private Bitmap recorte;             //Recorte del fondo para reponer


        //Constructor
        public NCSprite(int PosX, int PosY, int Ancho, int Alto, string Imagen,
                        int Cuadros, int Animaciones, bool Activo, bool Visible)
        {
            posX = PosX;
            posY = PosY; ;
            ancho = Ancho;
            alto = Alto;
            cuadros = Cuadros;
            animaciones = Animaciones;
            activo = Activo;
            visible = Visible;

            animacionActual = 0;
            cuadroActual = 0;
        }


        //Getters & Setters
        public int X { get { return posX; } set { posX = value; } }
        public int Y { get { return posY; } set { posY = value; } }

        public int Ancho { get { return ancho; } }
        public int Alto { get { return alto; } }

        public int CuadroActual { get { return cuadroActual; } set { cuadroActual = value; } }
        public int Animaciones { get { return animaciones; } }
        public int AnimacionActual { get { return animacionActual; } set { animacionActual = value; } }

        public bool Activo { get { return activo; } set { activo = value; } }
        public bool Visible { get { return visible; } set { visible = value; } }

        public string Version { get { return "1.0.0.1"; } }


        public void ColocarCanvas(Bitmap Canvas) 
        {
            canvas = Canvas;
        }

        public void ColocarImagen (Bitmap Imagen)
        {
            imagen = Imagen;
        }

        public void DibujarSprite()
        {
            int x = 0;
            int y = 0;

            for (x = posX; x < posX + ancho; x++)
            {
                for (y = posY; y < posY + alto; y++)
                {
                    canvas.SetPixel(x, y, Color.Blue);
                }
            }
        }
    }
}