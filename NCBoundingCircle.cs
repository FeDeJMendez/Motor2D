using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor2D
{
    class NCBoundingCircle:IColisionable
    {
        public bool DetectarColision(NCSprite sp1, NCSprite sp2)
        {
            bool colision = false;

            //Calculo de Distancia Cuadrada o al cuadrado
            int x = (sp1.X + sp1.Ancho / 2) - (sp2.X + sp2.Ancho / 2);
            int y = (sp1.Y + sp1.Alto / 2) - (sp2.Y + sp2.Alto / 2);
            int d = x * x + y * y;
            if (d <= sp1.RadioC + sp2.RadioC)
                colision = true;

            return colision;
        }
    }
}
