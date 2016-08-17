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
		int[] vertexIndices = { -1, -1, -1 };

		/**
		 * Texture coordinate indices.
		 * */
		int[] texCoordIndices = { -1, -1, -1 };

		/**
		 * Triangle normal, originally uninitialized.
		 * */
		public Vector3 normal;

		/**
		 * Normal property
		 * */
		public Vector3 Normal {
			set { normal = value; }
			get { return normal; }
		}

		public Triangle (int vertexIndexA, int vertexIndexB, int vertexIndexC) :
			this (vertexIndexA, vertexIndexB, vertexIndexC, 
			      -1, -1, -1,
			      Vector3.UnitY)
		{
		}

		public Triangle (int vertexIndexA, int vertexIndexB, int vertexIndexC, 
		                 int texCoordIndexA, int texCoordIndexB, int texCoordIndexC) :
			this (vertexIndexA, vertexIndexB, vertexIndexC, 
			      texCoordIndexA, texCoordIndexB, texCoordIndexC, 
			      Vector3.UnitY)
		{
		}

		public Triangle (int vertexIndexA, int vertexIndexB, int vertexIndexC, 
		                 int texCoordIndexA, int texCoordIndexB, int texCoordIndexC, 
		                 Vector3 normal)
		{
			vertexIndices [0] = vertexIndexA;
			vertexIndices [1] = vertexIndexB;
			vertexIndices [2] = vertexIndexC;
			texCoordIndices [0] = texCoordIndexA;
			texCoordIndices [1] = texCoordIndexB;
			texCoordIndices [2] = texCoordIndexC;
			this.normal = normal;
		}

		public int GetVertexIndex (int index)
		{
			return vertexIndices [index];
		}

		public int GetTexCoordIndex (int index)
		{
			return texCoordIndices [index];
		}
	}
}

