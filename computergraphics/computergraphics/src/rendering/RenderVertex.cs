using OpenTK;
using OpenTK.Graphics;

/**
 * Helping data structure to represent a render vertex in a @VertexBufferObject.
 * 
 *
 */
namespace computergraphics
{
	public class RenderVertex
	{
		/**
		 * 3D position.
		 */
		private Vector3 position;

		/**
		 * 3D normal.
		 */
		private Vector3 normal;

		/**
		 * 4D color.
		 */
		private Color4 color;

		/**
 		 * Texture coordinates
 		 * */
		private Vector2 texCoords;

		public Vector3 Position
		{
			get { return position; }
			set { position = value; }
		}

		public Vector3 Normal
		{
			get { return normal; }
		}

		public Color4 Color
		{
			get { return color; }
		}

		public Vector2 TexCoords
		{
			get { return texCoords; }
		}

		public RenderVertex(Vector3 position, Vector3 normal, Color4 color) : this(position, normal, color, new Vector2(0, 0))
		{
		}

		public RenderVertex(Vector3 position, Vector3 normal, Color4 color, Vector2 texCoords)
		{
			this.position = position;
			this.normal = normal;
			this.color = color;
			this.texCoords = texCoords;
		}
	}
}

