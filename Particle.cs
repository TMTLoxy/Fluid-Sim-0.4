using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace Fluid_Sim_0._4
{
    // its a partlce
    public class Particle
    {
        private Vector2 pos;
        private Vector2 prevPos;
        private Vector2 vel;
        private float mass;
        private float influenceRad;
        private Vector2 currentSquare;

        private int gridXCount;
        private int gridYCount;
        private int windX;
        private int windY;


        public Particle()
        {

        }

        public void ParticleCollision(Particle particle)
        {
            // swap velocities for fully elastic collisions, think of it later
        }

        public void particleCollisionCheck(Particle particle)
        {
            float dist = Vector2.Distance(pos, particle.pos);
            if (dist <= influenceRad + particle.influenceRad)
            {
                ParticleCollision(particle);
            }
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

        public float getInfluence(Vector2 d)
        {
            float dist = Vector2.Distance(pos, d);
            if (dist > influenceRad) return 0;
            else
            {
                float nDist = 1 / dist;
                float inf = (nDist - 1) * (nDist - 1);
                return inf;
            }
        }

        public void Update()
        {
            // do all movement and stuff 
            findGridSquare();
        }

        private void findGridSquare()
        {
            int gridX = (int)(pos.X / windX);
            int gridY = (int)(pos.Y / windY);
            currentSquare = new Vector2(gridX, gridY);
        }

        public Vector2 getGridSquare() => currentSquare;
        public float getMass() => mass;
        public Vector2 getPos() => pos;
    }
}
