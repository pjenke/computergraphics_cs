using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace computergraphics
{
	/**
	 * Draws a sphere with a given radius
	 * */
	public class SphereNode : LeafNode
	{
		/**
		 * Sphere radius
		 * */
		private float radius;

        /**
		 * Resolution of the sphere tesselation 
		 * */
        private int resolution;

        /**
		 * Vertex buffer object for the sphere
		*/
        private VertexBufferObject vbo = new VertexBufferObject();

        public float Radius
        {
            get
            {
                return radius;
            }
        }

        public SphereNode(float radius, int resolution)
		{
            this.radius = radius;
			this.resolution = resolution;
			CreateVBO();
		}

		public override void DrawGL(RenderMode mode, Matrix4 modelMatrix)
		{
			if (mode == RenderMode.REGULAR)
			{
				vbo.Draw();
			}
		}

        public override void UpdateTriangles(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Triangles.Add(p1);
            Triangles.Add(p2);
            Triangles.Add(p3);
        }

        public override void TimerTick(int counter)
		{
		}

		/**
		 * Flat rendering of the surface triangles.
		 * */
		private void CreateVBO()
		{
			List<RenderVertex> renderVertices = new List<RenderVertex>();

			Color4 color = Color4.MediumSeaGreen;
			float dTheta = (float)(Math.PI / resolution);
			float dPhi = (float)(Math.PI * 2.0 / resolution);
			for (int i = 0; i < resolution; i++)
			{
				for (int j = 0; j < resolution; j++)
				{
					Vector3 p0 = EvaluateSpherePoint(i * dTheta, j * dPhi);
					Vector3 p1 = EvaluateSpherePoint(i * dTheta, (j + 1) * dPhi);
					Vector3 p2 = EvaluateSpherePoint((i + 1) * dTheta, (j + 1) * dPhi);
					Vector3 p3 = EvaluateSpherePoint((i + 1) * dTheta, j * dPhi);
					Vector3 normal = Vector3.Cross(Vector3.Subtract(p2, p0), Vector3.Subtract(p1, p0)).Normalized();

					AddSideVertices(renderVertices, p0, p1, p2, p3, normal, color);
				}
			}

			vbo.Setup(renderVertices, PrimitiveType.Quads);
		}

		/**
		 * Compute a sphere surface point for two angles-parameterization 
		 * */
		private Vector3 EvaluateSpherePoint(float theta, float phi)
		{
			float x = (float)(radius * Math.Sin(theta) * Math.Cos(phi));
			float y = (float)(radius * Math.Sin(theta) * Math.Sin(phi));
			float z = (float)(radius * Math.Cos(theta));
			return new Vector3(x, y, z);
		}

		/**
		 * Add 4 vertices to the render list.
		 * */
		private void AddSideVertices(List<RenderVertex> renderVertices, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 normal, Color4 color)
		{
			renderVertices.Add(new RenderVertex(p3, normal, color));
			renderVertices.Add(new RenderVertex(p2, normal, color));
			renderVertices.Add(new RenderVertex(p1, normal, color));
			renderVertices.Add(new RenderVertex(p0, normal, color));
            UpdateTriangles(p0, p1, p2);
            UpdateTriangles(p0, p1, p3);
        }
	}
}

