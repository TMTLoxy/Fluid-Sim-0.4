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
        private GridSquare[,] gridSquares;
        private int gridSquaresYCount;
        private int gridSquaresXCount;
        public SimulationWindow()
        {
            InitializeComponent();
        }

        private void SimulationClock_Tick(object sender, EventArgs e)
        {
            // Clear all the gridSquares so their particle List's can be appended at the end of the function
            for (int i = 0; i < gridSquaresYCount; i++)
            {
                for (int j = 0; j < gridSquaresXCount; j++)
                {
                    gridSquares[i, j].Clear();
                }
            }

            // - Object collision check
            // - Particle collision stuff
            // - graphics refresh

            refreshDensities();
        }

        public void refreshDensities()
        {

        }
    }
}
