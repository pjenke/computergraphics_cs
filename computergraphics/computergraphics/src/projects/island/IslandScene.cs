using System;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace computergraphics
{
	public class IslandScene: Scene
	{
		public IslandScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
		{
			GetRoot().LightPosition = new Vector3(0, 0, 1);
			GetRoot().Animated = true;

			List<Hexagon> hexagons = GeneratePath();
			hexagons = CreateFilledAreaFromPath(hexagons,100);
			ITriangleMesh mesh = new TriangleMesh();
			foreach (Hexagon hex in hexagons)
			{
				if (!hex.ConsistencyCheck())
				{
					Console.WriteLine("Inconsistency detected!");
				}
				TriangleMeshFactory.Unite(mesh, hex.GenerateMesh());
			}
			CreateBoundary(mesh, hexagons);

			TriangleMeshNode node = new TriangleMeshNode(mesh);
			node.SetColor(Color4.DarkGreen);
			node.ShowNormals = false;
			GetRoot().AddChild(node);
		}

		private void CreateBoundary(ITriangleMesh mesh, List<Hexagon> hexagons)
		{
			float offset = hexagons[0].SideLength;
			foreach (Hexagon hex in hexagons)
			{
				for (int i = 0; i < 6; i++)
				{
					if (hex.GetNeighbor(i) == null)
					{
						Vector3 a = hex.GetCorner((i + 5) % 6);
						Vector3 b = hex.GetCorner(i);
						Vector3 c = Vector3.Add(b, new Vector3(0, 0, -offset));
						Vector3 d = Vector3.Add(a, new Vector3(0, 0, -offset));
						int aIndex = mesh.AddVertex(a);
						int bIndex = mesh.AddVertex(b);
						int cIndex = mesh.AddVertex(c);
						int dIndex = mesh.AddVertex(d);
						mesh.AddTriangle(aIndex, cIndex, dIndex);
						mesh.AddTriangle(aIndex, bIndex, cIndex);
					}
				}
			}
		}

		/**
 		 * Fill the border around a generated path from inside to outside
 		 * */
		private List<Hexagon> CreateFilledAreaFromPath(List<Hexagon> path, int finalNumberOfHexagons)
		{
			Queue<Hexagon> queue = new Queue<Hexagon>();
			foreach (Hexagon hex in path)
			{
				queue.Enqueue(hex);
			}
			while (path.Count < finalNumberOfHexagons)
			{
				Hexagon hex = queue.Dequeue();
				while (hex.HasBorder())
				{
					int nIndex = hex.GetRandomBorderIndex();
					Hexagon newHex = hex.CreateNeighbor(nIndex);
					path.Add(newHex);
					queue.Enqueue(newHex);
				}
			}

			return path;
		}

		/**
		 * Generate a list of hexagons along a path (currently random)
		 * */
		private List<Hexagon> GeneratePath()
		{
			List<Hexagon> hexagons = new List<Hexagon>();
			Hexagon start = new Hexagon(new Vector3(0, 0, 0), 0.1f);
			hexagons.Add(start);
			Hexagon current = start;
			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				Hexagon next = current.CreateNeighbor(random.Next(3));
				hexagons.Add(next);
				current = next;
			}
			return hexagons;
		}
	}
}
