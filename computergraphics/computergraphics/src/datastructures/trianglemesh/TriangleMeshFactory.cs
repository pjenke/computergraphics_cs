using System;
using OpenTK;
using System.Collections.Generic;

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
		public static void CreateCube(ITriangleMesh mesh, float sideLength)
		{
			mesh.Clear();
			mesh.AddVertex(new Vector3(-sideLength / 2.0f, -sideLength / 2.0f, -sideLength / 2.0f));
			mesh.AddVertex(new Vector3(sideLength / 2.0f, -sideLength / 2.0f, -sideLength / 2.0f));
			mesh.AddVertex(new Vector3(sideLength / 2.0f, sideLength / 2.0f, -sideLength / 2.0f));
			mesh.AddVertex(new Vector3(-sideLength / 2.0f, sideLength / 2.0f, -sideLength / 2.0f));
			mesh.AddVertex(new Vector3(-sideLength / 2.0f, -sideLength / 2.0f, sideLength / 2.0f));
			mesh.AddVertex(new Vector3(sideLength / 2.0f, -sideLength / 2.0f, sideLength / 2.0f));
			mesh.AddVertex(new Vector3(sideLength / 2.0f, sideLength / 2.0f, sideLength / 2.0f));
			mesh.AddVertex(new Vector3(-sideLength / 2.0f, sideLength / 2.0f, sideLength / 2.0f));

			mesh.AddTriangle(new Triangle(0, 2, 1));
			mesh.AddTriangle(new Triangle(0, 3, 2));
			mesh.AddTriangle(new Triangle(1, 2, 5));
			mesh.AddTriangle(new Triangle(2, 6, 5));
			mesh.AddTriangle(new Triangle(4, 5, 6));
			mesh.AddTriangle(new Triangle(4, 6, 7));
			mesh.AddTriangle(new Triangle(4, 7, 0));
			mesh.AddTriangle(new Triangle(0, 7, 3));
			mesh.AddTriangle(new Triangle(3, 7, 2));
			mesh.AddTriangle(new Triangle(2, 7, 6));
			mesh.AddTriangle(new Triangle(4, 0, 1));
			mesh.AddTriangle(new Triangle(1, 5, 4));
			mesh.ComputeTriangleNormals();
		}

		/**
		 * Create sphere.
		 * */
		public static void CreateSphere(ITriangleMesh mesh, float radius, int resolution)
		{
			mesh.Clear();
			float dTheta = (float)(Math.PI / (resolution + 1));
			float dPhi = (float)(Math.PI * 2.0 / resolution);
			// Create vertices

			// 0-180 degrees: i, theta
			for (int i = 0; i < resolution; i++)
			{

				// 0-360 degres: j, phi
				for (int j = 0; j < resolution; j++)
				{
					Vector3 p0 = EvaluateSpherePoint((i + 1) * dTheta, j * dPhi, radius);
					mesh.AddVertex(p0);
				}
			}
			int leftIndex = mesh.AddVertex(new Vector3(0, 0, radius));
			int rightIndex = mesh.AddVertex(new Vector3(0, 0, -radius));
			// Add triangles
			for (int i = 0; i < resolution - 1; i++)
			{
				for (int j = 0; j < resolution; j++)
				{
					mesh.AddTriangle(GetSphereIndex(i, j, resolution), GetSphereIndex(i + 1, j, resolution), GetSphereIndex(i + 1, j + 1, resolution));
					mesh.AddTriangle(GetSphereIndex(i, j, resolution), GetSphereIndex(i + 1, j + 1, resolution), GetSphereIndex(i, j + 1, resolution));
				}
			}
			for (int j = 0; j < resolution; j++)
			{
				mesh.AddTriangle(GetSphereIndex(0, j, resolution), GetSphereIndex(0, (j + 1) % resolution, resolution), leftIndex);
				mesh.AddTriangle(GetSphereIndex(resolution - 1, j, resolution), rightIndex, GetSphereIndex(resolution - 1, (j + 1) % resolution, resolution));
			}

			mesh.ComputeTriangleNormals();
		}

		private static Vector3 EvaluateSpherePoint(float theta, float phi, float radius)
		{
			float x = (float)(radius * Math.Sin(theta) * Math.Cos(phi));
			float y = (float)(radius * Math.Sin(theta) * Math.Sin(phi));
			float z = (float)(radius * Math.Cos(theta));
			return new Vector3(x, y, z);
		}

		private static int GetSphereIndex(int i, int j, int resolution)
		{
			return (i % resolution) * resolution + (j % resolution);
		}

		public static void CreateSquare(ITriangleMesh mesh, float extend)
		{
			mesh.Clear();
			mesh.AddVertex(new Vector3(-extend, 0, -extend));
			mesh.AddVertex(new Vector3(extend, 0, -extend));
			mesh.AddVertex(new Vector3(extend, 0, extend));
			mesh.AddVertex(new Vector3(-extend, 0, extend));
			mesh.AddTriangle(0, 2, 1);
			mesh.AddTriangle(0, 3, 2);
			mesh.ComputeTriangleNormals();
		}

		/**
		 * Generated the unification of two meshes. Attention: no new mesh is generated, in fact the 
		 * geometry of mesh2 is added to mesh1.
		 */
		public static void Unite(ITriangleMesh mesh1, ITriangleMesh mesh2)
		{
			int numberOfVertsMesh1 = mesh1.GetNumberOfVertices();
			int numberOfTexCoordsMesh1 = mesh1.GetNumberOfTexCoords();
			for (int i = 0; i < mesh2.GetNumberOfVertices(); i++)
			{
				mesh1.AddVertex(mesh2.GetVertex(i));
			}
			for (int i = 0; i < mesh2.GetNumberOfTexCoords(); i++)
			{
				mesh1.AddTextureCoordinate(mesh2.GetTextureCoordinate(i));
			}
			for (int i = 0; i < mesh2.GetNumberOfTriangles(); i++)
			{
				Triangle t = mesh2.GetTriangle(i).Clone();
				t.VertexIndexOffset(numberOfVertsMesh1);
				t.TexCoordsIndexOffset(numberOfTexCoordsMesh1);
				mesh1.AddTriangle(t);
			}
			mesh1.ComputeTriangleNormals();
		}

		public static ITriangleMesh Snap(ITriangleMesh mesh, float epsilon)
		{
			Dictionary<int, int> vertexMapping = new Dictionary<int, int>();
			ITriangleMesh result = new TriangleMesh();
			for (int i = 0; i < mesh.GetNumberOfVertices(); i++)
			{
				Vector3 vi = mesh.GetVertex(i);
				bool found = false;
				for (int j = 0; j < result.GetNumberOfVertices(); j++)
				{
					Vector3 vj = result.GetVertex(j);
					if (Vector3.Subtract(vi, vj).Length < epsilon)
					{
						vertexMapping[i] = j;
						found = true;
					}
				}
				if (!found)
				{
					int idx = result.AddVertex(vi);
					vertexMapping[i] = idx;
				}
			}

			for (int i = 0; i < mesh.GetNumberOfTexCoords(); i++)
			{
				result.AddTextureCoordinate(mesh.GetTextureCoordinate(i));
			}

			for (int i = 0; i < mesh.GetNumberOfTriangles(); i++)
			{
				Triangle t = mesh.GetTriangle(i).Clone();
				t.A = vertexMapping[t.A];
				t.B = vertexMapping[t.B];
				t.C = vertexMapping[t.C];
				result.AddTriangle(t);
			}

			Console.WriteLine("Mesh snapping: " + mesh.GetNumberOfVertices() + " -> " + result.GetNumberOfVertices() + " verts.");
			return result;
		}

		/**
		 * Flip triangle normal orientation.
		 * */
		public static void Flip(ITriangleMesh mesh)
		{
			for (int i = 0; i < mesh.GetNumberOfTriangles(); i++)
			{
				mesh.GetTriangle(i).Flip();
			}
		}

		public static void Translate(ITriangleMesh mesh, Vector3 translation)
		{
			for (int i = 0; i < mesh.GetNumberOfVertices(); i++)
			{
				mesh.SetVertex(i, Vector3.Add(mesh.GetVertex(i), translation));
			}
		}
	}
}

