using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluid_Sim_0._4
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        // TO-DO

        // Graphics
        // - integrate bitmaps
        // - switch over to using bitmap images instead of drawing particles (can be kept for dev stuff)

        // Simulation
        // - Fix the grid calculations for the particles to then get them moving correctly
        // - Get any sort of bezier shape on screen
        // - particle collisions with shapes (debug) ((gonna be a pain))
        // - Linked walls and particles being moved to the other side correctly

        // Optimization
        // - convert all the for loops to Parallel.For where possible 

        // GUI & Front End
        // - Initial loading screen / main menu
        // - Settings / setup screen before simulation

        // Random Notes
        // - grid index is currently getting messed up idk why
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }
    }
}
