using OpenTK;

namespace computergraphics
{
	/**
	 * Scales all child nodes in each axis according to the scale vector.
	 * */
	public class ScaleNode : InnerNode
	{
		/**
		 * Scaling matrix (model matrix).
		 * */
		private Matrix4 scale;

		public ScaleNode (Vector3 scale)
		{
			this.scale = Matrix4.CreateScale(scale);
		}

		public override void Traverse (RenderMode mode, Matrix4 modelMatrix)
		{
			base.Traverse(mode, Matrix4.Mult(scale, modelMatrix));
		}

		public override void TimerTick (int counter)
		{
			base.TimerTick (counter);
		}
	}
}

