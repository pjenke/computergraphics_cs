using System.Drawing;
using OpenTK;

namespace computergraphics
{
	/**
	 * Create a helicopter consisting of primitives.
	 * */
	public class Helicopter : InnerNode
	{
		public Helicopter (Vector3 scale, Vector3 offset)
		{

			ScaleNode scaleNode = new ScaleNode(scale);
			scaleNode.AddChild(CreateCabin(new Vector3(0, 0, 0)));
			scaleNode.AddChild(CreateBase(new Vector3(0, -0.19f, 0)));
			scaleNode.AddChild(CreateBack(new Vector3(0, 0.1f, 0.35f)));
			scaleNode.AddChild(CreateRotor(new Vector3(0, 0.2f, 0)));
			scaleNode.AddChild(CreateBackRotor(new Vector3(-0.05f, 0.1f, 0.65f)));

			TranslationNode offsetNode = new TranslationNode(offset);
			offsetNode.AddChild (scaleNode);

			//TriangleMesh dummyCubeMesh = new TriangleMesh();
			//TriangleMeshFactory.CreateCube(dummyCubeMesh, 1.0f);
			//TriangleMeshNode dummyCubeMeshNode = new TriangleMeshNode(dummyCubeMesh);
			//dummyCubeMeshNode.SetColor(Color.Aqua);
			//ScaleNode dummyCubeScale = new ScaleNode(new Vector3(0.2f, 0.2f, 0.2f));
			//dummyCubeScale.AddChild(dummyCubeMeshNode);
			//offsetNode.AddChild(dummyCubeScale);


			AddChild(offsetNode);
		}

		private INode CreateBackRotor (Vector3 offset)
		{
			INode rotor = CreateRotor (new Vector3 (0, 0, 0));
			ScaleNode scaleNode = new ScaleNode (new Vector3 (0.5f, 0.5f, 0.5f));
			scaleNode.AddChild (rotor);
			RotationNode rotationNode = new RotationNode (90.0f, new Vector3 (0, 0, 1));
			rotationNode.Animated = false;
			rotationNode.AddChild (scaleNode);
			TranslationNode offsetNode = new TranslationNode (offset);
			offsetNode.AddChild (rotationNode);
			return offsetNode;
		}

		private INode CreateBack (Vector3 offset)
		{
			INode backBar = CreateBar (new Vector3 (0.1f, 0.1f, 0.7f), Color.Red);
			TranslationNode offsetNode = new TranslationNode (offset);
			offsetNode.AddChild (backBar);
			return offsetNode;
		}

		private INode CreateBase (Vector3 offset)
		{
			INode left = CreateBar (new Vector3 (0.03f, 0.03f, 0.6f), Color.DarkGray);
			INode right = CreateBar (new Vector3 (0.03f, 0.03f, 0.6f), Color.DarkGray);
			TranslationNode translationLeft = new TranslationNode (new Vector3 (0.1f, 0, 0));
			translationLeft.AddChild (left);
			TranslationNode translationRight = new TranslationNode (new Vector3 (-0.1f, 0, 0));
			translationRight.AddChild (right);
			TranslationNode offsetNode = new TranslationNode (offset);
			offsetNode.AddChild (translationLeft);
			offsetNode.AddChild (translationRight);
			return offsetNode;
		}

		private INode CreateBar (Vector3 orientation, Color color)
		{
			ScaleNode bar1Scale = new ScaleNode (orientation);
			TriangleMesh bar1Mesh = new TriangleMesh ();
			TriangleMeshFactory.CreateCube (bar1Mesh, 1.0f);
			TriangleMeshNode bar1Node = new TriangleMeshNode (bar1Mesh);
			bar1Node.SetColor (color);
			bar1Scale.AddChild (bar1Node);
			return bar1Scale;
		}

		private INode CreateRotor (Vector3 offset)
		{
			var bar1 = CreateBar (new Vector3 (1, 0.03f, 0.03f), Color.DarkGray);
			var bar2 = CreateBar (new Vector3 (0.03f, 0.03f, 1), Color.DarkGray);
			RotationNode rotorNode = new RotationNode ();
			rotorNode.Animated = true;
			rotorNode.AddChild (bar1);
			rotorNode.AddChild (bar2);
			TranslationNode offsetNode = new TranslationNode (offset);
			offsetNode.AddChild (rotorNode);
			return offsetNode;
		}

		private INode CreateCabin (Vector3 offset)
		{
			TriangleMesh mesh = new TriangleMesh ();
			TriangleMeshFactory.CreateSphere (mesh, 0.2f, 20);
			TriangleMeshNode node = new TriangleMeshNode (mesh);

			TriangleMesh windowMesh = new TriangleMesh ();
			TriangleMeshFactory.CreateSphere (windowMesh, 0.17f, 20);
			TriangleMeshNode windowNode = new TriangleMeshNode (windowMesh);
			windowNode.SetColor (Color.LightBlue);
			TranslationNode windowTranslation = new TranslationNode (new Vector3 (0, 0.01f, -0.05f));
			windowTranslation.AddChild (windowNode);

			TranslationNode cabin = new TranslationNode (offset);
			cabin.AddChild (node);
			cabin.AddChild (windowTranslation);
			return cabin;
		}
	}
}

