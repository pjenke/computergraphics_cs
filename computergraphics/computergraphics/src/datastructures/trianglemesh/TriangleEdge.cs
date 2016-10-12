using System;
namespace computergraphics
{
	public class TriangleEdge
	{
		private int a, b;

		public int A {
			get { return a;}
		}

		public int B {
			get { return b;}
		}

		public TriangleEdge(int a, int b)
		{
			this.a = a;
			this.b = b;
		}

		public override bool Equals(object obj)
		{
			TriangleEdge other = obj as TriangleEdge;
			if (other != null)
			{
				return (a == other.a && b == other.b) || (a == other.b && b == other.a);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return a+b;
		}

		/**
		 * Flip end points.
		 * */
		public void Flip()
		{
			int x = a;
			a = b;
			b = x;
		}
	}
}
