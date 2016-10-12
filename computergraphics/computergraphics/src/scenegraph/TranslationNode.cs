using OpenTK;

namespace computergraphics
{
	/**
	 * Translates all child node along a given vector.
	 * */
	public class TranslationNode : InnerNode
	{

		/**
		 * Translation matrix (model matrix)
		 * */
		private Matrix4 translation;

		public Vector3 Translation
		{
			set { translation = Matrix4.CreateTranslation(value); }
		}

		public TranslationNode(Vector3 translation)
		{
			this.translation = Matrix4.CreateTranslation(translation);
		}

		public override void Traverse(RenderMode mode, Matrix4 modelMatrix)
		{
			base.Traverse(mode, Matrix4.Mult(translation, modelMatrix));
		}

		public override void TimerTick(int counter)
		{
			base.TimerTick(counter);
		}
	}
}

