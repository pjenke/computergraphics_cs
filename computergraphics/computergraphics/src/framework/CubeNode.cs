using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/** 
	 * Draws a cube.
	 * */
	public class CubeNode  : INode
	{

		/**
		 * Cube is centered at the origin and its sidelength is twice this value.
		 * */
		private float sideLength = 0.5f;

		public CubeNode ()
		{
		}

		public void Draw ()
		{
			GL.Begin (PrimitiveType.Triangles);
			GL.Color4 (Color4.Aquamarine);
			GL.Normal3 (0, 0, -1);
			GL.Vertex3 (-sideLength, -sideLength, -sideLength);
			GL.Vertex3 (sideLength, -sideLength, -sideLength);
			GL.Vertex3 (sideLength, sideLength, -sideLength);
			GL.Vertex3 (-sideLength, -sideLength, -sideLength);
			GL.Vertex3 (sideLength, sideLength, -sideLength);
			GL.Vertex3 (-sideLength, sideLength, -sideLength);

			GL.Normal3 (1, 0, 0);
			GL.Vertex3 (sideLength, -sideLength, -sideLength);
			GL.Vertex3 (sideLength, -sideLength, sideLength);
			GL.Vertex3 (sideLength, sideLength, -sideLength);
			GL.Vertex3 (sideLength, sideLength, -sideLength);
			GL.Vertex3 (sideLength, -sideLength, sideLength);
			GL.Vertex3 (sideLength, sideLength, sideLength);

			GL.Normal3 (0, 0, 1);
			GL.Vertex3 (-sideLength, -sideLength, sideLength);
			GL.Vertex3 (sideLength, -sideLength, sideLength);
			GL.Vertex3 (sideLength, sideLength, sideLength);
			GL.Vertex3 (-sideLength, -sideLength, sideLength);
			GL.Vertex3 (sideLength, sideLength, sideLength);
			GL.Vertex3 (-sideLength, sideLength, sideLength);

			GL.Normal3 (-1, 0, 0);
			GL.Vertex3 (-sideLength, -sideLength, sideLength); 
			GL.Vertex3 (-sideLength, -sideLength, -sideLength); 
			GL.Vertex3 (-sideLength, sideLength, sideLength); 
			GL.Vertex3 (-sideLength, -sideLength, -sideLength); 
			GL.Vertex3 (-sideLength, sideLength, -sideLength); 
			GL.Vertex3 (-sideLength, sideLength, sideLength); 

			GL.Normal3 (0, 1, 0);
			GL.Vertex3 (-sideLength, sideLength, -sideLength); 
			GL.Vertex3 (sideLength, sideLength, -sideLength); 
			GL.Vertex3 (-sideLength, sideLength, sideLength); 
			GL.Vertex3 (sideLength, sideLength, -sideLength); 
			GL.Vertex3 (sideLength, sideLength, sideLength); 
			GL.Vertex3 (-sideLength, sideLength, sideLength); 

			GL.Normal3 (0, -1, 0);
			GL.Vertex3 (-sideLength, -sideLength, sideLength); 
			GL.Vertex3 (sideLength, -sideLength, -sideLength); 
			GL.Vertex3 (-sideLength, -sideLength, -sideLength); 
			GL.Vertex3 (sideLength, -sideLength, -sideLength); 
			GL.Vertex3 (-sideLength, -sideLength, sideLength);
			GL.Vertex3 (sideLength, -sideLength, sideLength);

			GL.End ();
		}

		public void TimerTick()
		{
		}
	}
}

