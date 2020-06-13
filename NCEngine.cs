﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor2D
{
    class NCEngine
    {
        //Donde se dibuja la pantalla
        private Bitmap lienzo;

        //Tamaño del lienzo
        private int ancho;
        private int alto;

        //Lista de Sprites
        private List<NCSprite> listaSprites = new List<NCSprite>();


            //Constructor
        public NCEngine(int Ancho, int Alto)
        {
            ancho = Ancho;
            alto = Alto;

            lienzo = new Bitmap(ancho, alto);

            InitPruebas();
        }

            //Getters & Setters
        public Bitmap Canvas { get { return lienzo; } }


        //Temporal para pruebas
        public void InitPruebas()
        {
            for(int x = 0; x < lienzo.Width; x++)
            {
                for(int y = 0; y < lienzo.Height; y++)
                {
                    lienzo.SetPixel(x, y, Color.DarkGreen);
                }
            }
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
            /*Recorre Todos los Sprites y los Manda a Dibujar*/
            foreach(NCSprite sprite in listaSprites)
            {
                sprite.DibujarSprite();
            }
        }
    }
}
