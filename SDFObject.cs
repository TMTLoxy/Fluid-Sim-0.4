using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using System.Drawing;
using System.Security.Policy;
using System.Diagnostics;
using System.Globalization;

namespace Fluid_Sim_0._4
{
    interface IWinding
    {
        float getWinding(Vector2 p);
        // find the total angle for a segment around point p
    }
    abstract class SDFObject
    {
        protected float boundaryDist;
        protected Vector2 centre;
        public SDFObject(Vector2 centre)
        {
            this.centre = centre;
            boundaryDist = BoundaryDist(centre);
        }
        public abstract float SDF(Vector2 d);
        // where d is the signed distance to the nearest object edge

        public abstract float BoundaryDist(Vector2 centre);
        // gets distance from centre 

        public abstract Vector2 estimateNormal(Vector2 p);

        public Vector2 getCentre() => centre;
        public float getBoundaryDist() => boundaryDist;
    }

    class SDFBezier : SDFObject, IWinding
    {
        private List<Vector2> samplePoints;
        private int samplePointsCount;

        public SDFBezier(Vector2 centre, int degree) : base(centre)
        {
            samplePoints = new List<Vector2>(degree);
            samplePointsCount = samplePoints.Count;
        }

        public override float SDF(Vector2 d)
        {
            // do a bunch of back propagation stuff idk find closest distance
        }
        public override Vector2 estimateNormal(Vector2 p)
        {
            float eps = 0.001f;
            float dx = SDF(p + new Vector2(eps, 0)) - SDF(p - new Vector2(eps, 0));
            float dy = SDF(p + new Vector2(0, eps)) - SDF(p - new Vector2(0, eps));
            Vector2 n = new Vector2(dx, dy);
            return Vector2.Normalize(n);
        }

        public override float BoundaryDist(Vector2 centre)
        {
            throw new NotImplementedException();
        }

        public float getWinding(Vector2 p)
        {
            float totalWinding = 0;
            Vector2 a = samplePoints[0] - p;
            for (int i = 1; i < samplePointsCount; i++)
            {
                Vector2 b = samplePoints[i] - p;
                float dot = Vector2.Dot(a, b);
                float aSqr = a.X * a.X + a.Y * a.Y;
                float bSqr = b.X * b.X + b.Y * b.Y;
                totalWinding += (float)Math.Acos(dot / (float)Math.Sqrt(aSqr * bSqr));
                a = b;
            }
            return totalWinding;
        }
    }
    // once generalised, sample point num = the power of curve (ie quadratic = 2), then number of sample = 2 + 1 = 3

    class SDF_BezierShape : SDFObject
    {
        private List<SDFBezier> lineSegments = new List<SDFBezier>();
        public SDF_BezierShape(Vector2 centre) : base(centre)
        {
            boundaryDist = BoundaryDist(centre);
        }
        public override float SDF(Vector2 d)
        {
            // returns the mid distance to the shape from d, negative means inside shape, positive means outside
            float minDist = float.MaxValue;
            foreach (var segment in lineSegments)
            {
                minDist = (float)Math.Min(minDist, segment.SDF(d));
            }

            bool insideShape = ComputeWinding(d, lineSegments);
            return insideShape ? -minDist : minDist;
        }
        public override Vector2 estimateNormal(Vector2 p)
        {
            // estimates a normal vector using very small epsilon value to go either side of the point and create a normal based of the SDF values returned at those points
            float eps = 0.001f;
            float dx = SDF(p + new Vector2(eps, 0)) - SDF(p - new Vector2(eps, 0));
            float dy = SDF(p + new Vector2(0, eps)) - SDF(p - new Vector2(0, eps));
            Vector2 n = new Vector2(dx, dy);
            return Vector2.Normalize(n);
        }

        public override float BoundaryDist(Vector2 centre)
        {
            float maxDist = 0;
            foreach (var segment in lineSegments)
            {
                maxDist = (float)Math.Max(maxDist, segment.BoundaryDist(centre));
            }
            // + 20 is a random amount, is thickens of boundary, thicker = more accurate but slower to run
            return maxDist + 20;
        }

        public bool ComputeWinding(Vector2 p, List<SDFBezier> lineSegments)
        {
            // if winding angle is multiple of 2Pi then p is inside, else outside
            float totalAngle = 0;
            for (int i = 0; i < lineSegments.Count(); i++)
            {
                totalAngle += lineSegments[i].getWinding(p);
            }
            float angleMod = totalAngle % (2 * (float)Math.PI);
            return angleMod == 0 ? true : false;
        }
    }
}
