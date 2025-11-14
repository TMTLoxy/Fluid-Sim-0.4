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
        // -movement calculations for particles, get them moving around
        // -wall class where you can turn them on/ off & link them etc (simple object collisions as they straight lines)
        // -try getting a shape on screen
        // -get grid squares working, initialize them, get them being assigned correctly (mainly get sim running)
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }
    }
}
