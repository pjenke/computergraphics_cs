using OpenTK;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	public class BspNode : LeafNode
	{
		/**
		 * BSP tree root node
		 * */
		private BspTreeNode rootNode;

		/**
		 * List of points used to create the tree.
		 * */
		private List<Vector3> points;

		private Vector3 eye = new Vector3(1, 1, 0);

		private bool showPoints = true;
		private bool showPlanes = true;
		private bool showBackToFront = true;
		private bool showElements = true;

		private VertexBufferObject vboPoints = new VertexBufferObject();
		private VertexBufferObject vboPlanes = new VertexBufferObject();
		private VertexBufferObject vboBack2FrontPath = new VertexBufferObject();
		private VertexBufferObject vboElements = new VertexBufferObject();

		public bool ShowPoints
		{
			get { return showPoints; }
			set { showPoints = value; }
		}

		public bool ShowPlanes
		{
			get { return showPlanes; }
			set { showPlanes = value; }
		}

		public bool ShowBack2Front
		{
			get { return showBackToFront; }
			set { showBackToFront = value; }
		}

		public bool ShowElements
		{
			get { return showElements; }
			set { showElements = value; }
		}

		public BspNode(BspTreeNode rootNode, List<Vector3> points)
		{
			this.rootNode = rootNode;
			this.points = points;

			vboPoints.Setup(CreateVBOPoints(), PrimitiveType.Points);
			vboBack2FrontPath.Setup(CreateVBOBack2Front(), PrimitiveType.LineStrip);
			vboPlanes.Setup(CreateVBOPlanes(rootNode, 0.7f), PrimitiveType.Lines);
			vboElements.Setup(CreateVBOElements(rootNode), PrimitiveType.Lines);
		}

		public override void DrawGL(RenderMode mode, Matrix4 modelMatrix)
		{
			if (mode == RenderMode.REGULAR)
			{
				if (showPlanes)
				{
					vboPlanes.Draw();
				}

				if (showPoints)
				{
					GL.PointSize(5);
					vboPoints.Draw();
				}

				if (showBackToFront)
				{
					vboBack2FrontPath.Draw();
				}

				if (showElements)
				{
					vboElements.Draw();
				}
			}
		}

		public override void TimerTick(int counter)
		{
		}

		private List<RenderVertex> CreateVBOPoints()
		{
			List<RenderVertex> renderVertices = new List<RenderVertex>();
			foreach (Vector3 p in points)
			{
				renderVertices.Add(new RenderVertex(p, new Vector3(0, 1, 0), Color4.LightGreen));
			}
			renderVertices.Add(new RenderVertex(eye, new Vector3(0, 1, 0), Color4.Yellow));
			return renderVertices;
		}

		private List<RenderVertex> CreateVBOBack2Front()
		{
			List<RenderVertex> renderVertices = new List<RenderVertex>();
			List<int> sortedPoints = BspTreeTools.GetBackToFront(rootNode, points, eye);
			foreach (int index in sortedPoints)
			{
				renderVertices.Add(new RenderVertex(points[index], new Vector3(0, 1, 0), Color4.Yellow));
			}
			renderVertices.Add(new RenderVertex(eye, new Vector3(0, 1, 0), Color4.Yellow));
			return renderVertices;
		}

		private List<RenderVertex> CreateVBOPlanes(BspTreeNode node, float scale)
		{
			List<RenderVertex> renderVertices = new List<RenderVertex>();
			if (node == null)
			{
				return renderVertices;
			}
			Vector3 tangent = Vector3.Multiply(new Vector3(node.N.Y, -node.N.X, 0), scale);
			renderVertices.Add(new RenderVertex(Vector3.Add(node.P, tangent), new Vector3(0, 1, 0), Color4.White));
			renderVertices.Add(new RenderVertex(Vector3.Subtract(node.P, tangent), new Vector3(0, 1, 0), Color4.White));
			renderVertices.Add(new RenderVertex(node.P, new Vector3(0, 1, 0), Color4.White));
			renderVertices.Add(new RenderVertex(Vector3.Add(node.P, Vector3.Multiply(node.N, scale * 0.3f)), new Vector3(0, 1, 0), Color4.White));

			renderVertices.AddRange(CreateVBOPlanes(node.GetChild(BspTreeNode.Orientation.POSITIVE), scale * 0.5f));
			renderVertices.AddRange(CreateVBOPlanes(node.GetChild(BspTreeNode.Orientation.NEGATIVE), scale * 0.5f));

			return renderVertices;
		}

		private List<RenderVertex> CreateVBOElements(BspTreeNode node)
		{
			List<RenderVertex> renderVertices = new List<RenderVertex>();
			if (node == null)
			{
				return renderVertices;
			}
			for (int orientation = 0; orientation < 2; orientation++)
			{
				Color4 color = (orientation == 0) ? Color4.Magenta : Color4.Orange;
				for (int i = 0; i < node.getNumberOfElements((BspTreeNode.Orientation)orientation); i++)
				{
					int index = node.getElement((BspTreeNode.Orientation)orientation, i);
					renderVertices.Add(new RenderVertex(node.P, new Vector3(0,1,0), color));
					renderVertices.Add(new RenderVertex(points[index], new Vector3(0, 1, 0), color));
				}
			}
			renderVertices.AddRange(CreateVBOElements(node.GetChild(BspTreeNode.Orientation.POSITIVE)));
			renderVertices.AddRange(CreateVBOElements(node.GetChild(BspTreeNode.Orientation.NEGATIVE)));
			return renderVertices;
		}
	}
}

