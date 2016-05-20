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
		 * Index of first vertex.
		 * */
		public int a;

		/**
		 * Index of second vertex.
		 * */
		public int b;

		/**
		 * Index of third vertex.
		 * */
		public int c;

		/**
		 * Triangle normal, originally uninitialized.
		 * */
		public Vector3 normal;

		public Triangle (int a, int b, int c)
		{
			this.a = a;
			this.b = b;
			this.c = c;
		}

		public int Get (int index)
		{
			switch (index) {
			case 0:
				return a;
			case 1:
				return b;
			default:
				return c;
			}
		}

		public void setNormal (Vector3 normal)
		{
			this.normal = normal;
		}
	}
}

