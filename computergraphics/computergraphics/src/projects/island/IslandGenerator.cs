using System;
using System.Collections.Generic;
using OpenTK;

namespace computergraphics
{
	public class IslandGenerator
	{
		public IslandGenerator()
		{
		}

		public ITriangleMesh GenerateIslandMesh()
		{
			HexagonField field = new HexagonField();
			field.GenerateField();

			ITriangleMesh mesh = new TriangleMesh();

			// Top
			ITriangleMesh topMesh = CreateTopSurface(field.GetAllHexagons());
			TriangleMeshFactory.Unite(mesh, topMesh);

			// Grass
			float grassBorderHeight = field.SideLength * 0.1f;
			Vector2[] grassBoundaryTexCoords = { new Vector2(0.1f, 0.1f), new Vector2(0.1f, 0.2f), new Vector2(0.2f, 0.1f), new Vector2(0.2f, 0.2f)};
			ITriangleMesh grassBoundary = CreateBoundary(mesh, field.SideLength, grassBorderHeight, grassBorderHeight, new Vector3(0, 0, 0), grassBoundaryTexCoords);
			TriangleMeshFactory.Unite(mesh, grassBoundary);

			// Middle-bottom
			ITriangleMesh middleMesh = CreateMiddle(mesh, field.GetBorderHexagons(), field.SideLength);
			TriangleMeshFactory.Unite(mesh, middleMesh);

			ITriangleMesh bottomMesh = CreateTopSurface(field.GetNonBorderHexagons());

			// Mud
			float mudHeight = 2 * field.SideLength;
			float mudBorderHeight = field.SideLength * 0.3f;
			Vector2[] mudBoundaryTexCoords = { new Vector2(0.7f, 0.1f), new Vector2(0.7f, 0.2f), new Vector2(0.8f, 0.1f), new Vector2(0.8f, 0.2f) };
			ITriangleMesh mudBoundary = CreateBoundary(bottomMesh, mudHeight, 0, mudBorderHeight, new Vector3(0, 0, 0), mudBoundaryTexCoords);
			TriangleMeshFactory.Translate(mudBoundary, new Vector3(0, 0, -field.SideLength));
			TriangleMeshFactory.Unite(mesh, mudBoundary);

			// Bottom
			TriangleMeshFactory.Flip(bottomMesh);
			TriangleMeshFactory.Translate(bottomMesh, new Vector3(0, 0, -(mudHeight + field.SideLength)));
			TriangleMeshFactory.Unite(mesh, bottomMesh);

			mesh = TriangleMeshFactory.Snap(mesh, 1e-5f);

			mesh.ComputeTriangleNormals();
			mesh.SetTexture(new Texture("textures/island.png"));
			return mesh;
		}

		private ITriangleMesh CreateMiddle(ITriangleMesh mesh, List<Hexagon> borderHexagons, float sideLength)
		{
			ITriangleMesh bottomMesh = new TriangleMesh();
			foreach (Hexagon hex in borderHexagons)
			{
				ITriangleMesh hexMesh = hex.GenerateMesh();
				TriangleMeshFactory.Flip(hexMesh);
				TriangleMeshFactory.Unite(bottomMesh, hexMesh);
			}
			TriangleMeshFactory.Translate(bottomMesh, new Vector3(0, 0, -sideLength));
			return bottomMesh;
		}

		/**
		 * Create the mesh for the top.
		 * */
		private ITriangleMesh CreateTopSurface(List<Hexagon> hexagons)
		{
			ITriangleMesh mesh = new TriangleMesh();
			foreach (Hexagon hex in hexagons)
			{
				if (!hex.ConsistencyCheck())
				{
					Console.WriteLine("Inconsistency detected!");
				}
				ITriangleMesh hexMesh = hex.GenerateMesh();
				TriangleMeshFactory.Unite(mesh, hexMesh);
			}
			mesh = TriangleMeshFactory.Snap(mesh, 1e-5f);
			return mesh;
		}

		private ITriangleMesh CreateBoundary(ITriangleMesh mesh, float height, float borderHeightTop, float borderHeightBottom, Vector3 offset, Vector2[] cornerTexCoords)
		{
			List<TriangleEdge> border = mesh.GetBorder();
			int vertsPerBorderEdge = 3;
			ITriangleMesh borderMesh = new TriangleMesh();

			for (int i = 0; i < border.Count; i++)
			{
				borderMesh.AddVertex(mesh.GetVertex(border[i].A));
			}
			for (int i = 0; i < border.Count; i++)
			{
				TriangleEdge borderEdge = border[i];
				TriangleEdge prevEdge = border[(i + border.Count - 1) % border.Count];
				Vector3 v = borderMesh.GetVertex(i);

				// v0
				Vector3 edgeNormal = ComputeBorderEdgeNormal(borderEdge, mesh);
				Vector3 prevEdgeNormal = ComputeBorderEdgeNormal(prevEdge, mesh);
				Vector3 borderNormal = Vector3.Add(edgeNormal, prevEdgeNormal);
				borderNormal.Normalize();
				borderNormal = Vector3.Multiply(borderNormal, Math.Max(borderHeightTop, borderHeightBottom) / (float)Math.Sin(30.0 / 180.0 * Math.PI));
				Vector3 v0 = Vector3.Add(Vector3.Add(v, borderNormal), new Vector3(0, 0, -borderHeightTop));
				borderMesh.AddVertex(v0);

				// v1
				Vector3 v1 = Vector3.Add(v0, Vector3.Multiply(Vector3.UnitZ, -(height - (borderHeightTop + borderHeightBottom))));
				borderMesh.AddVertex(v1);

				// v2
				Vector3 v2 = Vector3.Add(v, Vector3.Multiply(Vector3.UnitZ, -height));
				borderMesh.AddVertex(v2);
			}

			float alpha = (float)(borderHeightTop * Math.Sqrt(2) / (borderHeightTop * Math.Sqrt(2) + borderHeightBottom * Math.Sqrt(2) + height));
			float beta = 1 - (float)(borderHeightBottom * Math.Sqrt(2) / (borderHeightTop * Math.Sqrt(2) + borderHeightBottom * Math.Sqrt(2) + height));
			int ti0 = borderMesh.AddTextureCoordinate(cornerTexCoords[0]);
			int ti1 = borderMesh.AddTextureCoordinate(Vector2.Add(Vector2.Multiply(cornerTexCoords[0], 1-alpha), Vector2.Multiply(cornerTexCoords[1], alpha)));
			int ti2 = borderMesh.AddTextureCoordinate(Vector2.Add(Vector2.Multiply(cornerTexCoords[0], 1-beta), Vector2.Multiply(cornerTexCoords[1], beta)));
			int ti3 = borderMesh.AddTextureCoordinate(cornerTexCoords[1]);
			int ti_0 = borderMesh.AddTextureCoordinate(cornerTexCoords[2]);
			int ti_1 = borderMesh.AddTextureCoordinate(Vector2.Add(Vector2.Multiply(cornerTexCoords[2], 1 - alpha), Vector2.Multiply(cornerTexCoords[3], alpha)));
			int ti_2 = borderMesh.AddTextureCoordinate(Vector2.Add(Vector2.Multiply(cornerTexCoords[2], 1 - beta), Vector2.Multiply(cornerTexCoords[3], beta)));
			int ti_3 = borderMesh.AddTextureCoordinate(cornerTexCoords[3]);

			// Triangles
			for (int i = 0; i < border.Count; i++)
			{
				TriangleEdge borderEdge = border[i];
				int vi0 = border.Count + vertsPerBorderEdge * i + 0;
				int vi1 = border.Count + vertsPerBorderEdge * i + 1;
				int vi2 = border.Count + vertsPerBorderEdge * i + 2;
				int vi_0 = border.Count + vertsPerBorderEdge * ((i + 1) % border.Count) + 0;
				int vi_1 = border.Count + vertsPerBorderEdge * ((i + 1) % border.Count) + 1;
				int vi_2 = border.Count + vertsPerBorderEdge * ((i + 1) % border.Count) + 2;
				// A
				Triangle t = new Triangle(i, vi0, (i + 1) % border.Count, ti0, ti1, ti_1);
				borderMesh.AddTriangle(t);
				// B
				t = new Triangle((i + 1) % border.Count, vi0, vi_0, ti_0, ti1, ti_1);
				borderMesh.AddTriangle(t);
				// C
				t = new Triangle(vi0, vi1, vi_0, ti1, ti2, ti_1 );
				borderMesh.AddTriangle(t);
				// D
				t = new Triangle(vi1, vi_1, vi_0, ti2, ti_2, ti_1);
				borderMesh.AddTriangle(t);
				// E
				t = new Triangle(vi1, vi2, vi_1, ti2, ti3, ti_2);
				borderMesh.AddTriangle(t);
				// F
				t = new Triangle(vi2, vi_2, vi_1, ti3, ti_3, ti_2);
				borderMesh.AddTriangle(t);
			}

			return borderMesh;
		}

		/**
 		 * Compute the normal of the border edge.
 		 * */
		private Vector3 ComputeBorderEdgeNormal(TriangleEdge edge, ITriangleMesh mesh)
		{
			Vector3 v0 = mesh.GetVertex(edge.A);
			Vector3 v1 = mesh.GetVertex(edge.B);
			Vector3 e = Vector3.Subtract(v1, v0);
			Vector3 n = Vector3.Cross(e, Vector3.UnitZ);
			n.Normalize();
			return n;
		}
	}
}
