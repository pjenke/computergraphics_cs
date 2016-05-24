using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace computergraphics
{
	/**
	 * Application main class.
	 * */
	class Application
	{
		public static void Main ()
		{
			// Select exercise scene to start
			Scene scene = new Exercise1 ();
			// Run game loop
			scene.GameLoop ();
		}
	}
}
