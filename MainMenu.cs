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
    public partial class MainMenu : Form
    {
        // all of the sim stuff, will be fetched from menu screen later currently just initializing here
        private int particleCount = 10;
        private float smoothingRad = 2;

        private int gridSquareXCount = 5;
        private int gridSquareYCount = 5;

        private int msPerTick = 1;
        private float targetDensity = 2;
        private float pressureMultiplier = 1;
        private float gravity = 9.81f;

        public MainMenu()
        {
            InitializeComponent();
        }

        private void StartSimulation_btn_Click(object sender, EventArgs e)
        {
            Form SimWindow = new SimulationWindow(this, particleCount, smoothingRad,
                                                    gridSquareXCount, gridSquareYCount, 
                                                    msPerTick, targetDensity, pressureMultiplier, gravity);
            this.Hide();
            SimWindow.Show();
        }
    }
}
