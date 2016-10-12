using System.Collections;
using OpenTK;

namespace computergraphics
{
	/**
	 * A group node has a number of children and passes information to the (e.g. render call events)
	 * */
	public class InnerNode : INode
	{
		/**
		 * List of child nodes.
		 * */
		ArrayList children = new ArrayList ();

		public InnerNode ()
		{
		}

		public override void Traverse (RenderMode mode, Matrix4 modelMatrix)
		{
			foreach (INode child in children) {
				child.Traverse (mode, modelMatrix);
			}
		}

		public override void TimerTick ( int counter )
		{
			foreach (INode child in children) {
				child.TimerTick (counter);
			}
		}

		/**
		 * Add new child node.
		 * */
		public void AddChild (INode child)
		{
			child.SetParentNode(this);
			children.Add (child);
		}
	}
}

