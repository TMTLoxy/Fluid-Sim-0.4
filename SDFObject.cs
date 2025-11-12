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
using System.Runtime.InteropServices;
using System.Windows.Forms;

// redo all of the sdf stuff so that 
// if a particle is in object detection boundary
// check it's distance to a bunch of points along the object ( use dist^2 to avoid sqrt )
// get the points from the segment points of each curve
// once you got the closest point check the segments of the curve either side of the point and get the sdf from there using the object winding 
// (sack off doing the sdf for each curve as adds compexity)

namespace Fluid_Sim_0._4
{
    interface IWinding
    {
        float getWinding(Vector2 p);
        // find the total angle for a segment around point p
    }
    public abstract class SDFObject
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
        public int F(int n)
        {
            for (int i = 1; i < n; i++)
            {
                n *= n - i;
            }
            return n;
        }
        public float Bi(int n, int i)
        {
            int nF = F(n);
            int iF = F(i);
            int inF = F(n - i);
            return nF / (iF * inF);
        }

        public Vector2 getCentre() => centre;
        public float getBoundaryDist() => boundaryDist;
    }

    class SDFBezier : SDFObject, IWinding
    {
        private int degree;
        private List<Vector2> referancePoints;
        private List<Vector2> segments; // used before running to find normals and stuff
        private List<Vector2> segmentPoints;
        private int segmentPointsCount;

        public SDFBezier(Vector2 centre, int degree, List<Vector2> referancePoints) : base(centre)
        {
            this.degree = degree;
            this.referancePoints = referancePoints;
            generateSegments();
            segmentPointsCount = segments.Count;
        }

        public Vector2 BezierEquation(float t)
        {
            // finds a point on any bezier curve given value t which is a value 0-1 and indicates how far along the curve the point is
            Vector2 p = new Vector2(0, 0);
            for (int i = 0; i < degree; i++)
            {
                float bi = Bi(degree, i);
                float exp = (float)Math.Pow(1 - t, degree - i) * (float)Math.Pow(t, i);
                p += bi * exp * referancePoints[i];
            }
            return p;
        }

        public override float SDF(Vector2 d, float influenceRad, SDF_BezierShape obj)
        {
            // for each segment, find the shortest distance to the segment from d
            float minDist = float.MaxValue;
            for (int i = 0; i < segments.Count - 1; i++)
            {
                // gets shortest distance to straight edge/segment
                Vector2 ba = segmentPoints[i + 1] - segmentPoints[i]; 
                float dotA = Vector2.Dot(d - segmentPoints[i], ba);
                float SqrBa = ba.X * ba.X + ba.Y * ba.Y;
                float t  = dotA / SqrBa;
                float dist = Math.Max(0, Math.Min(1, t));
                if (dist < minDist) minDist = dist;
            }
            if (minDist < influenceRad) bool inObj = obj.ComputeWinding(d, segments);
        }
        public override Vector2 estimateNormal(Vector2 p)
        {
            // just a place holder that was copy and pasted from SDF bezier shape, will not be used and will remove later
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
            // finds the total angle covered by a curve from point p
            float totalWinding = 0;
            Vector2 a = segmentPoints[0] - p;
            for (int i = 0; i < segments.Count - 1; i++)
            {
                Vector2 b = segmentPoints[i + 1] - p;
                float dot = Vector2.Dot(a, b);
                float aSqr = a.X * a.X + a.Y * a.Y;
                float bSqr = b.X * b.X + b.Y * b.Y;
                totalWinding += (float)Math.Acos(dot / (float)Math.Sqrt(aSqr * bSqr));
                a = b;
            }
            return totalWinding;
        }
        public void generateSegments()
        {
            // generates a list of vectors along the curve (segments) that are used to approximate the curve during particle collisions
            float tInterval = 0.01f;
            float t = 0;
            Vector2 lastPoint = BezierEquation(t);
            segmentPoints.Add(lastPoint);
            for (int i = 1; i < 1 / tInterval; i++)
            {
                t += tInterval;
                Vector2 newPoint = BezierEquation(t);
                segmentPoints.Add(newPoint);
                segments.Add(newPoint - lastPoint);
                lastPoint = newPoint;
            }
        }
    }
    

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
