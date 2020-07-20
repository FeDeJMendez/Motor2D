using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor2D
{
    public enum TipoAnimacion { SinAnimacion, UnaVez, Repite, PingPong };
    public enum DirAnimacion { Normal = 1, Reversa = -1 };

    public class NCSprite
    {
        //Posicion Sprite
        protected int posX;
        protected int posY;

        //Tamaño Sprite
        protected int ancho;
        protected int alto;

        //Imagen de Sprites
        protected Bitmap imagen;

        //Info de Animación
        private int cuadros;                //Cantidad de cuadros de Animación
        protected int cuadroActual;           //Animación que se está mostrando
        private int animaciones;            //Cantidad de animaciones del Sprite
        protected int animacionActual;        //Animación que se esta mostrando

        private bool activo;                //Check Sprite hace ciclo de animación
        private bool visible;               //Check Sprite se dibuja

        //Dibujo del sprite
        protected Bitmap canvas;              //Donde se dibuja el sprite
        private Bitmap recorte;             //Recorte del fondo para reponer

        //Efecto espejo Flip
        protected bool flipHorizontal;
        protected bool flipVertical;

        private TipoAnimacion tAnimacion;
        private DirAnimacion dAnimacion;
        private int velAnimacion;
        private int contAnimacion;

        //Deltas para el avance por cuadro de animacion
        private int dX;
        private int dY;

        //Copia auxiliar del fondo que se guarda en recorte para reponer en movimiento
        private bool usarCopia;
        private int Xc;
        private int Yc;
        private int recorridoXc;
        private int recorridoYc;

        //Para colisiones
        private bool colisionable;
        private bool colisionado;
        private int xan;
        private int yal;
        private int radioC;             //Radio al cuadrado para evitar la raiz

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

            tAnimacion = TipoAnimacion.SinAnimacion;
            dAnimacion = DirAnimacion.Normal;
            velAnimacion = 2;
            contAnimacion = 0;

            dX = 0;
            dY = 0;

            usarCopia = false;
            recorridoXc = 0;
            recorridoYc = 0;
            recorte = new Bitmap(ancho, alto);

            colisionable = false;
            colisionado = false;

            xan = posX + ancho;
            yal = posY + alto;

            radioC = (ancho / 2) * (ancho / 2) + (alto / 2) * (alto / 2);
        }


        //Getters & Setters
        public int X { get { return posX; } set { posX = value; } }
        public int Y { get { return posY; } set { posY = value; } }

        public int Ancho { get { return ancho; } }
        public int Alto { get { return alto; } }

        public int CuadroActual
        {
            get { return cuadroActual; }

            set
            {
                cuadroActual = value;
                if (cuadroActual >= cuadros)
                    cuadroActual = cuadros - 1;
                if (cuadroActual < 0)
                    cuadroActual = 0;
            }
        }

        public int Animaciones { get { return animaciones; } }
        public int AnimacionActual { get { return animacionActual; } set { animacionActual = value; } }

        public bool Activo { get { return activo; } set { activo = value; } }
        public bool Visible { get { return visible; } set { visible = value; } }

        public bool FlipH { get { return flipHorizontal; } set { flipHorizontal = value; } }
        public bool FlipV { get { return flipVertical; } set { flipVertical = value; } }

        public TipoAnimacion TipoAnim { get { return tAnimacion; } set { tAnimacion = value; } }
        public DirAnimacion DireccionAnim { get { return dAnimacion; } set { dAnimacion = value; } }
        public int VelAnimacion { get { return velAnimacion; } set { velAnimacion = value; } }

        public string Version { get { return "1.0.1.0"; } }

        public int deltaX { get { return dX; } set { dX = value; } }
        public int deltaY { get { return dY; } set { dY = value; } }

        public bool Colisionable { get { return colisionable; } set { colisionable = value; } }
        public bool Colisionado { get { return colisionado; } set { colisionado = value; } }

        public int Xan { get { return xan; } }
        public int Yal { get { return yal; } }

        public int RadioC { get { return radioC; } set { radioC = value; } }

        public void ColocarDelta(int DX, int DY)
        {
            dX = DX;
            dY = DY;
        }

        public void ColocarCanvas(Bitmap Canvas) 
        {
            canvas = Canvas;
        }

        public void ColocarImagen (Bitmap Imagen)
        {
            imagen = Imagen;
        }

        public virtual void DibujarSprite()
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

        protected bool evaluarSegunDireccion(int control, int tope, bool flip)
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

        public void AvanzarAnimacion()
        {
            if (tAnimacion == TipoAnimacion.SinAnimacion)
                return;

            if (tAnimacion == TipoAnimacion.UnaVez)
            {
                if (cuadroActual >= cuadros)
                {
                    cuadroActual = cuadros - 1;             //La deja en el ultimo cuadro
                    tAnimacion = TipoAnimacion.SinAnimacion;                //La frena
                }
                if (cuadroActual < 0)
                {
                    cuadroActual = 0;               //La deja en el primer cuadro
                    tAnimacion = TipoAnimacion.SinAnimacion;                //La frena
                }
            }

            if (tAnimacion == TipoAnimacion.PingPong)
            {
                if (cuadroActual >= cuadros-1)
                    dAnimacion = DirAnimacion.Reversa;
                if (cuadroActual == 0)
                    dAnimacion = DirAnimacion.Normal;
            }

            if (tAnimacion == TipoAnimacion.Repite && contAnimacion >= 30 - velAnimacion)
            {
                if (cuadroActual >= cuadros - 1 && dAnimacion == DirAnimacion.Normal)
                    cuadroActual = -1;
                if (cuadroActual == 0 && dAnimacion == DirAnimacion.Reversa)
                    cuadroActual = cuadros;

            }

            //Avance del cuadro de animacion
            contAnimacion += velAnimacion;
            if (contAnimacion >= 30)                //El programa procesa aprox. 30 img por seg.
            {
                cuadroActual += (int)dAnimacion;
                contAnimacion = 0;

                if (cuadroActual >= cuadros)
                    cuadroActual = cuadros - 1;
                if (cuadroActual < 0)
                    cuadroActual = 0;
            }
        }

        public void Movimiento()
        {
            posX += dX;
            posY += dY;

            if (colisionable)
            {
                xan = posX + ancho;
                yal = posY + alto;
            }
        }

        public void CopiarFondo()
        {
            //Bloque auxiliar para guardar el fondo
            Color colorImagen = new Color();
            int xr = 0;
            int yr = 0;

            //Limites del bloque
            int x = posX;
            int y = posY;
            int limX = posX + ancho;
            int limY = posY + alto;

            //Si hay Culing no realiza la copia y sale
            if ((posY + alto < 0) || posY >= canvas.Height || (posX + ancho < 0) || posX >= canvas.Width)
            {
                usarCopia = false;
                return;
            }
            else
                usarCopia = true;

            //Recortes en los Clipping
            if (posX < 0)               //Izquierda
            {
                x = 0;
                limX = ancho + posX;
            }
            else if (posX + ancho >= canvas.Width)              //Derecha
                limX = canvas.Width;

            if (posY < 0)               //Arriba
            {
                y = 0;
                limY = alto + posY;
            }
            else if (posY + alto >= canvas.Height)              //Abajo
                limY = canvas.Height;

            //Tamaño
            recorridoXc = limX - x;
            recorridoYc = limY - y;

            //Copia de la porcion de fondo
            Xc = x;
            Yc = y;
            int reinicioY = y;

            for (x = x, xr = 0; x < limX; x++, xr++)
            {
                for (y = reinicioY, yr = 0; y < limY; y++, yr++)
                {
                    colorImagen = canvas.GetPixel(x, y);
                    recorte.SetPixel(xr, yr, colorImagen);
                }
            }
        }

        public void PintarFondo()
        {
            if (usarCopia == false)
                return;

            Color colorImagen = new Color();
            int xr = 0;
            int yr = 0;
            int x = Xc;
            int y = Yc;

            //Volvemos la porcion al Canvas
            for (xr = 0; xr < recorridoXc; xr++)
            {
                for(yr = 0; yr < recorridoYc; yr++)
                {
                    colorImagen = recorte.GetPixel(xr, yr);
                    canvas.SetPixel(x + xr, y + yr, colorImagen);
                }
            }
        }
    }
}