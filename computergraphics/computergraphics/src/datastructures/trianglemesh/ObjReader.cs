using System;
using OpenTK;

namespace computergraphics
{
	/**
	 * Reads an OBJ file to a triangle mesh data structure.
	 * */
	public class ObjReader
	{
		/***
		 * Reference to the triangle mesh to be filled.
		 * */
		private ITriangleMesh mesh;

		public ObjReader ()
		{
		}

		/**
		 * Read an OBJ file an create a mesh from the content.
		 * */
		public void Read (string filename, ITriangleMesh mesh)
		{
			this.mesh = mesh;
			mesh.Clear ();

			// Read input
			string objSource = System.IO.File.ReadAllText (AssetPath.GetPathToAsset (filename));

			string[] lines = objSource.Split ('\n');
			foreach (String line in lines) {
				string[] tokens = line.Trim ().Split (' ', '\t');
				if (tokens.Length > 0) {
					if (tokens [0].CompareTo ("v") == 0) {
						ParseVertex (tokens);
					} else if (tokens [0].CompareTo ("f") == 0) {
						ParseFacet (tokens);
					} else if (tokens [0].CompareTo ("vt") == 0) {
						ParseTextureCoordinate (tokens);
					}
				}
			}

			mesh.ComputeTriangleNormals ();
			Console.WriteLine ("Read mesh from file " + filename + " with " + mesh.GetNumberOfTriangles () + " triangles and " + mesh.GetNumberOfVertices () + " vertices."); 
		}

		/**
		 * Parse a line containing a vertex
		 **/
		private void ParseVertex (String[] tokens)
		{
			if (tokens.Length < 4) {
				return;
			}
			Vector3 x = new Vector3 (float.Parse (tokens [1], System.Globalization.CultureInfo.InvariantCulture), 
				            float.Parse (tokens [2], System.Globalization.CultureInfo.InvariantCulture), 
				            float.Parse (tokens [3], System.Globalization.CultureInfo.InvariantCulture));
			mesh.AddVertex (x);
		}

		/**
		 * Parse a line containing a vertex
		 **/
		private void ParseTextureCoordinate (String[] tokens)
		{
			if (tokens.Length < 3) {
				return;
			}
			Vector2 x = new Vector2 (float.Parse (tokens [1], System.Globalization.CultureInfo.InvariantCulture), 
				            float.Parse (tokens [2], System.Globalization.CultureInfo.InvariantCulture));
			mesh.AddTextureCoordinate (x);
		}

		/**
		 * Parse a line containing a facet
		 **/
		private void ParseFacet (String[] tokens)
		{
			if (tokens.Length < 4) {
				return;
			}
			int[] vertexIndices = { -1, -1, -1 };
			int[] texCoordIndices = { -1, -1, -1 };
			bool hasTextureCoordinates = false;
			for (int i = 0; i < 3; i++) {
				string field = tokens [i + 1];
				string[] slashSeparatedIndices = field.Split ('/');
				vertexIndices [i] = int.Parse (slashSeparatedIndices [0]) - 1;
				if (slashSeparatedIndices.Length > 1) {
					texCoordIndices [i] = int.Parse (field.Split ('/') [1]) - 1;
					hasTextureCoordinates = true;
				}
			}
			if (!hasTextureCoordinates) {
				mesh.AddTriangle (vertexIndices [0], vertexIndices [1], vertexIndices [2]); 
			} else {
				Triangle t = new Triangle (vertexIndices [0], vertexIndices [1], vertexIndices [2], texCoordIndices [0], texCoordIndices [1], texCoordIndices [2]);
				mesh.AddTriangle (t);
			}
		}
	}
}

