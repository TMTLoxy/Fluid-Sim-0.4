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
        public MainMenu()
        {
            InitializeComponent();
        }

        private void StartSimulation_btn_Click(object sender, EventArgs e)
        {
            Form SimWindow = new SimulationWindow(this);
            this.Hide();
            SimWindow.Show();
        }
    }
}
