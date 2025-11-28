using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Diagnostics;

namespace Fluid_Sim_0._4
{
    // TO-DO list in Program.cs
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


        public Particle(Vector2 initPos)
        {
            pos = initPos;
            prevPos = pos;

        vel = new Vector2(-500, 0); // DT testing
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
        public void ObjectBoundaryCollisionCheck(List<SDF_BezierShape> objects)
        {
            // for each object check distance to that obejct, if dist > mincheck then dont check
            for (int i = 0; i < objects.Count; i++)
            {
                float dist = Vector2.Distance(pos, objects[i].getCentre());
                if (dist <= objects[i].getBoundaryDist())
                    ObjectCollisionCheck(objects[i]);
            }
        }
        public void ObjectCollisionCheck(SDF_BezierShape obj)
        {
            float sdf = obj.SDF(pos);
            if (sdf <= 0) ObjectCollision(obj, sdf);
        }
        public void ObjectCollision(SDF_BezierShape obj, float sdf)
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

        public void wallCollisions(List<Wall> walls)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                //Debug.WriteLine("Wall Collision Check..."); // DT
                if (walls[i].checkCollision(this))
                {
                    Debug.WriteLine("Wall Collision Detected"); // DT
                    walls[i].doCollision(this);
                }                
            }
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

        public void Update(float timeInterval, float g, float gridSquareWidth, float gridSquareHeight)
        {
            prevPos = pos;
            // do all movement and stuff
            vel.Y += g;
            pos = prevPos + (vel * timeInterval);
            findGridSquare(gridSquareWidth, gridSquareHeight);
        }

        private void findGridSquare(float gridSquareWidth, float gridSquareHeight)
        {
            int gridX = (int)(pos.X / gridSquareWidth);
            int gridY = (int)(pos.Y / gridSquareHeight);
            currentSquare = new Vector2(gridX, gridY);
        }

        public void setVel(Vector2 newVel) => vel = newVel; 
        public void setPos(Vector2 newPos) => pos = newPos;

        public Vector2 getGridSquare() => currentSquare;
        public float getMass() => mass;
        public Vector2 getPos() => pos;
        public Vector2 getPrevPos() => prevPos;
        public Vector2 getVel() => vel;
    }
}
