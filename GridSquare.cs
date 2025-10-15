using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fluid_Sim_0._4
{
    internal class GridSquare
    {
        private List<Particle> particles;
        private Vector2 CentrePos;
        private Vector2 topLeftVertex; // min x & y value of the square
        private Vector2 topRightVertex; // max x & y value of the square

        public GridSquare()
        {

        }
        public void Clear()
        {
            particles.Clear();
        }
        public List<Particle> getParticles() => particles;
    }
}
