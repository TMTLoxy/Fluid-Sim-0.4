using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Fluid_Sim_0._4
{
    // TO-DO list in Program.cs
    // its a partlce
    public class Particle
    {
        private Vector2 pos;
        private Vector2 prevPos;
        private Vector2 predictedPos;
        private Vector2 vel;
        private float mass;
        private float influenceRad;
        private Vector2 currentSquare;
        private float gridSquareWidth;
        private int particleID;

        // only density being used rn, use properties array later
        private float density;
        // private float[] properties = new float[density, pressure];
        // example of properties arry to hold the different bits for each particle ie the density and pressure at its location

        private int gridXCount;
        private int gridYCount;
        private int windX;
        private int windY;

        public Particle(Vector2 initPos, float gridSquareWidth, int particleID)
        {
            this.particleID = particleID;
            pos = initPos;
            prevPos = pos;
            predictedPos = pos;
            mass = 1;
            density = 1;

            this.gridSquareWidth = gridSquareWidth;
            setGridSquare();
        }


        #region Particle Interactions
        public void applyPressureForce(Vector2 pressureForce, float timeInterval)
        {
            Vector2 accel = pressureForce / density;
            vel += accel * timeInterval;
        }

        public void updatePosition(float timeInterval)
        {
            pos += vel * timeInterval;
        }
        
        public void calculateDensity(float vol, float smoothingRad, List<Particle> nearbyParticles)
        {
            float density = 0f;
            for (int i = 0; i < nearbyParticles.Count; i++)
            {
                if (nearbyParticles[i].getPos() == pos) continue;
                float dist = Vector2.Distance(predictedPos, nearbyParticles[i].getPredictedPos());
                float influence = smoothingKernal(dist, smoothingRad);
                float mass = nearbyParticles[i].getMass();
                density += influence * mass; // replace density with an index of particle/properties to use fumction for stuff other than density
            }
            this.density = density; // maybe div by volume not sure yet idk whats going on
        }

        public float smoothingKernal(float dist, float smoothingRad)
        {
            if (dist >= smoothingRad) return 0f;
            float coeff = dist / smoothingRad;
            float influence = coeff * coeff - 2 * coeff + 1;
            return influence; // if doesn't work try returning influence * smoothingRad
        }
        public void predictPositions(float g, float timeInterval)
        {
            vel.Y += g * timeInterval;
            predictedPos = pos + vel * timeInterval;
        }
        #endregion
        #region Object Interactions
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
        #endregion
        #region Grid handling
        public Vector2 findGridSquare()
        {
            int gridX = (int)(predictedPos.X / gridSquareWidth);
            int gridY = (int)(predictedPos.Y / gridSquareWidth);
            currentSquare = new Vector2(gridX, gridY);
            return currentSquare;
        }
        public void setGridSquare()
        {
            int gridX = (int)(predictedPos.X / gridSquareWidth);
            int gridY = (int)(predictedPos.Y / gridSquareWidth);
            currentSquare = new Vector2(gridX, gridY);
        }
        #endregion
        #region Wall Interactions
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

        public void setPos(Vector2 pos)
        {
            this.pos = pos;
        }
        public void setVel(Vector2 vel)
        {
            this.vel = vel;
        }
        #endregion

        #region Get Functions
        public Vector2 getGridSquare() => currentSquare;
        public float getMass() => mass;
        public Vector2 getPos() => pos;
        public Vector2 getPrevPos() => prevPos;
        public Vector2 getPredictedPos() => predictedPos;
        public Vector2 getVel() => vel;
        public float getDensity() => density;
        public int getID() => particleID;
        #endregion
    }
}
