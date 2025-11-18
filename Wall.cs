using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Fluid_Sim_0._4
{
    public abstract class Wall
    {
        protected int borderVal;

        public Wall(int borderVal)
        {
            this.borderVal = borderVal;
        }
    }

    class VerticleWall : Wall
    {
        // reflect x vel
        public VerticleWall(int borderVal) : base(borderVal)
        {
            
        }
    }
    class HorizontalWall : Wall
    {
        // reflect y vel
        public HorizontalWall(int borderVal) : base(borderVal)
        {

        }
    }
}