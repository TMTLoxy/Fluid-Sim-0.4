using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        private List<GridSquare> gridSquares;
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
