using OpenTK;
using System.Collections.Generic;
using System.Collections;

namespace computergraphics
{
	/**
	 * Triangle mesh data structure in indexed format.
	 * */
	public class TriangleMesh : ITriangleMesh
	{
		/**
		 * List of vertices.
		 * */
		private List<Vector3> vertices = new List<Vector3>();

		/**
		 * List of indexed triangles.
		 * */
		private List<Triangle> triangles = new List<Triangle>();

		/**
		 * List of texture coordinates.
		 * */
		private List<Vector2> textureCoordinates = new List<Vector2>();

		/**
		 * Triangle object, null if no texture is used
		 * */
		private Texture texture = null;

		public TriangleMesh()
		{
			texture = null;
		}

		public TriangleMesh(string textureFilename)
		{
			texture = new Texture(textureFilename);
		}

		public int AddVertex(Vector3 vertex)
		{
			vertices.Add(vertex);
			return vertices.Count - 1;
		}

		public void AddTriangle(int a, int b, int c)
		{
			Triangle t = new Triangle(a, b, c);
			triangles.Add(t);
		}

		public void AddTriangle(Triangle t)
		{
			triangles.Add(t);
		}

		public int GetNumberOfTriangles()
		{
			return triangles.Count;
		}

		public void AddTextureCoordinate(Vector2 texCoord)
		{
			textureCoordinates.Add(texCoord);
		}

		public Triangle GetTriangle(int index)
		{
			return triangles[index];
		}

		public int GetNumberOfVertices()
		{
			return vertices.Count;
		}

		public Vector3 GetVertex(int index)
		{
			return vertices[index];
		}

		public Vector2 GetTextureCoordinate(int index)
		{
			return textureCoordinates[index];
		}

		public Texture GetTexture()
		{
			return texture;
		}

		public void SetTexture(Texture texture)
		{
			this.texture = texture;
		}

		/**
		 * Compute normals for all triangles.
		 * */
		public void ComputeTriangleNormals()
		{
			for (int i = 0; i < GetNumberOfTriangles(); i++)
			{
				Triangle t = triangles[i];
				Vector3 a = vertices[triangles[i].GetVertexIndex(0)];
				Vector3 b = vertices[triangles[i].GetVertexIndex(1)];
				Vector3 c = vertices[triangles[i].GetVertexIndex(2)];
				Vector3 normal = Vector3.Cross(Vector3.Subtract(b, a), Vector3.Subtract(c, a));
				normal.Normalize();
				triangles[i].normal = normal;
			}
		}

		/**
		 * Clear all content.
		 * */
		public void Clear()
		{
			vertices.Clear();
			triangles.Clear();
			textureCoordinates.Clear();
		}

		public void CreateShadowPolygons(Vector3 lightPosition, float extend, ITriangleMesh shadowPolygonMesh)
		{
			shadowPolygonMesh.Clear();
			ArrayList silhouetteEdges = GetSilhouette(lightPosition);
			for (int i = 0; i < silhouetteEdges.Count; i++)
			{
				Edge edge = (Edge)silhouetteEdges[i];
				Vector3 v0 = GetVertex(edge.a);
				Vector3 v1 = GetVertex(edge.b);
				Vector3 dv0 = Vector3.Multiply(Vector3.Subtract(v0, lightPosition).Normalized(), extend);
				Vector3 dv1 = Vector3.Multiply(Vector3.Subtract(v1, lightPosition).Normalized(), extend);
				Vector3 v0Dash = Vector3.Add(v0, dv0);
				Vector3 v1Dash = Vector3.Add(v1, dv1);

				int v0Index = shadowPolygonMesh.AddVertex(v0);
				int v1Index = shadowPolygonMesh.AddVertex(v1);
				int v0DashIndex = shadowPolygonMesh.AddVertex(v0Dash);
				int v1DashIndex = shadowPolygonMesh.AddVertex(v1Dash);
				shadowPolygonMesh.AddTriangle(v0Index, v0DashIndex, v1DashIndex);
				shadowPolygonMesh.AddTriangle(v0Index, v1DashIndex, v1Index);
			}
			shadowPolygonMesh.ComputeTriangleNormals();
		}

		/**
		 * Compute the silhouette (list of edges) for a given position
		 * */
		public ArrayList GetSilhouette(Vector3 position)
		{
			ArrayList silhouetteEdges = new ArrayList();
			Dictionary<Edge, int> edge2FacetMap = new Dictionary<Edge, int>();
			for (int triangleIndex = 0; triangleIndex < triangles.Count; triangleIndex++)
			{
				Triangle t = triangles[triangleIndex];
				for (int i = 0; i < 3; i++)
				{
					int a = t.GetVertexIndex(i);
					int b = t.GetVertexIndex((i + 1) % 3);
					Edge edge = new Edge(a, b);
					if (edge2FacetMap.ContainsKey(edge))
					{
						// Opposite egde found
						int oppositeTriangle = edge2FacetMap[edge];
						if (IsSilhouetteEdge(position, triangleIndex, oppositeTriangle, edge))
						{
							silhouetteEdges.Add(edge);
						}
						// Debugging: if edge map is empty in the end, then the surface is closed.
						edge2FacetMap.Remove(edge);
					}
					else {
						// Opposite edge not yet found
						edge2FacetMap.Add(edge, triangleIndex);
					}
				}
			}

			//Console.WriteLine ("Silhouette edge map size (should be 0): " + edge2FacetMap.Count + " (#edges: " + triangles.Count*3 + ")");

			return silhouetteEdges;
		}

		/**
		 * Returns true if the edge between the two given triangles is a silhouette edge for the given position.
		 * */
		private bool IsSilhouetteEdge(Vector3 position, int triangle1Index, int triangle2Index, Edge edge)
		{
			Triangle t1 = triangles[triangle1Index];
			Triangle t2 = triangles[triangle2Index];
			Vector3 v1 = vertices[t1.GetVertexIndex(0)];
			Vector3 v2 = vertices[t2.GetVertexIndex(0)];
			double d1 = Vector3.Dot(t1.Normal, position) - Vector3.Dot(t1.Normal, v1);
			double d2 = Vector3.Dot(t2.Normal, position) - Vector3.Dot(t2.Normal, v2);
			if (d1 < 0)
			{
				edge.Flip();
			}
			return d1 * d2 < 0;
		}
	}
}

