using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Scales all child node in each axis according to the scale vector.
	 * */
	public class ScaleNode : GroupNode
	{

		private Vector3 scale;

		public ScaleNode (Vector3 scale)
		{
			this.scale = scale;
		}

		public override void DrawGL ()
		{
			GL.PushMatrix ();
			GL.MatrixMode (MatrixMode.Modelview);
			GL.Scale (scale);
			base.DrawGL ();
			GL.PopMatrix ();
		}

		public override void TimerTick ()
		{
			base.TimerTick ();
		}
	}
}

