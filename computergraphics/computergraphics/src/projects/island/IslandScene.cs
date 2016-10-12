using System;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace computergraphics
{
	public class IslandScene : Scene
	{
		public IslandScene() : base(100, Shader.ShaderMode.TEXTURE, RenderMode.REGULAR)
		{
			
		}

		public override void InitContent()
		{
			GetRoot().LightPosition = new Vector3(0, -2, 2);
			GetRoot().Animated = true;
			IslandGenerator gen = new IslandGenerator();
			ITriangleMesh mesh = gen.GenerateIslandMesh();
			TriangleMeshNode node = new TriangleMeshNode(mesh);
			node.ShowBorder = true;
			node.SetColor(Color4.DarkGreen);
			node.ShowNormals = false;
			GetRoot().AddChild(node);
		}
	}
}
