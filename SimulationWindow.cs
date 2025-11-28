using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private float smoothingRad;

        private GridSquare[,] gridSquares;
        private float gridSquareWidth;
        private float gridSquareHeight;
        private List<Wall> walls;

        private float timeInterval;
        private float g = 9.81f;
        private float vol;
        public SimulationWindow(Form mainMenu, 
            int particleCount, float smoothingRad, 
            int gridSquareXCount, int gridSquareYCount, 
            int interval)
        {
            InitializeComponent();

            // sim initialization
            setClockInterval(interval);

            vol = getVol(smoothingRad); // need to assign smoothingRad

            this.particleCount = particleCount; // can be made to be adjustable later
            particles = new List<Particle>();
            Vector2 initPos = new Vector2(500 / 2,500/ 2);
            for (int i = 0; i < particleCount; i++)
            {
                particles.Add(new Particle(initPos));
                initPos += new Vector2(1f, 1f); 
            }

            simGridSetup(gridSquareXCount, gridSquareYCount);

            // set walls
            walls = new List<Wall>();
            walls.Add(new VerticleWall(10, false, null));              // left
            walls.Add(new VerticleWall(this.Width - 10, true, null));      // right
            walls.Add(new HorizontalWall(10, false, null));            // top
            walls.Add(new HorizontalWall(this.Height - 10, true, null));   // bottom
            // ioIndicator : true => outside the simulation is greater than the borderVal
            // currently no linked walls can add later once program is working (used mainly in wind tunnel)

            timeInterval = this.SimulationClock.Interval / 1000f; // seconds per tick

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
            frameCount++;

            // - Object collision check
            // - Particle collision stuff
            // - graphics refresh
            for (int i = 0; i < particleCount; i++)
            {
                //Debug.WriteLine("PrevPos: " + particles[i].getPrevPos().X + ", " + particles[i].getPrevPos().Y); // DT
                //Debug.WriteLine("Pos: " + particles[i].getPos().X + ", " + particles[i].getPos().Y); // DT

                // Collision algorithm
                // check ,for each particle, against the contents of it's grid square and it's adjacent squares
                // get current particle's square
                // for squares of index i, +- 1 create a list of particles in all those squares
                // run collision checks against all object edges in that square and adjacent 
                // run collision checks for all of those particles

                // Particle Collisions
                List<Particle> nearbyParticles = getNearbyParticles(particles[i].getPos());
                for (int p = 0; p < nearbyParticles.Count; p++)
                {
                    if (nearbyParticles[p] != particles[i])
                    {
                        particles[i].particleCollisionCheck(nearbyParticles[p]);
                    }
                }
                // Wall Collisions
                if(frameCount != 0 )
                   particles[i].wallCollisions(walls);

                particles[i].Update(timeInterval, g, gridSquareWidth, gridSquareHeight);
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
                    nearbyParticles.AddRange(gridSquares[i, j].getParticles());
                }
            }
            return nearbyParticles;
        }

        public void simGridSetup(int gridSquareXCount, int gridSquareYCount)
        {
            gridSquareWidth = this.Width / gridSquareXCount;
            gridSquareHeight = this.Height / gridSquareYCount;
            gridSquares = new GridSquare[gridSquareXCount, gridSquareYCount];
            for (int i = 0; i < gridSquareXCount; i++)
            {
                for (int j = 0; j < gridSquareYCount; j++)
                {
                    gridSquares[i, j] = new GridSquare(new Vector2(i, j), gridSquareWidth, gridSquareHeight);
                }
            }
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

        private void setClockInterval(int interval)
        {
            SimulationClock.Interval = interval;
        }


        // GUI stuff
        private void EndSim_btn_Click(object sender, EventArgs e)
        {
            mainMenu.Show();
            this.Close();
        }

        private void PauseSim_btn_Click(object sender, EventArgs e)
        {
            if (SimulationClock.Enabled) SimulationClock.Enabled = false;
            else SimulationClock.Enabled = true;
        }
    }
}