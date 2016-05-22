using System;
using System.Drawing;
using System.Drawing.Imaging;
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

		/**
		 * Scene graph root node
		 * */
		GroupNode rootNode = new GroupNode ();

		public Scene ()
		{
			camera = new Camera ();
			shader = new Shader ("../../../../assets/shader/fragment_shader_phong_shading.glsl", "../../../../assets/shader/vertex_shader_phong_shading.glsl");
			mesh = new TriangleMesh ();
			mesh.CreateCube ();
			ObjReader reader = new ObjReader ();
			reader.read (mesh, "../../../../assets/meshes/cube.obj");
			TriangleMeshNode meshNode = new TriangleMeshNode (mesh);
			getRootNode ().Add (meshNode);
		}

		/**
		 * Called once when OpenGL is ready.
		 * */
		public void Init ()
		{
			// Setup shader
			shader.CompileAndLink ();
			shader.Use ();
		}

		/**
		 * Called for each frame redraw
		 * */
		public void Draw ()
		{
			shader.SetCameraEyeShaderParameter (camera.Eye);
			shader.SetUseTextureParameter (true);
			rootNode.Draw ();
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

		public void Zoom(int factor)
		{
			camera.Zoom(factor);
		}

		public GroupNode getRootNode()
		{
			return rootNode;
		}
			
		/***
		 * TODO
		 * */
		private int LoadTexture(string filename)
		{
			if (String.IsNullOrEmpty(filename))
				throw new ArgumentException(filename);

			int id = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, id);

			// We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
			// We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
			// mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			Bitmap bmp = new Bitmap(filename);
			BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

			bmp.UnlockBits(bmp_data);

			return id;
		}
	}
}

