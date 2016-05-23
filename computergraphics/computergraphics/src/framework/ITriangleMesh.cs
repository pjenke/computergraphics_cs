using System;
using OpenTK;

namespace computergraphics
{
	/**
	 * Shared interfaces for triangle meshes.
	 * */
	public interface ITriangleMesh
	{

		/**
		 * Add vertex.
		 * */
		void AddVertex (Vector3 vertex);

		/**
		 * Add triangle by indices.
		 * */
		void AddTriangle (int a, int b, int c);

		/**
		 * Add triangle.
		 * */
		void AddTriangle (Triangle t);

		/**
		 * Add texture coordinate.
		 * */
		void AddTextureCoordinate(Vector2 texCoord);

		int GetNumberOfTriangles ();

		Triangle GetTriangle (int triangleIndex);

		int GetNumberOfVertices ();

		Vector3 GetVertex (int vertexIndex);

		Vector2 GetTextureCoordinate(int index);

		/**
		 * Compute normals for all triangles.
		 * */
		void ComputeTriangleNormals ();

		/**
		 * Clear datastructure.
		 * */
		void Clear ();
	}
}

