using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Fluid_Sim_0._4
{
    public abstract class Wall
    {
        protected int borderVal;
        protected bool ioIndicator; // inside/outside, true => outside is greater than borderVal, false => less than
        protected Wall linkedWall;

        
        public Wall(int borderVal, bool ioIndicator, Wall linkedWall)
        {
            this.borderVal = borderVal;
            this.linkedWall = linkedWall;
            Debug.WriteLine("Wall borderVal: " + Convert.ToString(borderVal)); // DT
        }


        public abstract bool checkCollision(Particle incParticle);

        public abstract void wallCollision(Particle incParticle);
        public abstract void linkedWallCollision(Particle incParticle); // can be done later, not essential

        public void doCollision(Particle incParticle)
        {
            if (linkedWall == null) wallCollision(incParticle);
            else linkedWallCollision(incParticle);
        }
    }

    class VerticleWall : Wall
    {
        // reflect x vel
        public VerticleWall(int borderVal, bool ioIndicator, Wall linkedWall) : base(borderVal, ioIndicator, linkedWall)
        {
        }

        public override void wallCollision(Particle incParticle)
        {
            Debug.WriteLine(incParticle.getPrevPos());
            incParticle.setPos(incParticle.getPrevPos());
            incParticle.setVel(new Vector2(-incParticle.getVel().X, incParticle.getVel().Y));
        }
        public override bool checkCollision(Particle incParticle)
        {
            float prevPos = incParticle.getPrevPos().X;
            float pos = incParticle.getPos().X;
            if (prevPos < borderVal & pos >= borderVal & !ioIndicator) return true;
            else if (prevPos >= borderVal & pos < borderVal) return true;
            return false;
        }
        public override void linkedWallCollision(Particle incParticle)
        {
            
        }
    }
    class HorizontalWall : Wall
    {
        // reflect y vel
        public HorizontalWall(int borderVal, bool ioIndicator, Wall linkedWall) : base(borderVal, ioIndicator, linkedWall)
        {
        }

        public override void wallCollision(Particle incParticle)
        {
            incParticle.setPos(incParticle.getPrevPos());
            incParticle.setVel(new Vector2(incParticle.getVel().X, -incParticle.getVel().Y));
        }

        public override bool checkCollision(Particle incParticle)
        {
            float prevPos = incParticle.getPrevPos().Y;
            float  pos = incParticle.getPos().Y;
            if (prevPos < borderVal & pos >= borderVal & !ioIndicator) return true;
            else if (prevPos >= borderVal & pos < borderVal) return true;
            return false;
        }
        public override void linkedWallCollision(Particle incParticle)
        {

        }
    }
}