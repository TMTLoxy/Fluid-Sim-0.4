using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fluid_Sim_0._4
{
    internal class ObjectEdge
    {
        private Vector2 vertex1;
        private Vector2 vertex2;
        private float grad;

        public ObjectEdge(Vector2 vertex1, Vector2 vertex2)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            calcGrad();
        }

        public void calcGrad()
        {
            float dy = vertex1.Y - vertex2.Y;
            float dx = vertex2.X - vertex1.X;
            grad = dy / dx;
        }
    }
}
