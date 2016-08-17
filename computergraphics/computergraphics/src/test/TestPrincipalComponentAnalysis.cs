using System;
using OpenTK;

namespace computergraphics
{
	/**
	 * Test class for PrincipalComponentAnalysis
	*/
	public class TestPrincipalComponentAnalysis
	{
		public TestPrincipalComponentAnalysis()
		{
		}

		public static void testPCA()
		{
			Vector3 x = new Vector3(2, 0, 0);
			Vector3 y = new Vector3(0, 1, 0);
			Vector3 z = new Vector3(0, 0, 0.5f);
			int numberOfSamples = 100;
			Random random = new Random();
			PrincipalComponentAnalysis pca = new PrincipalComponentAnalysis();
			for (int i = 0; i < numberOfSamples; i++)
			{
				Vector3 data = Vector3.Add(
					Vector3.Add(
						Vector3.Multiply(x, (float)random.NextDouble()),
						Vector3.Multiply(y, (float)random.NextDouble())),
					Vector3.Multiply(z, (float)random.NextDouble()));
				pca.Add(data);
			}
			pca.applyPCA();
			Console.WriteLine("centroid: " + pca.getCentroid());
			Console.WriteLine("eigenvalues: " + pca.getEigenValues());
			for (int i = 0; i < 3; i++)
			{
				Console.WriteLine("eigenvector " + i + ": " + pca.getEigenVector(i));
			}
		}
	}
}

