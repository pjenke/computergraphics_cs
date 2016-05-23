using System;
using System.Collections;

namespace computergraphics
{
	/**
	 * A group node has a number of children and passes information to the (e.g. render call events)
	 * */
	public class GroupNode : INode
	{
		ArrayList children = new ArrayList();

		public GroupNode ()
		{
		}
			
		public virtual void Draw(){
			foreach (INode child in children ){
				child.Draw();
			}
		}

		public virtual void TimerTick()
		{
		}

		/**
		 * Add new child node.
		 * */
		public void Add(INode child)
		{
			children.Add (child);
		}
	}
}

