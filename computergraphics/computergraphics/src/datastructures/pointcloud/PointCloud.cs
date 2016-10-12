using System;
using OpenTK;
using System.Collections.Generic;

namespace computergraphics
{
	/**
	 * Container class for an unstructured point cloud
	 * */
	public class PointCloud
	{

		private List<Vector3> points = new List<Vector3>();

		public int Count
		{
			get { return points.Count;  }
		}

		public PointCloud()
		{
		}

		public void Add(Vector3 point)
		{
			points.Add(point);
		}

		public Vector3 Get(int index)
		{
			return points[index];
		}
	}
}
