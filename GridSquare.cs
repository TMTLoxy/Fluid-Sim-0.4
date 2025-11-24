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
        private Vector2 index;
        private float width;
        private float height;

        public GridSquare(Vector2 index, float width, float height)
        {
            // go through all the objects and see which ones have bits in the square
            //   use their segment points
            particles = new List<Particle>();
        }

        // each frame update particles list to contain particles that are in this square

        public void updateParticles(List<Particle> particles)
        {
            this.particles = particles;
        }

        public List<Particle> getParticles()
        {
            return particles;
        }
        public Vector2 getIndex() => index;
    }
}
