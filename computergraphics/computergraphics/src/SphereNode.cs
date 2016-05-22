using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Draws a sphere with a given radius
	 * */
	public class SphereNode : INode
	{
		/**
		 * Sphere radius
		 * */
		private float radius;

		/**
		 * Resolution of the sphere tesselation 
		 * */
		private int resolution;

		public SphereNode (float radius, int resolution)
		{
			this.radius = radius;
			this.resolution = resolution;
		}

		public void Draw ()
		{
			GL.Begin (PrimitiveType.Triangles);
			GL.Color4 (Color4.Aquamarine);
			GL.Normal3 (0, 0, -1);

			float dTheta = (float)(Math.PI / resolution);
			float dPhi = (float)(Math.PI * 2.0 / resolution);
			for (int i = 0; i < resolution; i++) {
				for (int j = 0; j < resolution; j++) {
					Vector3 p0 = evaluateSpherePoint (i * dTheta, j * dPhi);
					Vector3 p1 = evaluateSpherePoint (i * dTheta, (j + 1) * dPhi);
					Vector3 p2 = evaluateSpherePoint ((i + 1) * dTheta, (j + 1) * dPhi);
					Vector3 p3 = evaluateSpherePoint ((i + 1) * dTheta, j * dPhi);
					Vector3 normal = evaluateSpherePoint ((i + 0.5f) * dTheta, (j + 0.5f) * dPhi).Normalized ();
					GL.Normal3 (normal);
					GL.Vertex3 (p0);
					GL.Vertex3 (p1);
					GL.Vertex3 (p2);
					GL.Vertex3 (p0);
					GL.Vertex3 (p2);
					GL.Vertex3 (p3);
				}
			}

			GL.End ();
		}

		private Vector3 evaluateSpherePoint (float theta, float phi)
		{
			float x = (float)(radius * Math.Sin (theta) * Math.Cos (phi));
			float y = (float)(radius * Math.Sin (theta) * Math.Sin (phi));
			float z = (float)(radius * Math.Cos (theta));
			return new Vector3 (x, y, z);
		}
	}
}

