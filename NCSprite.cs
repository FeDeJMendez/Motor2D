using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
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

        //Efecto espejo Flip
        private bool flipHorizontal;
        private bool flipVertical;

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

            imagen = new Bitmap(Imagen);                //Carga la imagen BMP o PNG con los cuadros

            flipHorizontal = false;
            flipVertical = false;
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

        public bool FlipH { get { return flipHorizontal; } set { flipHorizontal = value; } }
        public bool FlipV { get { return flipVertical; } set { flipVertical = value; } }

        public string Version { get { return "1.0.0.3"; } }


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
            Color colorImagen = new Color();                //Color de un pixel obtenido de la imagen

            //Posiciones de copiado
            int x = posX;
            int y = posY;

            int inicioX = 0;
            int inicioY = 0;
            int finalX = 0;
            int finalY = 0;
            int xRecorrido = 0;
            int yRecorrido = 0;
            int avanceX = 1;
            int avanceY = 1;
            int reinicioY = posY;

            //Si el Sprite esta fuera del Canvas no lo dibuja y sale
            if ((posY + alto < 0) || posY >= canvas.Height || (posX + ancho < 0) || posX >= canvas.Width)
                return;

            if (flipHorizontal == false)
            {
                inicioX = cuadroActual * ancho;
                finalX = inicioX + ancho;
                avanceX = 1;

                //Verifica si hay Clipping a la Izquierda sin Flip
                if (posX < 0)
                {
                    x = 0;
                    inicioX += -posX;
                }
                //Verifica si hay Clipping a la Derecha sin Flip 
                else if (posX + ancho >= canvas.Width)
                {
                    finalX -= (x + ancho) - canvas.Width;
                }
            }
            else
            {
                finalX = cuadroActual * ancho;
                inicioX = finalX + ancho - 1;
                avanceX = -1;

                //Verifica si hay Clipping a la Izquierda con Flip
                if (posX < 0)
                {
                    x = 0;
                    inicioX += posX;
                }
                
                //Verifica si hay Clipping a la Derecha con Flip
                if ((posX + ancho) >= canvas.Width)
                {
                    finalX += (x + ancho) - canvas.Width;
                }
            }

            if (flipVertical == false)
            {
                inicioY = animacionActual * alto;
                finalY = inicioY + alto;
                avanceY = 1;

                //Verifica si hay Clipping a la Arriba sin Flip
                if (posY < 0)
                {
                    y = 0;
                    inicioY += -posY;
                    reinicioY = 0;
                }

                //Verifica si hay Clipping a la Abajo sin Flip
                if (posY + alto >= canvas.Height)
                {
                    finalY -= (y + alto) - canvas.Height;
                }
            }
            else
            {
                finalY = animacionActual * alto;
                inicioY = finalY + alto - 1;
                avanceY = -1;

                //Verifica si hay Clipping a la Arriba con Flip
                if (posY < 0)
                {
                    y = 0;
                    reinicioY = 0;
                    inicioY += posY;
                }

                //Verifica si hay Clipping a la Abajo con Flip
                if ((posY + alto) >= canvas.Height)
                {
                    finalY += (y + alto) - canvas.Height;
                }
            }


            //Recorre la imagen, copia cada pixel y lo coloca en el Canvas
            for (xRecorrido=inicioX; evaluarSegunDireccion(xRecorrido, finalX, flipHorizontal); xRecorrido+=avanceX, x++)
            {
                for (yRecorrido=inicioY, y=reinicioY; evaluarSegunDireccion(yRecorrido, finalY, flipVertical); yRecorrido+=avanceY, y++)
                {
                    colorImagen = imagen.GetPixel(xRecorrido, yRecorrido);
                    canvas.SetPixel(x, y, colorImagen);
                }
            }            
        }

        private bool evaluarSegunDireccion(int control, int tope, bool flip)
        {
            bool resultado = false;
            if (flip == false)
            {
                resultado = control < tope;
            }
            else
            {
                resultado = control >= tope;
            }
            return resultado;
        }
    }
}