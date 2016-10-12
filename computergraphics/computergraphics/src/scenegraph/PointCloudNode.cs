using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	public class PointCloudNode : LeafNode
	{
		private VertexBufferObject vbo = new VertexBufferObject();
		private PointCloud pc; 
		public PointCloudNode(PointCloud pc)
		{
			this.pc = pc;
			CreateVBO();
		}

		private void CreateVBO()
		{
			List<RenderVertex> renderVertices = new List<RenderVertex>();
			for (int i = 0; i < pc.Count; i++)
			{
				renderVertices.Add(new RenderVertex(pc.Get(i), Vector3.UnitY, Color4.MediumPurple));
			}
			vbo.Setup(renderVertices, OpenTK.Graphics.OpenGL.PrimitiveType.Points);
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
		public override void DrawGL(RenderMode mode, Matrix4 modelMatrix)
		{
			GL.PointSize(3);
			vbo.Draw();
		}

		/**
		 * Timer event.
		 * */
		public override void TimerTick(int counter)
		{
		}
	}
}
