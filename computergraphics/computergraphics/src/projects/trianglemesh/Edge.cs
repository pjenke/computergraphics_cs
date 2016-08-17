namespace computergraphics
{
	/**
	 * Represents an edge in a triangle mesh.
	 * */
	public class Edge
	{
		/**
		 * Index of teh first vertex.
		 * */
		public int a;

		/**
		 * Index of the second vertex.
		 * */
		public int b;

		public Edge (int a, int b)
		{
			this.a = a;
			this.b = b;
		}

		public override bool Equals (object other)
		{
			Edge otherEdge = other as Edge;
			if (otherEdge == null) {
				return false;
			}
			return (a == otherEdge.a && b == otherEdge.b) ||
			(a == otherEdge.b && b == otherEdge.a);
		}

		public override int GetHashCode ()
		{
			return a + b;
		}

		public override string ToString ()
		{
			return "(" + a + ", " + b + ")";
		}

		/**
		 * Flip end vertices.
		 * */
		public void Flip()
		{
			int tmp = b;
			b = a;
			a = tmp;
		}
	}
}

