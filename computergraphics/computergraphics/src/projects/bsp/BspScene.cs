using System;
using OpenTK;
using OpenTK.Input;
using System.Collections.Generic;

namespace computergraphics
{
	/**
	 * Testing the BSP tree implementation.
	 * */
	public class BspScene : Scene
	{

		private BspNode node;

		public BspScene() : base(100, Shader.ShaderMode.NO_LIGHTING, RenderMode.REGULAR)
		{
			Random random = new Random();
			int numberOfPoints = 10;
			List<Vector3> points = new List<Vector3>();
			List<int> pointIndices = new List<int>();
			for (int i = 0; i < numberOfPoints; i++)
			{
				points.Add(new Vector3((float)(2 * random.NextDouble() - 1), (float)(2 * random.NextDouble() - 1), 0));
				pointIndices.Add(i);
			}
			BspTreeNode rootNode = BspTreeTools.CreateBspTree(null, points, pointIndices);
			node = new BspNode(rootNode, points);
			GetRoot().AddChild(node);

			GetRoot().LightPosition = new Vector3(0, 0, 1);

		}

		public override void KeyPressed(Key key)
		{
			switch (key)
			{
				case Key.P:
					node.ShowPoints = !node.ShowPoints;
					break;
				case Key.E:
					node.ShowElements = !node.ShowElements;
					break;
				case Key.L:
					node.ShowPlanes = !node.ShowPlanes;
					break;
				case Key.B:
					node.ShowBack2Front = !node.ShowBack2Front;
					break;
			}
		}
	}
}

