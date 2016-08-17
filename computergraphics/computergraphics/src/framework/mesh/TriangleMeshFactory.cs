using System;
using OpenTK;

namespace computergraphics
{
	/**
	 * Create triangle meshes.
	 * */
	public class TriangleMeshFactory
	{
		/**
		 * Create cube.
		 * */
		public static void CreateCube (ITriangleMesh mesh, float sideLength)
		{
			mesh.Clear ();
			mesh.AddVertex (new Vector3 (-sideLength/2.0f, -sideLength / 2.0f, -sideLength / 2.0f));
			mesh.AddVertex (new Vector3 (sideLength / 2.0f, -sideLength / 2.0f, -sideLength / 2.0f));
			mesh.AddVertex (new Vector3 (sideLength / 2.0f, sideLength / 2.0f, -sideLength / 2.0f));
			mesh.AddVertex (new Vector3 (-sideLength / 2.0f, sideLength / 2.0f, -sideLength / 2.0f));
			mesh.AddVertex (new Vector3 (-sideLength / 2.0f, -sideLength / 2.0f, sideLength / 2.0f));
			mesh.AddVertex (new Vector3 (sideLength / 2.0f, -sideLength / 2.0f, sideLength / 2.0f));
			mesh.AddVertex (new Vector3 (sideLength / 2.0f, sideLength / 2.0f, sideLength / 2.0f));
			mesh.AddVertex (new Vector3 (-sideLength / 2.0f, sideLength / 2.0f, sideLength / 2.0f));

			mesh.AddTriangle (new Triangle (0, 2, 1));
			mesh.AddTriangle (new Triangle (0, 3, 2));
			mesh.AddTriangle (new Triangle (1, 2, 5));
			mesh.AddTriangle (new Triangle (2, 6, 5));
			mesh.AddTriangle (new Triangle (4, 5, 6));
			mesh.AddTriangle (new Triangle (4, 6, 7));
			mesh.AddTriangle (new Triangle (4, 7, 0));
			mesh.AddTriangle (new Triangle (0, 7, 3));
			mesh.AddTriangle (new Triangle (3, 7, 2));
			mesh.AddTriangle (new Triangle (2, 7, 6));
			mesh.AddTriangle (new Triangle (4, 0, 1));
			mesh.AddTriangle (new Triangle (1, 5, 4));
			mesh.ComputeTriangleNormals ();
		}

		/**
		 * Create sphere.
		 * */
		public static void CreateSphere (ITriangleMesh mesh, float radius, int resolution)
		{
			mesh.Clear ();
			float dTheta = (float)(Math.PI / (resolution + 1));
			float dPhi = (float)(Math.PI * 2.0 / resolution);
			// Create vertices

			// 0-180 degrees: i, theta
			for (int i = 0; i < resolution; i++) {

				// 0-360 degres: j, phi
				for (int j = 0; j < resolution; j++) {
					Vector3 p0 = EvaluateSpherePoint ((i+1) * dTheta, j * dPhi, radius);
					mesh.AddVertex (p0);
				}
			}
			int leftIndex = mesh.AddVertex (new Vector3 (0, 0, radius ));
			int rightIndex = mesh.AddVertex (new Vector3 (0, 0, -radius ));
			// Add triangles
			for (int i = 0; i < resolution - 1; i++) {
				for (int j = 0; j < resolution; j++) {
					mesh.AddTriangle (GetSphereIndex (i, j, resolution), GetSphereIndex (i + 1, j, resolution), GetSphereIndex (i + 1, j + 1, resolution));
					mesh.AddTriangle (GetSphereIndex (i, j, resolution), GetSphereIndex (i + 1, j + 1, resolution), GetSphereIndex (i, j + 1, resolution));
				}
			}
			for (int j = 0; j < resolution; j++) {
				mesh.AddTriangle (GetSphereIndex (0, j, resolution), GetSphereIndex (0, (j+1)%resolution, resolution), leftIndex);
				mesh.AddTriangle (GetSphereIndex (resolution-1, j, resolution), rightIndex, GetSphereIndex (resolution-1, (j+1)%resolution, resolution) );
			}

			mesh.ComputeTriangleNormals ();
		}

		private static Vector3 EvaluateSpherePoint (float theta, float phi, float radius)
		{
			float x = (float)(radius * Math.Sin (theta) * Math.Cos (phi));
			float y = (float)(radius * Math.Sin (theta) * Math.Sin (phi));
			float z = (float)(radius * Math.Cos (theta));
			return new Vector3 (x, y, z);
		}

		private static int GetSphereIndex (int i, int j, int resolution)
		{
			return (i % resolution) * resolution + (j % resolution);
		}

		public static void CreateSquare(ITriangleMesh mesh, float extend)
		{
			mesh.Clear ();
			mesh.AddVertex (new Vector3(-extend, 0, -extend));
			mesh.AddVertex (new Vector3(extend, 0, -extend));
			mesh.AddVertex (new Vector3(extend, 0, extend));
			mesh.AddVertex (new Vector3(-extend, 0, extend));
			mesh.AddTriangle (0, 2, 1);
			mesh.AddTriangle (0, 3, 2);
			mesh.ComputeTriangleNormals ();
		}
	}
}

