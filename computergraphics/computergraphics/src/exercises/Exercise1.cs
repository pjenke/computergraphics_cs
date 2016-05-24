using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	public class Exercise1 : Scene
	{
		public Exercise1 () : base (Shader.ShaderMode.TEXTURE)
		{
			// Sphere geometry
			TranslationNode sphereTranslation =
				new TranslationNode (new Vector3 (1.1f, 0, 0));
			SphereNode sphereNode = new SphereNode (0.5f, 20);
			sphereTranslation.AddChild (sphereNode);
			getRoot ().AddChild (sphereTranslation);

			// Cube geometry
			TranslationNode cubeTranslation =
				new TranslationNode (new Vector3 (0, 0, 1.1f));
			CubeNode cubeNode = new CubeNode (0.5f);
			cubeTranslation.AddChild (cubeNode);
			getRoot ().AddChild (cubeTranslation);

			// Triangle mesh
			TriangleMesh mesh = new TriangleMesh ("textures/lego.png");
			ObjReader reader = new ObjReader ();
			reader.read ("meshes/cube.obj", mesh);
			TriangleMeshNode meshNode = new TriangleMeshNode (mesh);
			getRoot ().AddChild (meshNode);

			Console.WriteLine ("Added");
		}
	}
}

