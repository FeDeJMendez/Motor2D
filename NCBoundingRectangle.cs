using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor2D
{
    class NCBoundingRectangle:IColisionable
    {
        public bool DetectarColision(NCSprite sp1, NCSprite sp2)
        {
            bool colision = false;
            
            if (((sp1.X >= sp2.X && sp1.X < sp2.Xan) || (sp1.Xan >= sp2.X && sp1.Xan < sp2.Xan)) && 
                ((sp1.Y >= sp2.Y && sp1.Y < sp2.Yal) || (sp1.Yal >= sp2.Y && sp1.Yal < sp2.Yal)))
            {
                colision = true;
            }

            return colision;
        }
    }
}
