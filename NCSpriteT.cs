using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Motor2D
{
    class NCSpriteT:NCSprite
    {
        private Color colorT;               //Color de Transparencia

        public NCSpriteT (int PosX, int PosY, int Ancho, int Alto, string Imagen,
                        int Cuadros, int Animaciones, bool Activo, bool Visible, Color ColorT)
            :base(PosX, PosY, Ancho, Alto, Imagen, Cuadros, Animaciones, Activo, Visible)
        {
            colorT = ColorT;
        }

        public override void DibujarSprite()
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
            for (xRecorrido = inicioX; evaluarSegunDireccion(xRecorrido, finalX, flipHorizontal); xRecorrido += avanceX, x++)
            {
                for (yRecorrido = inicioY, y = reinicioY; evaluarSegunDireccion(yRecorrido, finalY, flipVertical); yRecorrido += avanceY, y++)
                {
                    colorImagen = imagen.GetPixel(xRecorrido, yRecorrido);
                    if(colorImagen != colorT)
                        canvas.SetPixel(x, y, colorImagen);
                }
            }
        }

    }
}
