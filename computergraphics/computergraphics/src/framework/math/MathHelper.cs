using System;
using OpenTK;

namespace computergraphics
{
	/**
	 * Helper class with math functionality.
	 * */
	public class MathHelper
	{
		/**
		 * Helper method: multiply x = A * b (vector = matrix * vector)
		 * */
		public static Vector3 Mult (Matrix3 M, Vector3 v)
		{
			return new Vector3 (Vector3.Dot (M.Row0, v), Vector3.Dot (M.Row1, v), Vector3.Dot (M.Row2, v));
		}
	}
}

