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
    // TO-DO list in Program.cs
    public partial class SimulationWindow : Form
    {
        // graphics stuff
        private Form mainMenu;

        private float[,] densities;
        private int windX;
        private int windY;

            // dev particle drawing things
        private Brush[] colours = { Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Purple };
        private float rad = 5f;
        private int frameCount = 0;

        // sim stuff
        private List<Particle> particles;
        private int particleCount;
        private GridSquare[,] gridSquares;
        private float vol;
        private float smoothingRad;
        public SimulationWindow(Form mainMenu)
        {
            InitializeComponent();

            // sim initialization
            vol = getVol(smoothingRad); // need to assign smoothingRad

            particleCount = 100; // can be made to be adjustable later
            particles = new List<Particle>();
            Vector2 initPos = new Vector2(this.Width / 2, this.Height / 2);
            for (int i = 0; i < particleCount - 1; i++)
            {
                particles.Add(new Particle(initPos));
                initPos += new Vector2(1f, 1f); 
            }

            // graphics initialization
            this.mainMenu = mainMenu;

            this.Paint += new PaintEventHandler(PaintParticles);
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true);

            windX = this.Width; // can be adjusted to fit in buttons or whatever
            windY = this.Height;
            // dev tools

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

            // draw next frame
            this.Refresh();
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


        // GRAPHICS

            // dev particle drawing
        private void PaintParticles(object sender, PaintEventArgs e)
        {
            // graphics settings
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // wipe previous frame
            e.Graphics.Clear(this.BackColor);

            // draw new frame

            for (int i = 0; i < particles.Count; i++)
            {
                e.Graphics.FillEllipse(colours[i % 6], particles[i].getPos().X - rad, particles[i].getPos().Y - rad, rad * 2, rad * 2);
            }
        }


        // GUI stuff
        private void EndSim_btn_Click(object sender, EventArgs e)
        {
            mainMenu.Show();
            this.Close();
        }
    }
}