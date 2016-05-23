using System;
using OpenTK;

namespace computergraphics
{
	/**
	 * Represents a triangles in an indexed structure.
	 * */
	public class Triangle
	{
		/**
		 * Vertex indices
		 * */
		int [] vertexIndices = {-1, -1, -1};

		/**
		 * Texture coordinate indices.
		 * */
		int [] texCoordIndices = {-1, -1, -1};


		/**
		 * Triangle normal, originally uninitialized.
		 * */
		public Vector3 normal;

		public Triangle (int vertexIndexA, int vertexIndexB, int vertexIndexC)
		{
			vertexIndices[0] = vertexIndexA;
			vertexIndices[1] = vertexIndexB;
			vertexIndices[2] = vertexIndexC;
		}

		public Triangle (int vertexIndexA, int vertexIndexB, int vertexIndexC, int texCoordIndexA, int texCoordIndexB, int texCoordIndexC)
		{
			vertexIndices[0] = vertexIndexA;
			vertexIndices[1] = vertexIndexB;
			vertexIndices[2] = vertexIndexC;
			texCoordIndices [0] = texCoordIndexA;
			texCoordIndices [1] = texCoordIndexB;
			texCoordIndices [2] = texCoordIndexC;
		}

		public int GetVertexIndex (int index)
		{
			return vertexIndices [index];
		}

		public int GetTexCoordIndex (int index)
		{
			return texCoordIndices [index];
		}

		public void setNormal (Vector3 normal)
		{
			this.normal = normal;
		}
	}
}

