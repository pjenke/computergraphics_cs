using OpenTK;

namespace computergraphics
{
	/**
	 * A leaf node allows to draw OpenGl content.
	 * */
	public abstract class LeafNode : INode
	{
		public LeafNode()
		{
		}

		public override void Traverse(RenderMode mode, Matrix4 modelMatrix)
		{
			ShaderAttributes.GetInstance().SetModelMatrixParameter(modelMatrix);
			ShaderAttributes.GetInstance().SetViewMatrixParameter(GetRootNode().Camera.GetViewMatrix());
			DrawGL(mode, modelMatrix);
		}

		/**
		 * Draw GL content
		 * */
		public abstract void DrawGL(RenderMode mode, Matrix4 modelMatrix);
	}
}

