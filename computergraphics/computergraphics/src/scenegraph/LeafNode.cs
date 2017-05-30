using OpenTK;
using System.Collections.Generic;

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

        public List<Vector3> Triangles { get;  } = new List<Vector3>();

        public abstract void UpdateTriangles(Vector3 p1, Vector3 p2, Vector3 p3);

        /**
		 * Draw GL content
		 * */
        public abstract void DrawGL(RenderMode mode, Matrix4 modelMatrix);
	}
}

