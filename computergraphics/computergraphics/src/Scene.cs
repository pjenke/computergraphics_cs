using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Reprsents a 3D scene.
	 * */
	public class Scene
	{
		/**
		 * Shader (fragment + vertex)
		 * */
		Shader shader;

		/**
		 * Displayed mesh
		 * */
		TriangleMesh mesh;

		/**
		 * Virtual camera 
		 */
		Camera camera;

		public Scene ()
		{
			camera = new Camera ();
			shader = new Shader ("../../../../assets/shader/fragment_shader_phong_shading.glsl", "../../../../assets/shader/vertex_shader_phong_shading.glsl");
			mesh = new TriangleMesh ();
			mesh.CreateCube ();
		}

		/**
		 * Called once when OpenGL is ready.
		 * */
		public void Init ()
		{
			shader.CompileAndLink ();
			shader.Use ();
		}

		/**
		 * Called for each frame redraw
		 * */
		public void Draw ()
		{
			GL.Begin (PrimitiveType.Triangles);
			for (int i = 0; i < mesh.GetNumberOfTriangles (); i++) {
				Triangle t = mesh.GetTriangle (i);
				GL.Color3 (Color.Ivory);
				GL.Normal3 (t.normal.X, t.normal.Y, t.normal.Z); 
				for (int j = 0; j < 3; j++) {
					GL.Vertex3 (mesh.GetVertex (t.Get (j)).X, mesh.GetVertex (t.Get (j)).Y, mesh.GetVertex (t.Get (j)).Z);
				}
			}
			GL.End ();
		}

		/**
		 * Event callback: window resize.
		 * */
		public void Resize (int width, int height)
		{
			camera.AspectRatio = (float)width / (float)height;
		}

		/**
		 * Set the current projectio matrix.
		 * TODO: Only apply after change.
		 * */
		public void SetProjectionMatrix ()
		{
			GL.MatrixMode (MatrixMode.Projection);
			GL.LoadIdentity ();
			Matrix4 P = Matrix4.CreatePerspectiveFieldOfView (camera.getFovy (), camera.AspectRatio, camera.getZNear (), camera.getZFar ());
			GL.LoadMatrix (ref P);
		}

		/**
		 * Set the current model view matrix.
		 * TODO: Only apply after change.
		 * */
		public void SetModelViewMatrix ()
		{
			GL.MatrixMode (MatrixMode.Modelview);
			Matrix4 LookAt = camera.getLookAtMatrix ();
			GL.LoadMatrix (ref LookAt);
		}

		/**
		 * Rotate the camera left <-> right and up <-> down.
		 * */
		public void RotateCamera (float angleAroundUp, float angleUpDown)
		{
			camera.updateLookAtMatrix (angleAroundUp, angleUpDown);
		}
	}
}

