using OpenTK;
using System.Collections.Generic;

namespace computergraphics
{
	/**
	 * Tools to work with BSP trees
	 * */
	public class BspTreeTools
	{
		/**
		 * Recursively create a BSP tree for a given set of points
		 * */
		public static BspTreeNode CreateBspTree(BspTreeNode parentNode, List<Vector3> allPoints, List<int> pointIndices)
		{
			if (pointIndices.Count == 1)
			{
				InsertNode(parentNode, allPoints, pointIndices[0]);
			}
			else if (pointIndices.Count == 2)
			{
				// Two nodes - create separating plane
				BspTreeNode node = new BspTreeNode();
				Vector3 p0 = allPoints[pointIndices[0]];
				Vector3 p1 = allPoints[pointIndices[1]];

				node.P = Vector3.Multiply(Vector3.Add(p0, p1), 0.5f);
				node.N = Vector3.Subtract(p0, p1).Normalized();

				InsertNode(node, allPoints, pointIndices[0]);
				InsertNode(node, allPoints, pointIndices[1]);

				return node;
			}
			else 
			{
				// PCA-based separation
				BspTreeNode node = new BspTreeNode();
				PrincipalComponentAnalysis pca = new PrincipalComponentAnalysis();
				foreach (int index in pointIndices)
				{
					pca.Add(allPoints[index]);
				}
				pca.applyPCA();
				node.P = pca.getCentroid();
				node.N = pca.getEigenVector(2);

				// Handle children
				List<int> pos = new List<int>();
				List<int> neg = new List<int>();
				foreach (int index in pointIndices)
				{
					if (node.IsPositive(allPoints[index]))
					{
						pos.Add(index);
					}
					else {
						neg.Add(index);
					}
				}
				node.SetChild(BspTreeNode.Orientation.POSITIVE, CreateBspTree(node, allPoints, pos));
				node.SetChild(BspTreeNode.Orientation.NEGATIVE, CreateBspTree(node, allPoints, neg));

				return node;
			}

			return null;
		}

		private static void InsertNode(BspTreeNode node, List<Vector3> allPoints, int pointIndex)
		{
			if (node.IsPositive(allPoints[pointIndex]))
			{
				node.AddElement(BspTreeNode.Orientation.POSITIVE, pointIndex);
			}
			else {
				node.AddElement(BspTreeNode.Orientation.NEGATIVE, pointIndex);
			}
		}

		public static List<int> GetBackToFront(BspTreeNode node, List<Vector3> points, Vector3 eye)
		{
			if (node == null)
			{
				return new List<int>();
			}

			if (node.IsPositive(eye))
			{
				List<int> l1 = GetBackToFrontElements(node, BspTreeNode.Orientation.NEGATIVE, points, eye);
				List<int> l2 = GetBackToFrontElements(node, BspTreeNode.Orientation.POSITIVE, points, eye);
				l1.AddRange(l2);
				return l1;
			}
			else {
				List<int> l1 = GetBackToFrontElements(node, BspTreeNode.Orientation.POSITIVE, points, eye);
				List<int> l2 = GetBackToFrontElements(node, BspTreeNode.Orientation.NEGATIVE, points, eye);
				l1.AddRange(l2);
				return l1;
			}
		}

		private static List<int> GetBackToFrontElements(BspTreeNode node, BspTreeNode.Orientation orientation, List<Vector3> allPoints, Vector3 eye)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < node.getNumberOfElements(orientation); i++)
			{
				list.Add(node.getElement(orientation, i));
			}
			list.AddRange(GetBackToFront(node.GetChild(orientation), allPoints, eye));
			return list;
		}

	}
}

