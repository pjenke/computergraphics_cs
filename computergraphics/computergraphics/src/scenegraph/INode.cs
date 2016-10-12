using OpenTK;

namespace computergraphics
{
	/**
	 * This enum allows to pass different render mode states to the drawing 
	 * routines. The default state is REGULAR.
	 * */
	public enum RenderMode
	{
		REGULAR, SHADOW_VOLUME, DEBUG_SHADOW_VOLUME
	};

	/**
	 * Shared interface for all nodes in the scene graph.
	 * */
	public abstract class INode
	{
		private INode parentNode;

		/**
		 * Render node content using OpenGL commands. Usually, drawing is done
		 * with mode = REGULAR. The transformation representens the current model 
		 * matrix and muste be passed (and if required adjusted) the the child draw calls. 
		 * */
		public abstract void Traverse(RenderMode mode, Matrix4 modelMatrix);

		/**
		 * This method is called at each timer tick. Counter provides a tick index.
		 * */
		public abstract void TimerTick(int counter);

		/**
		 * Every node must know its root node
		 * */
		public virtual RootNode GetRootNode()
		{
			return parentNode.GetRootNode();
		}

		/**
		 * Every node must know its root node
		 * */
		public void SetParentNode(INode parentNode)
		{
			this.parentNode = parentNode;
		}
	}
}

