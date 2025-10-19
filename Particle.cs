using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Fluid_Sim_0._4
{
    internal class Particle
    {
        private Vector2 pos;
        private Vector2 prevPos;
        private Vector2 vel;
        private float influenceRad;

        public Particle()
        {

        }

        public void ParticleCollision(Particle incidentParticle)
        {
            
        }
        public void ObjectBoundaryCollisionCheck(List<SDFObject> objects)
        {
            // for each object check distance to that obejct, if dist > mincheck then dont check
            for (int i = 0; i < objects.Count; i++)
            {
                float dist = Vector2.Distance(pos, objects[i].getCentre());
                if (dist <= objects[i].getBoundaryDist())
                    ObjectCollisionCheck(objects[i]);
            }
        }
        public void ObjectCollisionCheck(SDFObject obj)
        {
            float sdf = obj.SDF(pos);
            if (sdf <= 0) ObjectCollision(obj, sdf);
        }
        public void ObjectCollision(SDFObject obj, float sdf)
        {
            // gets normal, finds the velocity perpendicular and parallel to shape normal,
            // computes new vels after collision then adjusts the particle's position to the edge of the shape
            Vector2 n = obj.estimateNormal(pos);
            Vector2 Vper = (Vector2.Dot(vel, n)) * n;
            Vector2 Vpar = vel - Vper;
            Vector2 newVper = -(Vper);
            vel = newVper + Vpar;
            // later can add some dampening and stuff 
            pos -= sdf * n; 
        }

        public void Update()
        {

        }
    }
}
