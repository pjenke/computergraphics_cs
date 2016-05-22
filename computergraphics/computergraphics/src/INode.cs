using System;

namespace computergraphics
{
	/**
	 * Shared interface for all nodes in the scene graph.
	 * */
	public interface INode
	{
		/**
		 * Render node content using OpenGL commands.
		 * */
		void Draw();
	}
}

