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
        // this version (0.4) is entirely new and has been completely rewritten since the last code check (version 0.3)

        // TO-DO
        // -convert all the for loops to Parallel.For where possible 
        // -movement calculations for particles, get them moving around
        // -wall class where you can turn them on/ off & link them etc (simple object collisions as they straight lines)
        // -try getting a shape on screen
        // -get grid squares working, initialize them, get them being assigned correctly (mainly get sim running)
        // -particle collision method

        // Current Notes
        // -grid system is broken, nearby particle is returning an empty list / list with no items
        // -redo al the grid stuff probably with that nvidia paper
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }
    }
}
