using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor2D
{
    interface IColisionable
    {
        bool DetectarColision(NCSprite sp1, NCSprite sp2);
    }
}
