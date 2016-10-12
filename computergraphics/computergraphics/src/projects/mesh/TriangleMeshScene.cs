using OpenTK;

namespace computergraphics
{
	/**
	 * Playing around with meshes
	 * */
	public class TriangleMeshScene : Scene
	{
		public TriangleMeshScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
		{
		}

		public override void InitContent()
		{
			GetRoot().LightPosition = new Vector3(1, 1, 1);
			GetRoot().Animated = true;

			ITriangleMesh mesh = new TriangleMesh();
			mesh.SetTexture(new Texture("textures/lego.png"));
			ObjReader reader = new ObjReader();
			reader.Read("meshes/cow.obj", mesh);
			TriangleMeshNode node = new TriangleMeshNode(mesh);
			node.ShowNormals = false;
			GetRoot().AddChild(node);
		}
	}
}

