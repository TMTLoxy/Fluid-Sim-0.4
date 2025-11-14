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
//TO-DO
// only check closest 3 segments for sdf calc rather then all of them just to optimise

namespace Fluid_Sim_0._4
{
    // TO-DO list in Program.cs
    interface estNormal
    {
        Vector2 estimateNormal(Vector2 p);
        // estimates the normal at point p
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
        // where d is particle position

        public abstract float BoundaryDist(Vector2 centre);
        // gets distance from centre 

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

    class SDFBezier : SDFObject
    {
        private int degree;
        private Vector2 middle; // average of start and end points used to find closest edges to d
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
            middle = (segmentPoints[0] + (segmentPoints[segmentPoints.Count - 1])) / 2;
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

        public override float SDF(Vector2 d)
        {
            // for each segment, find the shortest distance to the segment from d
            float minDist = float.MaxValue;
            Vector2 closestPoint = new Vector2(0, 0);
            for (int i = 0; i < segments.Count - 1; i++)
            {
                // gets shortest distance to straight edge/segment
                Vector2 ba = segmentPoints[i + 1] - segmentPoints[i]; 
                float dotA = Vector2.Dot(d - segmentPoints[i], ba);
                float SqrBa = ba.X * ba.X + ba.Y * ba.Y;
                float t  = dotA / SqrBa;
                float dist = Math.Max(0, Math.Min(1, t));
                if (dist < minDist)
                {
                    minDist = dist;
                    closestPoint = segmentPoints[i] * t - ba;
                }
            }
            if (isInside(d, closestPoint, centre)) return 0 - minDist;
            return minDist;
        }
        public bool isInside(Vector2 d, Vector2 closestPoint, Vector2 centre)
        {
            // kinda iffy method but should work in almost all cases
            // checks distance from centre to closest point and distance from centre to d, if dist is < closest point then d is inside
            Vector2 distToCentre = centre - d;
            Vector2 centreToClosest = closestPoint - centre;
            if (distToCentre.LengthSquared() >= centreToClosest.LengthSquared()) return false;
            return true;
        }

        public override float BoundaryDist(Vector2 centre)
        {
            throw new NotImplementedException();
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

        public Vector2 getMiddle() => middle;
    }
    

    public class SDF_BezierShape : SDFObject, estNormal
    {
        private List<SDFBezier> lineSegments = new List<SDFBezier>();
        public SDF_BezierShape(Vector2 centre) : base(centre)
        {
            boundaryDist = BoundaryDist(centre);
        }
        public override float SDF(Vector2 d)
        {
            // eventually only use 3 closest line segments for efficiency

            // returns the mid distance to the shape from d, negative means inside shape, positive means outside
            float sdf = float.MaxValue;
            foreach (var segment in lineSegments)
            {
                sdf = (float)Math.Min(sdf, segment.SDF(d));
            }
            return sdf;
        }
        public Vector2 estimateNormal(Vector2 p)
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
    }
}
