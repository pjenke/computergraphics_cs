using OpenTK;
using System.Collections.Generic;

namespace computergraphics
{
	/**
	 * Apply principal component analysis to a dataset
	*/
	public class PrincipalComponentAnalysis
	{

		private List<Vector3> points = new List<Vector3>();
		private Vector3 eigenValues;
		private Vector3[] eigenVectors = new Vector3[3];
		private Vector3 centroid;

		public PrincipalComponentAnalysis()
		{
		}

		/**
		 * Add new point to dataset to be analyzed
		*/
		public void Add(Vector3 point)
		{
			points.Add(point);
		}

		/**
		 * Clear all results and data points.
		 * */
		public void clear()
		{
			points.Clear();
		}

		/**
		 * Analyze the data
		 * */
		public void applyPCA()
		{
			centroid = new Vector3(0, 0, 0);
			double[,] x = new double[points.Count, 3];
			for (int i = 0; i < points.Count; i++)
			{
				x[i, 0] = points[i].X;
				x[i, 1] = points[i].Y;
				x[i, 2] = points[i].Z;
				centroid = Vector3.Add(centroid, points[i]);
			}
			centroid = Vector3.Multiply(centroid, 1.0f / points.Count);
			int info;
			double [] eval = { -1, -1, -1};
			double [,] evec = { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };
			alglib.pcabuildbasis(x, points.Count, 3, out info, out eval, out evec);
			eigenValues = new Vector3((float)eval[2], (float)eval[1], (float)eval[0]);
			for (int i = 0; i < 3; i++)
			{
				eigenVectors[i] = new Vector3((float)evec[2-i, 0], (float)evec[2-i, 1], (float)evec[2-i, 2]);
			}
		}

		/**
		 * Return the three (sorted) eigenvalues, ascending order
		 * */
		public Vector3 getEigenValues()
		{
			return eigenValues;
		}

		/**
		 * Return the eigenvector corresponding to the eigenvalue with the given index (ascending order).
		 * */
		public Vector3 getEigenVector( int index)
		{
			return eigenVectors[index];
		}

		/**
		 * Get the data centroid; only valid after analysis.
		 * */
		public Vector3 getCentroid()
		{
			return centroid;
		}
	}
}

