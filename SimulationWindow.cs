using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
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
        public SimulationWindow()
        {
            InitializeComponent();
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

                Vector2 gridIndex = particles[i].getGridSquare();
                List<Particle> nearbyParticles = new List<Particle>();
                for (int j = (int)gridIndex.X - 1; j < gridIndex.X + 1; j++)
                {
                    for (int k = (int)gridIndex.Y - 1; k < gridIndex.Y + 1; k++)
                    {
                        if (j < 0 || k < 0 || j >= gridSquares.GetLength(0) || k >= gridSquares.GetLength(1)) continue;
                        nearbyParticles.AddRange(gridSquares[j, k].getParticles());
                    }
                }

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
            // 1/6 pi * s^2 ( 3s^2 - 8s + 6)
            // this gives the volume for the sum of the influence of the particles in the smoothing radius at s distance

            // densitie at a given point p are the sum of the smoothing kernal values for all particles within the influence range of p
            // where the kernal returns a value based of distance
            // kernal: x^2 - 2x + 1
            // the volume of the influence rad is always constant no matter that radius so do stuff
            // volume = 1/6 pi * s^2 ( 3s^2 - 8s + 6)
        }

        public void getDensity(Vector2 d, float smoothingRad)
        {
            // get rid of the get nearby particles its just here for sake of it
            GridSquare sqr = getGridSquare(d);
            Vector2 gridIndex = sqr.getIndex();
            List<Particle> nearbyParticles = new List<Particle>();
            for (int j = (int)gridIndex.X - 1; j < gridIndex.X + 1; j++)
            {
                for (int k = (int)gridIndex.Y - 1; k < gridIndex.Y + 1; k++)
                {
                    if (j < 0 || k < 0 || j >= gridSquares.GetLength(0) || k >= gridSquares.GetLength(1)) continue;
                    nearbyParticles.AddRange(gridSquares[j, k].getParticles());
                }
            }


        }

        public GridSquare getGridSquare(Vector2 d)
        {
            int gridX = (int)(d.X / windX);
            int gridY = (int)(d.Y / windY);
            return gridSquares[gridX, gridY];
        }
    }
}
