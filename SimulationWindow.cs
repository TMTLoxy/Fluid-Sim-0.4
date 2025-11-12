using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Fluid_Sim_0._4
{
    public partial class SimulationWindow : Form
    {
        // graphics stuff
        private float[,] densities;
        private int windX;
        private int windY;

        // sim stuff
        private List<Particle> particles;
        private int particleCount;
        private GridSquare[,] gridSquares;
        private float vol;
        private float smoothingRad;
        public SimulationWindow()
        {
            InitializeComponent();
            vol = getVol(smoothingRad); // need to assign smoothingRad
        }

        private void SimulationClock_Tick(object sender, EventArgs e)
        {
            // - Object collision check
            // - Particle collision stuff
            // - graphics refresh
            for (int i = 0; i < particleCount; i++)
            {
                // Collision algorithm
                // check ,for each particle, against the contents of it's grid square and it's adjacent squares
                // get current particle's square
                // for squares of index i, +- 1 create a list of particles in all those squares
                // run collision checks against all object edges in that square and adjacent 
                // run collision checks for all of those particles

                List<Particle> nearbyParticles = getNearbyParticles(particles[i].getPos());
                for (int p = 0; p < nearbyParticles.Count; p++)
                {
                    if (nearbyParticles[p] != particles[i])
                    {
                        particles[i].particleCollisionCheck(nearbyParticles[p]);
                    }
                }
                // collision stuff

                particles[i].Update();
                // update all the grid squares to have new particles in
            }

            refreshDensities();

        }

        public void refreshDensities()
        {
            // for each pixel in the display, run getDensity to get the density at that point
        }

        public float getDensity(Vector2 d)
        {
            List<Particle> nearbyParticles = getNearbyParticles(d);
            float density = 0f;
            for (int i = 0; i < nearbyParticles.Count; i++)
            {
                float influence = 1 / nearbyParticles[i].getInfluence(d);
                density += influence * nearbyParticles[i].getMass();
            }
            return density / vol;
        }

        public float getVol(float s)
        {
            return (float)((1f / 6f) * Math.PI * s * s * (3 * s * s - 8 * s + 6));
        }

        public List<Particle> getNearbyParticles(Vector2 d)
        {
            int gridX = (int)d.X / windX;
            int gridY = (int)d.Y / windY;
            Vector2 gridIndex = new Vector2(gridX, gridY);

            List<Particle> nearbyParticles = new List<Particle>();
            for (int i = (int)gridIndex.X - 1; i < gridIndex.X + 1; i++)
            {
                for (int j = (int)gridIndex.Y - 1; j < gridIndex.Y + 1; j++)
                {
                    if (i < 0 || j < 0 || i >= gridSquares.GetLength(0) || j >= gridSquares.GetLength(1)) continue; 
                    // idk what gridSqares.getLength(0) does so change that to highest gridsquare index
                    nearbyParticles.AddRange(gridSquares[i, j].getParticles());
                }
            }
            return nearbyParticles;
        }
    }
}
}