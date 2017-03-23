using OpenTK;
using System.Collections.Generic;

namespace computergraphics
{
	/**
	 * A leaf node allows to draw OpenGl content.
	 * */
	public abstract class LeafNode : INode
	{

        public List<Vector3> Triangles
        {
            get
            {
                return Triangles;
            }
        }

        public List<Vector3> Normals
        {
            get
            {
                return Normals;
            }
        }

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
         * Update triangles and normals.
         * */
         public void UpdateTriangles(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 n)
        {
            Triangles.Add(p1);
            Triangles.Add(p2);
            Triangles.Add(p3);
            Normals.Add(n);
        }

		/**
		 * Draw GL content
		 * */
		public abstract void DrawGL(RenderMode mode, Matrix4 modelMatrix);
	}
}

