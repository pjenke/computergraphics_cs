using OpenTK;

namespace computergraphics
{
	public class TriangleMeshScene : Scene
	{
		public TriangleMeshScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
		{
			GetRoot().LightPosition = new Vector3(1, 1, 1);
			GetRoot().Animated = true;

			ITriangleMesh mesh = new TriangleMesh();
			ObjReader reader = new ObjReader();
			reader.Read("meshes/cow.obj", mesh);
			TriangleMeshNode node = new TriangleMeshNode(mesh);
			node.ShowNormals = false;
			GetRoot().AddChild(node);
		}
	}
}

