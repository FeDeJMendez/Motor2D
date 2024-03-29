﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Motor2D
{
    public partial class Form1 : Form
    {
        private Bitmap resultante;          // Totalidad de todo el cuadro - Suma de todos los Sprites
        private int anchoVentana, altoVentana;

        private NCEngine motor;

        /*Definición "Flicker (Parpadeo)"= Parpadeo por la desincronizacion del tiempo de Refresh de la pantalla
                                           y el cambio de cuadro*/

        //Variables de Doble Buffer para evitar el Flicker
        private Bitmap dBufferBMP;
        private Graphics dBufferDC;         /*Es el Device Context del Dibujo.
                                              La Superficie sobre la que se esta Dibujando.*/

        private int cx = 100, cy = 0;         //Variables para Prueba

        //Creo dos Sprites (Coordenadas, Dimensiones, NombreArchivo, CantidadCuadrosyAnimaciones, Visible, Procesando)
        private NCSpriteT uno = new NCSpriteT(100, 100, 80, 60, "Sprite0.png", 5, 4, true, true, 1, Color.FromArgb(0,0,255));
        private NCSprite dos = new NCSprite(250, 200, 80, 60, "Sprite0.png", 5, 4, true, true, 2);
        private NCSprite tres = new NCSprite(250, 200, 80, 60, "Sprite0.png", 5, 4, true, true, 3);


        public Form1()
        {
            InitializeComponent();

            //Nuevo Bitmap y su superficie de trabajo (el Device Context)
            dBufferBMP = new Bitmap(this.Width, this.Height);
            dBufferDC = Graphics.FromImage(dBufferBMP);

            //Bitmap Resultante del Cuadro
            resultante = new Bitmap(800, 600);

            //Valores para Dibujo con Scrolls
            anchoVentana = 800;
            altoVentana = 600;

            //Versión del Sprite
            this.Text += " " + uno.Version.ToString();

            //Instancia del Motor que Administra los Sprites
            motor = new NCEngine(anchoVentana, altoVentana, TiposColisiones.BoundingCircle);

            motor.AgregarSprite(uno);
            motor.AgregarSprite(tres);
            motor.AgregarSprite(dos);
            motor.InicializarEngine();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Solamente Inicia y Termina el Timer
            if (simularToolStripMenuItem.Checked == true)
                timer1.Enabled = true;
            else
                timer1.Enabled = false;
        }


        private void procesarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Procesa y Coloca los Píxeles Necesarios
            uno.CuadroActual = 0;
            uno.AnimacionActual = 0;

            //uno.FlipH = true;
            //uno.FlipV = true;
            uno.X = 100;
            uno.Y = 100;
            uno.CuadroActual = 3;
            uno.VelAnimacion = 2;
            uno.DireccionAnim = DirAnimacion.Normal;
            uno.TipoAnim = TipoAnimacion.PingPong;
            //uno.ColocarDelta(5, 3);
            uno.deltaX = 1;
            uno.deltaY = 0;
            uno.Colisionable = true;

            dos.X = 400;
            dos.Y = 120;
            dos.AnimacionActual = 2;
            dos.Colisionable = true;
            //dos.deltaY = 2;

            tres.X = 300;
            tres.Y = 90;
            tres.AnimacionActual = 3;
            tres.Colisionable = false;

            motor.CicloJuego();
            resultante = motor.Canvas;
            this.Invalidate();          //Redibujo de la Ventana
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Actualizo Variables de Prueba
            cx++; 
            cy++;

            //Dibujo
            //uno.Y += 3;             //En cada cuadro se actualiza el valor de X del Sprite
            //uno.X += 4;
            //dos.Y = cy;
            motor.CicloJuego();         //LLeva a cabo el dibujo interno
            resultante = motor.Canvas;          //Guarda lo que dibujo el motor

            //COPIAR TODO AL BUFFER

            //Copia el dibujo de la ventana
            Graphics ClientDC = this.CreateGraphics();          //ClientDC es el Device Context de la Ventana

            if (resultante != null)         //Verificacion de Bitmap Iniciado OK
            {
                AutoScrollMinSize = new Size(anchoVentana, altoVentana);            //Cálculo de Scroll

                //Copia del Bitmap Resultante al Buffer
                ClientDC.DrawImage(resultante,
                                    new Rectangle(this.AutoScrollPosition.X,
                                                    this.AutoScrollPosition.Y + 30,
                                                    anchoVentana, altoVentana));

                ClientDC.DrawImage(dBufferBMP, 0, 0);           //Dibujo de Buffer a la Ventana
            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ivan Zurlis", "En Construccion");
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (resultante != null)         //Verificacion = Bitmap Instanciado
            {
                Graphics g = e.Graphics;            //Obtencion del Objeto Graphics                  
                AutoScrollMinSize = new Size(anchoVentana, altoVentana);            //Dimensiones del Area de Scroll

                //Copia del Bitmap a la Ventana. Y +30 para no superponer el menú
                g.DrawImage(resultante,
                            new Rectangle(this.AutoScrollPosition.X,
                                          this.AutoScrollPosition.Y + 30,
                                          anchoVentana, altoVentana));

                g.Dispose();            //Liberación del Recurso
            }
        }
    }
}
