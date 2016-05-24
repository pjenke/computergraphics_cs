using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Scene graph node for triangle meshes.
	 * */
	public class TriangleMeshNode : INode
	{

		/**
		 * Contained triangle mesh.
		 * */
		private TriangleMesh mesh;

		public TriangleMeshNode (TriangleMesh mesh)
		{
			this.mesh = mesh;
		}

		public void DrawGL ()
		{

			if (mesh.Tex != null) {
				if (!mesh.Tex.IsLoaded ()) {
					mesh.Tex.Load ();
				}
				mesh.Tex.Bind ();
			}

			GL.Begin (PrimitiveType.Triangles);
			for (int i = 0; i < mesh.GetNumberOfTriangles (); i++) {
				Triangle t = mesh.GetTriangle (i);
				GL.Color3 (Color.DarkKhaki);
				GL.Normal3 (t.normal.X, t.normal.Y, t.normal.Z); 
				for (int j = 0; j < 3; j++) {
					if (t.GetTexCoordIndex (j) >= 0) {
						GL.TexCoord2 (mesh.GetTextureCoordinate (t.GetTexCoordIndex (j)).X, mesh.GetTextureCoordinate (t.GetTexCoordIndex (j)).Y);
					}
					GL.Vertex3 (mesh.GetVertex (t.GetVertexIndex (j)).X, 
						mesh.GetVertex (t.GetVertexIndex (j)).Y, 
						mesh.GetVertex (t.GetVertexIndex (j)).Z);
				}
			}
			GL.End ();
		}

		public void TimerTick ()
		{
		}
	}
}

