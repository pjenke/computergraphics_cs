using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace computergraphics
{
	/** 
	 * Draws a cube.
	 * */
	public class CubeNode : LeafNode
	{

		/**
		 * Cube is centered at the origin and its sidelength is twice this value.
		 * */
		private float sideLength;

        private List<RenderVertex> renderVertices;

		private VertexBufferObject vbo = new VertexBufferObject();

        public CubeNode(float sideLength)
		{
            this.sideLength = sideLength;
			CreateVBO();
		}

		public override void DrawGL(RenderMode mode, Matrix4 modelMatrix)
		{
			if (mode == RenderMode.REGULAR)
			{
				vbo.Draw();
			}
		}

		public override void TimerTick(int counter)
		{
		}

		void CreateVBO()
		{
            renderVertices = new List<RenderVertex>();

			Vector3 p0 = new Vector3(-sideLength, -sideLength, -sideLength);
			Vector3 p1 = new Vector3(sideLength, -sideLength, -sideLength);
			Vector3 p2 = new Vector3(sideLength, sideLength, -sideLength);
			Vector3 p3 = new Vector3(-sideLength, sideLength, -sideLength);
			Vector3 p4 = new Vector3(-sideLength, -sideLength, sideLength);
			Vector3 p5 = new Vector3(sideLength, -sideLength, sideLength);
			Vector3 p6 = new Vector3(sideLength, sideLength, sideLength);
			Vector3 p7 = new Vector3(-sideLength, sideLength, sideLength);
			Vector3 n0 = new Vector3(0, 0, -1);
			Vector3 n1 = new Vector3(1, 0, 0);
			Vector3 n2 = new Vector3(0, 0, 1);
			Vector3 n3 = new Vector3(-1, 0, 0);
			Vector3 n4 = new Vector3(0, 1, 0);
			Vector3 n5 = new Vector3(0, -1, 0);


			Color4 color = Color4.Aquamarine;
			AddSideVertices(renderVertices, p0, p1, p2, p3, n0, color);
			AddSideVertices(renderVertices, p1, p5, p6, p2, n1, color);
			AddSideVertices(renderVertices, p4, p7, p6, p5, n2, color);
			AddSideVertices(renderVertices, p0, p3, p7, p4, n3, color);
			AddSideVertices(renderVertices, p2, p6, p7, p3, n4, color);
			AddSideVertices(renderVertices, p5, p1, p0, p4, n5, color);

			vbo.Setup(renderVertices, PrimitiveType.Quads);
		}

        public override void UpdateTriangles(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Triangles.Add(p1);
            Triangles.Add(p2);
            Triangles.Add(p3);
        }

        /**
		 * Add 4 vertices to the render list.
		 * */
        private void AddSideVertices(List<RenderVertex> renderVertices, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 normal, Color4 color)
		{
			renderVertices.Add(new RenderVertex(p3, normal, color));
			renderVertices.Add(new RenderVertex(p2, normal, color));
			renderVertices.Add(new RenderVertex(p1, normal, color));
			renderVertices.Add(new RenderVertex(p0, normal, color));
            UpdateTriangles(p0, p1, p2);
            UpdateTriangles(p0, p2, p3);
        }
	}
}

