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

namespace Fluid_Sim_0._4
{
    public partial class SimulationWindow : Form
    {
        // graphics stuff
        private float[,] densities;

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

        }

    }
}
