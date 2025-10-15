using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Fluid_Sim_0._4
{
    abstract class SDFObject
    {
        public abstract float SDF(Vector2 d);
        // where d is the signed distance to the nearest object edge
    }
    class SDFBezierLinear : SDFObject
    {

    }
    class SDFBezierQuadratic : SDFObject
    {
        
    }
    class SDFBezierCubic : SDFObject
    {

    }

    class SDF_BezierShape : SDFObject
    {
        private List<SDFObject> lineSegments = new List<SDFObject>();
        public override float SDF(Vector2 d)
        {
            float minDist = float.MaxValue;
            foreach (var segment in lineSegments)
            {
                minDist = (float)Math.Min(minDist, segment.SDF(d));
            }
        }
    }
}
