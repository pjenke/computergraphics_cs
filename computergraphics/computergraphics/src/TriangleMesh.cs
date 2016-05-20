using System;
using OpenTK;
using System.Collections.Generic;

namespace computergraphics
{
	/**
	 * Triangle mesh data structure in indexed format.
	 * */
	public class TriangleMesh
	{
		/**
		 * List of vertices.
		 * */
		List<Vector3> vertices = new List<Vector3> ();

		/**
		 * List of indexed triangles.
		 * */
		List<Triangle> triangles = new List<Triangle> ();

		public TriangleMesh ()
		{
		}

		public void AddVertex (Vector3 vertex)
		{
			vertices.Add (vertex);
		}

		public void AddTriangle (int a, int b, int c)
		{
			Triangle t = new Triangle (a, b, c);
			triangles.Add (t);
		}

		public void AddTriangle (Triangle t)
		{
			triangles.Add (t);
		}

		public int GetNumberOfTriangles ()
		{
			return triangles.Count;
		}

		public Triangle GetTriangle (int index)
		{
			return triangles [index];
		}

		public Vector3 GetVertex (int index)
		{
			return vertices [index];
		}

		/**
		 * Compute normals for all triangles.
		 * */
		public void ComputeNormals ()
		{
			for (int i = 0; i < GetNumberOfTriangles (); i++) {
				Triangle t = triangles [i];
				Vector3 a = vertices [triangles [i].a];
				Vector3 b = vertices [triangles [i].b];
				Vector3 c = vertices [triangles [i].c];
				Vector3 normal = Vector3.Cross (Vector3.Subtract (b, a), Vector3.Subtract (c, a));
				normal.Normalize ();
				triangles [i].setNormal (normal);
			}
		}

		/**
		 * Create example mesh (cube).
		 * */
		public void CreateCube ()
		{
			Clear ();
			AddVertex (new Vector3 (-0.5f, -0.5f, -0.5f));
			AddVertex (new Vector3 (0.5f, -0.5f, -0.5f));
			AddVertex (new Vector3 (0.5f, 0.5f, -0.5f));
			AddVertex (new Vector3 (-0.5f, 0.5f, -0.5f));
			AddVertex (new Vector3 (-0.5f, -0.5f, 0.5f));
			AddVertex (new Vector3 (0.5f, -0.5f, 0.5f));
			AddVertex (new Vector3 (0.5f, 0.5f, 0.5f));
			AddVertex (new Vector3 (-0.5f, 0.5f, 0.5f));

			AddTriangle (new Triangle (0, 1, 2));
			AddTriangle (new Triangle (0, 2, 3));

			AddTriangle (new Triangle (1, 5, 2));
			AddTriangle (new Triangle (2, 5, 6));

			AddTriangle (new Triangle (4, 5, 6));
			AddTriangle (new Triangle (4, 6, 7));

			AddTriangle (new Triangle (4, 0, 7));
			AddTriangle (new Triangle (0, 3, 7));
			AddTriangle (new Triangle (3, 2, 7));
			AddTriangle (new Triangle (2, 6, 7));
			AddTriangle (new Triangle (4, 1, 0));
			AddTriangle (new Triangle (1, 4, 5));
			ComputeNormals ();
		}

		/**
		 * Clear all content.
		 * */
		public void Clear ()
		{
			vertices.Clear ();
			triangles.Clear ();
		}
	}
}

