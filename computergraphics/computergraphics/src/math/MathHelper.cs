using OpenTK;

namespace computergraphics
{
	/**
	 * Helper class with math functionality.
	 * */
	public class MathHelper
	{
		/**
		 * Helper method: multiply x = A * b (vector = matrix * vector, 3D)
		 * */
		public static Vector3 Multiply (Matrix3 M, Vector3 v)
		{
			return new Vector3 (Vector3.Dot (M.Row0, v), Vector3.Dot (M.Row1, v), Vector3.Dot (M.Row2, v));
		}

		/**
		 * Helper method: multiply x = A * b (vector = matrix * vector, 4D)
		 * */
		public static Vector4 Multiply(Matrix4 M, Vector4 v)
		{
			return new Vector4(Vector4.Dot(M.Row0, v), Vector4.Dot(M.Row1, v), Vector4.Dot(M.Row2, v), Vector4.Dot(M.Row3, v));
		}
	}
}

