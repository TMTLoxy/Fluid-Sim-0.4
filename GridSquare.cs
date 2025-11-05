using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluid_Sim_0._4
{
    internal class GridSquare
    {
        private List<Particle> particles;
        private List<SDFObject> objectWalls;

        public GridSquare(List<SDFObject> objs)
        {
            // go through all the objects and see which ones have bits in the square
            //   use their segment points
        }

        // each frame update particles list to contain particles that are in this square

        private void updateParticles(List<Particle> particles)
        {
            this.particles = particles;
        }
    }
}
