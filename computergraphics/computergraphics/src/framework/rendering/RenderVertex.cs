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

		public Vector3 Position
		{
			get { return position;}
		}

		public Vector3 Normal
		{
			get { return normal; }
		}

		public Color4 Color
		{
			get { return color; }
		}

		public RenderVertex(Vector3 position, Vector3 normal, Color4 color)
		{
			this.position = position;
			this.normal = normal;
			this.color = color;
		}
	}
}

