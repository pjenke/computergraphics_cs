using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Translates all child node along a given vector.
	 * */
	public class TranslationNode : GroupNode
	{

		private Vector3 translation;

		public TranslationNode (Vector3 translation)
		{
			this.translation = translation;
		}

		public override void DrawGL()
		{
			GL.PushMatrix ();
			GL.MatrixMode(MatrixMode.Modelview);
			GL.Translate (translation);
			base.DrawGL ();
			GL.PopMatrix ();
		}

		public override void TimerTick ()
		{
			base.TimerTick ();
		}
	}
}

