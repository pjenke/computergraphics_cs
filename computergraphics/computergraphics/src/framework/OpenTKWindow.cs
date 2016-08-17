using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace computergraphics
{
	public class OpenTKWindow
	{

		/**
		 * Represented scene object.
		 * */
		private Scene scene;

		public OpenTKWindow(Scene scene)
		{
			this.scene = scene;
		}

		/**
		 * Main thread + event handlers.
		 * */
		[STAThread]
		public void Run()
		{
			var graphicsmode = new GraphicsMode(
								   GraphicsMode.Default.ColorFormat,
									GraphicsMode.Default.Depth,
								   8,
								   GraphicsMode.Default.Samples,
								   GraphicsMode.Default.AccumulatorFormat,
								   GraphicsMode.Default.Buffers,
								   GraphicsMode.Default.Stereo);


			var game = new GameWindow(640, 480,
									  graphicsmode,
									  "Computergraphics (using OpenTK Game Window)",
									  GameWindowFlags.Default,
									  DisplayDevice.Default,
									  2, 1,
									  GraphicsContextFlags.ForwardCompatible);


			game.Load += (sender, e) =>
			{
					// setup settings, load textures, sounds
					game.VSync = VSyncMode.On;
				scene.Init();
			};

			// Resize event
			game.Resize += (sender, e) =>
			{
				GL.Viewport(0, 0, game.Width, game.Height);
				scene.Resize(game.Width, game.Height);
			};

			// Handle key events
			game.KeyUp += (sender, e) =>
			{
				scene.HandleKeyEvent(e.Key);
			};

			// Mouse move
			game.MouseMove += (sender, e) =>
			{
				var mouse = Mouse.GetState();
				Scene.MouseButton button = Scene.MouseButton.NONE;
				if (mouse[MouseButton.Left])
				{
					button = Scene.MouseButton.LEFT;
				}
				else if (mouse[MouseButton.Middle])
				{
					button = Scene.MouseButton.MIDDLE;
				}
				else if (mouse[MouseButton.Right])
				{
					button = Scene.MouseButton.RIGHT;
				}
				scene.HandleMouseEvent(button, mouse.X, mouse.Y);
			};

			// Rendering
			game.RenderFrame += (sender, e) =>
			{
				scene.Redraw();
				game.SwapBuffers();
			};

			// Run the game at 60 updates per second
			game.Run(60.0);
		}
	}
}

