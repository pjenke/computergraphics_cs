using System;
using OpenTK;

namespace computergraphics
{
	/**
	 * Represents a virtual camera in 3-space.
	 * */
	public class Camera
	{
		/**
		 * Eye point
		 * */
		Vector3 eye = new Vector3 (0, 0, -5);

		public Vector3 Eye {
			get { return eye; }
		}

		/**
		 * Camera refernece point
		 * */
		Vector3 target = new Vector3 (0, 0, 0);

		/**
		 * Up-vector
		 * */
		Vector3 up = Vector3.UnitY;

		/**
		 * Current modelview matrix
		 * */
		Matrix4 LookAt;

		/**
		 * Field of view in y-direction
		 * */
		float fovy = 45.0f * (float)Math.PI / 180.0f;

		/**
		 * Near clipping plane
		 * */
		float zNear = 0.05f;

		/**
		 * Far clipping plane
		 * */
		float zFar = 10.0f;

		/**
		 * Aspect ratio (should match window dimensions
		 * */
		float aspectRatio = 1.0f;

		/**
		 * Aspect raio property
		 * */
		public float AspectRatio {
			set { aspectRatio = value; }
			get { return aspectRatio; }
		}

		public Camera ()
		{
			LookAt = Matrix4.LookAt (eye, target, up);
		}

		public Matrix4 getLookAtMatrix ()
		{
			return LookAt;
		}

		/**
		 * Update the camera eye point by rotation angles.
		 * */
		public void updateLookAtMatrix (float alpha, float beta)
		{
			Vector3 dir = Vector3.Subtract (eye, target);
			// Rotate around up-vector
			eye = Vector3.Add (Mult (Matrix3.CreateFromAxisAngle (up, alpha), dir), target);
			// Rotate around side-vector
			dir = Vector3.Subtract (eye, target);
			Vector3 side = Vector3.Cross (dir, up);
			side.Normalize ();
			eye = Vector3.Add (Mult (Matrix3.CreateFromAxisAngle (side, -beta), dir), target);
			// Fix up-vector
			dir = Vector3.Subtract (target, eye);
			side = Vector3.Cross (dir, up);
			side.Normalize ();
			up = Vector3.Cross (side, dir);
			up.Normalize ();
			// Update LookAt
			LookAt = Matrix4.LookAt (eye, target, up);
		}

		public void Zoom (int factor)
		{
			Vector3 dir = Vector3.Subtract (target, eye);
			eye = Vector3.Add (eye, Vector3.Multiply (dir, 0.002f * factor));

			// Update LookAt
			LookAt = Matrix4.LookAt (eye, target, up);
		}

		/**
		 * Helper method: multiply x = A * b (vector = matrix * vector)
		 * */
		public static Vector3 Mult (Matrix3 M, Vector3 v)
		{
			return new Vector3 (Vector3.Dot (M.Row0, v), Vector3.Dot (M.Row1, v), Vector3.Dot (M.Row2, v));
		}

		public float getFovy ()
		{
			return fovy;
		}

		public float getZNear ()
		{
			return zNear;
		}

		public float getZFar ()
		{
			return zFar;
		}
	}
}

