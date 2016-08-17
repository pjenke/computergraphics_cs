using OpenTK;
using OpenTK.Input;

namespace computergraphics
{
	public class Exercise1 : Scene
	{
		// Base constructor: Timer timeout between two TimerTick events and shader mode (PHONG, TEXTURE, NO_LIGHTING)
		public Exercise1() : base(100, Shader.ShaderMode.PHONG)
		{
			GetRoot().LightPosition = new Vector3(1, 1, 1);
			GetRoot().Animated = true;

			// Sphere geometry
			TranslationNode sphereTranslation =
				new TranslationNode(new Vector3(1, -0.5f, 0));
			INode sphereNode = new SphereNode(0.5f, 20);
			sphereTranslation.AddChild(sphereNode);
			GetRoot().AddChild(sphereTranslation);

			// Cube geometry
			TranslationNode cubeTranslation =
				new TranslationNode(new Vector3(-1, 0.5f, 0));
			INode cubeNode = new CubeNode(0.5f);
			cubeTranslation.AddChild(cubeNode);
			GetRoot().AddChild(cubeTranslation);
		}

		public override void KeyPressed(Key key)
		{
			// Key pressed event
		}

		public override void TimerTick(int counter)
		{
			// Timer tick event
		}
	}
}

