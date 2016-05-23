using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
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
		private Shader shader;

		/**
		 * Virtual camera 
		 */
		private Camera camera;

		/**
		 * Scene graph root node
		 * */
		private GroupNode rootNode = new GroupNode ();

		private Timer timer;

		/**
		 * Projection matrix needs to be rebuilt.
		 * */
		private bool projectionMatrixUpdate = true;

		/**
		 * Model-view matrix needs to be rebuilt.
		 * */
		private bool modelviewMatrixUpdate = true;

		public Scene ()
		{
			camera = new Camera ();
			shader = new Shader (false);

			// Timer
			timer = new Timer (100);
			timer.Elapsed += new ElapsedEventHandler (OnTimedEvent);

			TriangleMesh mesh = new TriangleMesh ();
			ObjReader reader = new ObjReader ();
			reader.read (mesh, "meshes/cow.obj");
			mesh.Tex = new Texture ("textures/lego.png");
			TriangleMeshNode meshNode = new TriangleMeshNode (mesh);
			getRootNode ().Add (meshNode);


//			TranslationNode cubeTranslation = new TranslationNode(new Vector3(0,0,1));
//			CubeNode cubeNode = new CubeNode ();
//			cubeTranslation.Add (cubeNode);
//			getRootNode ().Add (cubeTranslation);
//
//			TranslationNode sphereTranslation = new TranslationNode(new Vector3(1,0,0));
//			SphereNode sphereNode = new SphereNode (0.5f, 20);
//			sphereTranslation.Add (sphereNode);
//			getRootNode ().Add (sphereTranslation);
		}

		/**
		 * Timer event handler.
		 * */
		private void OnTimedEvent (object source, ElapsedEventArgs e)
		{
			rootNode.TimerTick ();
		}

		/**
		 * Called once when OpenGL is ready.
		 * */
		public void Init ()
		{
			// ARB_separate_shader_objects
			string version_string = GL.GetString (StringName.Version);
			Console.WriteLine ("OpenGL-Version: " + version_string);

			// For OpenGL 2.1 and lower
			Dictionary<string, bool> extensions =
				new Dictionary<string, bool> ();
			string extension_string = GL.GetString (StringName.Extensions);
			foreach (string extension in extension_string.Split(' ')) {
				extensions.Add (extension, true);
			}

			// Setup shader
			shader.CompileAndLink ();
			shader.Use ();

			// Start timer
			timer.Start ();
		}

		/**
		 * Called for each frame redraw
		 * */
		public void Draw ()
		{
			shader.SetCameraEyeShaderParameter (camera.Eye);
			shader.SetUseTextureParameter ();
			rootNode.Draw ();
		}

		/**
		 * Event callback: window resize.
		 * */
		public void Resize (int width, int height)
		{
			camera.AspectRatio = (float)width / (float)height;
			projectionMatrixUpdate = true;
		}

		/**
		 * Set the current projectio matrix.
		 * */
		public void SetProjectionMatrix ()
		{
			if (projectionMatrixUpdate) {
				GL.MatrixMode (MatrixMode.Projection);
				GL.LoadIdentity ();
				Matrix4 P = Matrix4.CreatePerspectiveFieldOfView (camera.getFovy (), camera.AspectRatio, camera.getZNear (), camera.getZFar ());
				GL.LoadMatrix (ref P);
				projectionMatrixUpdate = false;
			}
		}

		/**
		 * Set the current model view matrix.
		 * */
		public void SetModelViewMatrix ()
		{
			if (modelviewMatrixUpdate) {
				GL.MatrixMode (MatrixMode.Modelview);
				Matrix4 LookAt = camera.getLookAtMatrix ();
				GL.LoadMatrix (ref LookAt);
				modelviewMatrixUpdate = false;
			}
		}

		/**
		 * Rotate the camera left <-> right and up <-> down.
		 * */
		public void RotateCamera (float angleAroundUp, float angleUpDown)
		{
			camera.updateLookAtMatrix (angleAroundUp, angleUpDown);
			modelviewMatrixUpdate = true;
		}

		public void Zoom (int factor)
		{
			camera.Zoom (factor);
			modelviewMatrixUpdate = true;
		}

		public GroupNode getRootNode ()
		{
			return rootNode;
		}
	}
}

