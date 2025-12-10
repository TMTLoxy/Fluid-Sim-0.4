using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
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
        #region Graphics & Menus
        private Form mainMenu;

        private float[,] densities;
        private int windX;
        private int windY;
        #endregion
        #region Partilcle Drawing (dev)
        private Brush[] colours = { Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Purple };
        private float rad = 5f;
        private int frameCount = 0;
        #endregion
        #region Particles
        private List<Particle> particles;
        private int particleCount;
        private float smoothingRad;
        #endregion
        #region Grid & Walls
        private float gridSquareWidth;
        private GridSquare[,] gridSquares;
        Vector2 gridSquareIndexMax;
        private List<Wall> walls;
        #endregion
        #region Sim Parameters
        private float timeInterval;
        private float g = 9.81f;
        private float vol;
        private float targetDensity;
        private float pressureMultiplier;
        #endregion
        public SimulationWindow(Form mainMenu,
            int particleCount, float smoothingRad,
            int interval, float targetDensity, float pressureMultiplier, float gravity)
        {
            // INITIALIZATION
            #region graphics initialization
            InitializeComponent();
            this.mainMenu = mainMenu;

            this.Paint += new PaintEventHandler(PaintParticles);
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true);

            windX = this.Width; // can be adjusted to fit in buttons or whatever
            windY = this.Height;
            #endregion
            #region sim parameters
            this.targetDensity = targetDensity;
            this.pressureMultiplier = pressureMultiplier;
            g = gravity;
            setClockInterval(interval);

            timeInterval = this.SimulationClock.Interval / 1000f; // seconds per tick
            vol = getVol(smoothingRad);
            #endregion
            # region particles
            this.smoothingRad = smoothingRad;
            this.particleCount = particleCount;

            particles = new List<Particle>();
            Vector2 initPos = new Vector2(500 / 2, 500 / 2);
            for (int i = 0; i < particleCount; i++)
            {
                particles.Add(new Particle(initPos));
                initPos += new Vector2(10f, 10f);
            }
            #endregion
            #region Grid & Walls
            walls = new List<Wall>();
            walls.Add(new VerticleWall(10, false, null));                  // left
            walls.Add(new VerticleWall(this.Width - 10, true, null));      // right
            walls.Add(new HorizontalWall(10, false, null));                // top
            walls.Add(new HorizontalWall(this.Height - 10, true, null));   // bottom
            // ioIndicator : true => outside the simulation is greater than the borderVal
            // currently no linked walls can add later once program is working (used mainly in wind tunnel)
            gridSquareWidth = smoothingRad * 20;
            gridSquareIndexMax = simGridSetup();
            #endregion
            
            // dev tools
        }

        public void SimulationClock_Tick(object sender, EventArgs e)
        {
            frameCount++;
            for (int i = 0; i < particleCount; i++)
            {
                Debug.WriteLine("Pos: " + Convert.ToString(particles[i].getPos()));
                Debug.WriteLine("Vel: " + Convert.ToString(particles[i].getVel()));

                // update predicted positions & apply gravity
                    particles[i].predictPositions(g, timeInterval);
                Debug.WriteLine("Predicted Pos: " + Convert.ToString(particles[i].getPredictedPos()));

                // update grid
                Vector2 gridSquare = particles[i].findGridSquare(gridSquareWidth); // runs off particle's predicted position
                Debug.WriteLine("GridSquare: " + Convert.ToString(particles[i].getGridSquare()));
                gridSquares[(int)gridSquare.X, (int)gridSquare.Y].addParticle(particles[i]);
                

                // particle calculations
                List<Particle> nearbyParticles = getNearbyParticles(particles[i].getGridSquare());
                particles[i].calculateDensity(vol, smoothingRad, nearbyParticles);
                
                Debug.WriteLine("Nearby Particles: ");
                for (int j = 0; j < nearbyParticles.Count; j++)
                {
                    Debug.WriteLine(nearbyParticles[j].getPos());
                }

                Vector2 pressureForce = calculatePressureGradient(particles[i].getPredictedPos(), nearbyParticles);
                particles[i].applyPressureForce(pressureForce, timeInterval);

                particles[i].updatePosition(timeInterval);
                // Wall Collisions
                if (frameCount != 0)
                    particles[i].wallCollisions(walls);

            }
            Debug.WriteLine("----");

            // draw next frame
            this.Refresh();
        }

        public float getVol(float s)
        {
            return (float)((1f / 6f) * Math.PI * s * s * (3 * s * s - 8 * s + 6));
        }

        public List<Particle> getNearbyParticles(Vector2 gridSquare)
        {
            int xMin = (int)gridSquare.X - 1;
            int xMax = (int)gridSquare.X + 1;

            int yMin = (int)gridSquare.Y - 1;
            int yMax = (int)gridSquare.X + 1;

            List<Particle> nearbyParticles = new List<Particle>();
            for (int i = xMin; i < xMax; i++)
            {
                for (int j = yMin; j < yMax; j++)
                {
                    if (i < 0 || j < 0 || i > gridSquares.GetLength(0) || j > gridSquares.GetLength(1)) continue; 
                    nearbyParticles.AddRange(gridSquares[i, j].getParticles());
                }
            }
            return nearbyParticles;
        }

        public Vector2 simGridSetup()
        {
            int gridCountX = (int)(windX / (gridSquareWidth)) + 1;
            int gridCountY = (int)(windY / (gridSquareWidth)) + 1;
            gridSquares = new GridSquare[gridCountX, gridCountY];
            for (int i = 0; i < gridCountX; i++)
            {
                for (int j = 0; j < gridCountY; j++)
                {
                    gridSquares[i, j] = new GridSquare(gridSquareWidth);
                }
            }
            return new Vector2(gridCountX, gridCountY);
        }

        public float smoothingKernalDerivative(float dist, float smoothingRad)
        {
            if (dist >= smoothingRad) return 0f;
            float coeff = dist / smoothingRad;
            float gradient = (2 * coeff - 2) / smoothingRad;
            return gradient;
        }

        public Vector2 calculatePressureGradient(Vector2 d, List<Particle> nearbyParticles)
        {
            Vector2 pressureGrad = Vector2.Zero;
            for (int i = 0; i < nearbyParticles.Count; i++)
            {
                if (d == nearbyParticles[i].getPredictedPos()) continue; // skip if self (this will also continue if two particles in same position but oh well)
                Vector2 predPos = nearbyParticles[i].getPredictedPos();
                float dist = Vector2.Distance(d, predPos);
                Vector2 dir = predPos - d / dist;

                float slope = smoothingKernalDerivative(dist, smoothingRad);
                float mass = nearbyParticles[i].getMass();
                float density = nearbyParticles[i].getDensity();
                pressureGrad += densityToPressure(density) * slope * dir * mass / density; // currently used for just pressure calcs but can be adapted for all properties
            }
            
            return -pressureGrad;
        }
        public float densityToPressure(float density)
        {
            float densityDif = density - targetDensity;
            float pressure = densityDif * pressureMultiplier;
            return pressure;
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