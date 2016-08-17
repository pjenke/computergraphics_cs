using OpenTK;

namespace computergraphics
{
	/**
	 * Implements a rotation node
	 * */
	public class RotationNode : InnerNode
	{
		Matrix4 rotation = Matrix4.Identity;

		private bool animated = false;
		private Vector3 axis = Vector3.UnitY;

		public bool Animated {
			get { return animated; }
			set{ animated = value; }
		}

		private float animationSpeed = 1;

		public float AnimationSpeed {
			get { return animationSpeed; }
			set{ animationSpeed = value; }
		}

		public RotationNode () : this (0, new Vector3 (0, 1, 0))
		{
		}

		public RotationNode (float angle, Vector3 axis)
		{
			rotation = Matrix4.CreateFromAxisAngle(axis, angle);
			this.axis = axis;
		}

		public override void Traverse (RenderMode mode, Matrix4 modelMatrix)
		{
			base.Traverse(mode, Matrix4.Mult(rotation, modelMatrix));
		}

		public override void TimerTick (int counter)
		{
			base.TimerTick (counter);

			if (animated) {
				rotation = Matrix4.CreateFromAxisAngle(axis, counter * 10.0f * animationSpeed);
			}
		}
	}
}

