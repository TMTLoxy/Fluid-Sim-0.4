using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Fluid_Sim_0._4
{
    internal class Object
    {
        private List<Vector2> vertices;
        private List<ObjectEdge> edges;

        private Vector2 centrePos;
        private float detectionBoxRad;
        private int vertexCount;


        public Object(List<Vector2> vertices)
        {
            this.vertices = vertices;
            DrawEdges();
            CalculateCentrePos();
            CalculateDetectionBoxRad();
            vertexCount = vertices.Count;
        }

        public void DrawEdges()
        {
            edges = new List<ObjectEdge>(vertexCount - 1);
            for (int i = 0; i < vertexCount; i++)
            {
                edges[i] = new ObjectEdge(vertices[i], vertices[(i + 1) % vertexCount]);
            }
        }

        public void CalculateCentrePos()
        {
            float xTotal = 0;
            float yTotal = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                xTotal += vertices[i].X;
                yTotal += vertices[i].Y;
            }
            centrePos =  new Vector2(xTotal / vertices.Count, yTotal / vertices.Count);
        }

        public void CalculateDetectionBoxRad()
        {
            float maxDist = 0;
            for (int i = 0; i < edges.Count; i++)
            {
                float dy = vertices[i].Y - centrePos.Y;
                float dx = vertices[i].X - centrePos.X;
                float dist = (float)Math.Sqrt(dy * dy + dx * dx);
                if (dist > maxDist) maxDist = dist;
            }
            detectionBoxRad = maxDist;
        }
    }
}
