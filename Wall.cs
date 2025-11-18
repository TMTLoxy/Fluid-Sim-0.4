using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;

namespace Fluid_Sim_0._4
{
    public abstract class Wall
    {
        protected Vector2 pos1;
        protected Vector2 pos2;
        protected int wallSideIndex; // 1/-1, -1 = below/ to left etc.
        protected float maxVal;
        protected Wall linkedWall;


        public Wall(Vector2 pos1, Vector2 pos2, int wallSideIndex)
        {
            this.pos1 = pos1;
            this.pos2 = pos2;
            this.wallSideIndex = wallSideIndex;
            maxVal = Math.Max(Math.Abs(pos2.X - pos1.X), Math.Abs(pos2.Y - pos1.Y));
        }

        public abstract void linkWall();
    }

    class VerticleWall : Wall
    {
        // reflect x vel
        public VerticleWall()
        {

        }
    }
    class HorizontalWall : Wall
    {
        // reflect y vel
        public HorizontalWall()
        {

        }
    }
}
