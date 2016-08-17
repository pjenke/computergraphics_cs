using OpenTK;

namespace computergraphics
{
	/**
	 * A vertex consists of a position and a normal
	 * */
	public class Vertex
	{
		/**
		 * Position in 3-space
		 * */
		private Vector3 position;

		/**
		 * Vertex normal
		 * */
		private Vector3 normal;

		/**
		 * Position property
		 * */
		public Vector3 Position
		{
			set { position = value; }
			get { return position; }
		}

		/**
		 * Normal property
		 * */
		public Vector3 Normal
		{
			set { normal = value; }
			get { return normal; }
		}

		public Vertex(Vector3 position) :
			this(position, Vector3.UnitY)
		{
		}

		public Vertex(Vector3 position, Vector3 normal)
		{
			this.position = position;
			this.normal = normal;
		}
	}
}

