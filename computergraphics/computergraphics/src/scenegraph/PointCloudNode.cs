using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	public class PointCloudNode : LeafNode
	{
		/**
		 * Dynamic VBO 
		 **/
		private VertexBufferObject vbo = new VertexBufferObject(true);
		private PointCloud pc; 
		private List<RenderVertex> renderVertices = new List<RenderVertex>();

		public PointCloudNode(PointCloud pc)
		{
            this.pc = pc;
			renderVertices = new List<RenderVertex>();
			CreateVBO();
		}

		private void CreateVBO()
		{
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

        public override void UpdateTriangles(Vector3 p1, Vector3 p2, Vector3 p3)
        {
        }

        public void updateVBO()
		{
			if (renderVertices.Count != pc.Count)
			{
				Console.WriteLine("Dynamic change of point cloud size relative to VBO size not support, call Setup() again");
				return;
			}

			for (int i = 0; i < pc.Count; i++)
			{
				renderVertices[i].Position = pc.Get(i);
			}
			vbo.Invalidate();
		}
	}
}
