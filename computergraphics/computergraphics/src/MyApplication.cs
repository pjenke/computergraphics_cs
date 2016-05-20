using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace computergraphics
{
	/**
	 * Central 3D application
	 * */
	class MyApplication
	{

		/**
		 * Main thread + event handlers.
		 * */
		[STAThread]
		public static void Main ()
		{

			Scene scene = new Scene ();
			Nullable<MouseState> previous = null;

			bool mouseDown = false;
			bool isInit = true;
		
			using (var game = new GameWindow ()) {
				game.Load += (sender, e) => {
					// setup settings, load textures, sounds
					game.VSync = VSyncMode.On;
				};

				// Resize event
				game.Resize += (sender, e) => {
					GL.Viewport (0, 0, game.Width, game.Height);
					scene.Resize (game.Width, game.Height);
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
					if (mouseDown && previous != null) {
						var deltaX = (mouse.X - previous.Value.X) / 200.0f;
						var deltaY = (mouse.Y - previous.Value.Y) / 200.0f;
						scene.RotateCamera (deltaX, deltaY);
					}
					previous = mouse;
				};

				// Mouse down
				game.MouseDown += (sender, e) => {
					var mouse = Mouse.GetState ();
					if (mouse [MouseButton.Left]) {
						mouseDown = true;
					}
				};

				// Mouse up
				game.MouseUp += (sender, e) => {
					var mouse = Mouse.GetState ();
					if (!mouse [MouseButton.Left]) {
						mouseDown = false;
						previous = null;
					}
				};
					
				// Rendering
				game.RenderFrame += (sender, e) => {
					if (isInit) {
						GL.Enable (EnableCap.DepthTest);
						game.Title = "computergraphics";
						scene.Init ();
						isInit = false;
						return;
					}

					// Clear
					GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

					// Render scene
					scene.SetProjectionMatrix ();
					scene.SetModelViewMatrix ();
					scene.Draw ();

					game.SwapBuffers ();
				};

				// Run the game at 60 updates per second
				game.Run (60.0);
			}
		}
	}
}
