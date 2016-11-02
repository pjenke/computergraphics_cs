using System;
using OpenTK;
namespace computergraphics
{
	public class PointCloudScene : Scene
	{
		private PointCloud pc;
		private PointCloudNode node;

		public PointCloudScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
		{
		}

		public override void InitContent()
		{
			GetRoot().LightPosition = new Vector3(1, 1, 1);
			GetRoot().Animated = true;

			pc = new PointCloud();
			Random random = new Random();
			for (int i = 0; i < 100; i++)
			{
				pc.Add(new Vector3((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f));
			}

			node = new PointCloudNode(pc);
			GetRoot().AddChild(node);
		}

		public override void TimerTick(int timerCounter)
		{
			Matrix3 rot = Matrix3.CreateRotationX(0.05f);	
			for (int i = 0; i < pc.Count; i++)
			{
				Vector3 p = pc.Get(i);
				p = MathHelper.Multiply(rot, p);
				pc.Set(i, p);
			}
			node.updateVBO();
		}
	}
}
