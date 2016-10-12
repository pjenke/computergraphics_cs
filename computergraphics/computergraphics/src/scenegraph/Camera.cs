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
		private Vector3 eye;

		/**
		 * Eye property
		 * */
		public Vector3 Eye {
			get { return eye; }
		}

		/**
		 * Camera refernece point
		 * */
		private Vector3 refPoint;

		/**
		 * Up-vector
		 * */
		private Vector3 up;

		/**
		 * Current modelview matrix
		 * */
		Matrix4 viewMatrix;

		/**
		 * Field of view in y-direction (angle in radiens)
		 * */
		private float fovy;

		/**
		 * Near clipping plane
		 * */
		private float zNear;

		/**
		 * Far clipping plane
		 * */
		private float zFar;

		/**
		 * Aspect ratio (should match window dimensions
		 * */
		private float aspectRatio;

		/**
		 * Aspect raio property
		 * */
		public float AspectRatio {
			set { aspectRatio = value; }
			get { return aspectRatio; }
		}

		public Camera ()
		{
			eye = new Vector3(0, 0, 5);
			refPoint = new Vector3(0, 0, 0);
			up = Vector3.UnitY;
			fovy = 45.0f * (float)Math.PI / 180.0f;
			zNear = 0.05f;
			zFar = 10.0f;
			aspectRatio = 1.0f;
			viewMatrix = Matrix4.LookAt (eye, refPoint, up);
		}

		/**
		 * Update the camera eye point by rotation angles.
		 * */
		public void UpdateLookAtMatrix (float alpha, float beta)
		{
			Vector3 dir = Vector3.Subtract (eye, refPoint);
			// Rotate around up-vector
			eye = Vector3.Add (MathHelper.Multiply (Matrix3.CreateFromAxisAngle (up, alpha), dir), refPoint);
			// Rotate around side-vector
			dir = Vector3.Subtract (eye, refPoint);
			Vector3 side = Vector3.Cross (dir, up);
			side.Normalize ();
			eye = Vector3.Add (MathHelper.Multiply (Matrix3.CreateFromAxisAngle (side, -beta), dir), refPoint);
			// Fix up-vector
			dir = Vector3.Subtract (refPoint, eye);
			side = Vector3.Cross (dir, up);
			side.Normalize ();
			up = Vector3.Cross (side, dir);
			up.Normalize ();
			// Update LookAt
			viewMatrix = Matrix4.LookAt (eye, refPoint, up);
		}

		/**
		 * Zoom in/out.
		 * */
		public void Zoom (int factor)
		{
			Vector3 dir = Vector3.Subtract (refPoint, eye);
			eye = Vector3.Add (eye, Vector3.Multiply (dir, 0.002f * factor));

			// Update LookAt
			viewMatrix = Matrix4.LookAt (eye, refPoint, up);
		}

		public Matrix4 GetViewMatrix()
		{
			return viewMatrix;
		}

		public float GetFovy ()
		{
			return fovy;
		}

		public float GetZNear ()
		{
			return zNear;
		}

		public float GetZFar ()
		{
			return zFar;
		}
	}
}

