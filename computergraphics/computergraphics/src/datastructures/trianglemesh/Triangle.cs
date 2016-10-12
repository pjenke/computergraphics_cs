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

		public int A
		{
			set { vertexIndices[0] = value; }
			get { return vertexIndices[0]; }
		}

		public int B
		{
			set { vertexIndices[1] = value; }
			get { return vertexIndices[1]; }
		}

		public int C
		{
			set { vertexIndices[2] = value; }
			get { return vertexIndices[2]; }
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

		public Triangle Clone()
		{
			Triangle t = new Triangle(vertexIndices[0], vertexIndices[1], vertexIndices[2],
									  texCoordIndices[0], texCoordIndices[1], texCoordIndices[2],
			                          normal);
			return t;
		}

		/**
		 * Flip normal orientation
		 * */
		public void Flip()
		{
			int h = vertexIndices[0];
			vertexIndices[0] = vertexIndices[1];
			vertexIndices[1] = h;
			h = texCoordIndices[0];
			texCoordIndices[0] = texCoordIndices[1];
			texCoordIndices[1] = h;
			normal = Vector3.Multiply(normal, -1);
		}

		public void VertexIndexOffset(int offset)
		{
			for (int i = 0; i < 3; i++)
			{
				vertexIndices[i] += offset;
			}
		}

		public void TexCoordsIndexOffset(int offset)
		{
			for (int i = 0; i < 3; i++)
			{
				texCoordIndices[i] += offset;
			}
		}
	}
}

