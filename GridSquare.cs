using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Fluid_Sim_0._4
{
    public class GridSquare
    {
        private List<Particle> particles;
        private List<SDFObject> objectWalls;
        private float width;

        public GridSquare(float width)
        {
            // go through all the objects and see which ones have bits in the square
            //   use their segment points
            particles = new List<Particle>();
            this.width = width;
        }

        // each frame update particles list to contain particles that are in this square

        public void addParticle(Particle particle)
        {
            particles.Add(particle);
        }


        public List<Particle> getParticles()
        {
            return particles;
        }
        public void clearParticles()
        {
            particles.Clear();
        }
    }
}
