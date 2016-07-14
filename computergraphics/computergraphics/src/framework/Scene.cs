using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

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

		/**
		 * Timer to create animation events
		 * */
		private Timer timer;

		/**
		 * Projection matrix needs to be rebuilt.
		 * */
		private bool projectionMatrixUpdate = true;

		/**
		 * Model-view matrix needs to be rebuilt.
		 * */
		private bool modelviewMatrixUpdate = true;

		public Scene (int timerTimeout, Shader.ShaderMode mode)
		{
			camera = new Camera ();
			shader = new Shader (mode);
			timer = new Timer (timerTimeout);
			timer.Elapsed += new ElapsedEventHandler (OnTimedEvent);
		}

		/**
		 * Main thread + event handlers.
		 * */
		[STAThread]
		public void GameLoop ()
		{
			Nullable<MouseState> previous = null;

			bool isInit = true;

			using (var game = new GameWindow ()) {
				game.Load += (sender, e) => {
					// setup settings, load textures, sounds
					game.VSync = VSyncMode.On;
				};

				// Resize event
				game.Resize += (sender, e) => {
					GL.Viewport (0, 0, game.Width, game.Height);
					Resize (game.Width, game.Height);
				};

				// Handle key events
				game.KeyUp += (sender, e) => {
					switch (e.Key) {
					case Key.Escape:
						game.Exit ();
						break;
					}
				};

				// Mouse move
				game.MouseMove += (sender, e) => {
					var mouse = Mouse.GetState ();
					if (mouse [MouseButton.Left]) {
						if (previous != null) {
							var deltaX = (mouse.X - previous.Value.X) / 200.0f;
							var deltaY = (mouse.Y - previous.Value.Y) / 200.0f;
							RotateCamera (deltaX, deltaY);
						}
					} else if (mouse [MouseButton.Middle]) {
						if (previous != null) {
							var deltaY = (mouse.Y - previous.Value.Y);
							Zoom (deltaY);
						}
					}
					previous = mouse;
				};

				// Mouse up
				game.MouseUp += (sender, e) => {
					var mouse = Mouse.GetState ();
					if (!mouse [MouseButton.Left]) {
						previous = null;
					}
				};

				// Rendering
				game.RenderFrame += (sender, e) => {
					if (isInit) {
						GL.Enable (EnableCap.DepthTest);
						game.Title = "computergraphics";
						Init ();
						isInit = false;
						return;
					}

					// Clear
					GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    // Render scene
                    SetProjectionMatrix ();
                    SetModelViewMatrix();
                    Draw ();

					game.SwapBuffers ();
				};

				// Run the game at 60 updates per second
				game.Run (60.0);
			}
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
			rootNode.DrawGL ();
		}

		/**
		 * Event callback: window resize.
		 * */
		public void Resize (int width, int height)
		{
			camera.AspectRatio = (float)width / (float)height;
			projectionMatrixUpdate = true;
            modelviewMatrixUpdate = true;
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

		public GroupNode getRoot ()
		{
			return rootNode;
		}
	}
}

