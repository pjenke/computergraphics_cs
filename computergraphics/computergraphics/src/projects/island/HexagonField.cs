using System.Collections.Generic;
using OpenTK;
using System;

namespace computergraphics
{
	public class HexagonField
	{
		private List<Hexagon> hexagons = new List<Hexagon>();

		public int Count
		{
			get { return hexagons.Count; }
		}

		public Hexagon Get(int index)
		{
			return hexagons[index];
		}

		public float SideLength
		{
			get { return hexagons[0].SideLength; }
		}

		public HexagonField()
		{
		}

		public void GenerateField()
		{
			hexagons = GeneratePath(5);
			hexagons = CreateFilledAreaFromPath(hexagons, 30);
		}

		/**
		 * Generate a list of hexagons along a path (currently random)
		 * */
		private List<Hexagon> GeneratePath(int numSteps)
		{
			List<Hexagon> hexagons = new List<Hexagon>();
			Hexagon start = new Hexagon(new Vector3(0, 0, 0), 0.1f);
			hexagons.Add(start);
			Hexagon current = start;
			Random random = new Random();
			for (int i = 0; i < numSteps; i++)
			{
				Hexagon next = current.CreateNeighbor(random.Next(3));
				hexagons.Add(next);
				current = next;
			}
			return hexagons;
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
				while (hex.IsBorder())
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
		 * Returns the list of polygons at at the border.
		 **/
		public List<Hexagon> GetBorderHexagons()
		{
			List<Hexagon> border = new List<Hexagon>();
			foreach (Hexagon hex in hexagons)
			{
				if (hex.IsBorder())
				{
					border.Add(hex);
				}
			}
			return border;
		}

		/**
		 * Returns a list of polygons not at the border
		 * */
		public List<Hexagon> GetNonBorderHexagons()
		{
			List<Hexagon> nonBorder = new List<Hexagon>();
			foreach (Hexagon hex in hexagons)
			{
				if (!hex.IsBorder())
				{
					nonBorder.Add(hex);
				}
			}
			return nonBorder;
		}

		/**
		 * Return a list of all hexagons
		*/
		public List<Hexagon> GetAllHexagons()
		{
			List<Hexagon> result = new List<Hexagon>();
			foreach (Hexagon hex in hexagons)
			{
				result.Add(hex);
			}
			return result;
		}
	}
}
