using System;
using OpenTK;
namespace computergraphics
{
	public class PointCloudScene : Scene
	{
		public PointCloudScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
		{
		}

		public override void InitContent()
		{
			GetRoot().LightPosition = new Vector3(1, 1, 1);
			GetRoot().Animated = true;

			PointCloud pc = new PointCloud();
			Random random = new Random();
			for (int i = 0; i < 100; i++)
			{
				pc.Add(new Vector3((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f));
			}
			PointCloudNode node = new PointCloudNode(pc);
			GetRoot().AddChild(node);
		}
	}
}
