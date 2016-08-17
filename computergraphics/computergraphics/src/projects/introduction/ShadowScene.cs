using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace computergraphics
{
	public class ShadowScene : Scene
	{

		int numberOfTrees = 20;
		float treeSize = 0.15f;
		int sphereResolution = 20;

		/**
		 * Rotation angle used for animations.
		 * */
		private float alpha = 0;

		private TranslationNode lightNode = new TranslationNode(new Vector3(0, 0, 0));

		private Random random = new Random();

		private Vector3 baseLightPosition = new Vector3(0, 0, -0.4f);

		// Base constructor: Timer timeout between two TimerTick events and shader mode (PHONG, TEXTURE, NO_LIGHTING)
		public ShadowScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.SHADOW_VOLUME)
		{

			//CreateCubeRoomScene();
			//CreateTriangleMeshScene();
			CreateHelicopterForestScene();
			//CreateSimpleScene();

			//Light
			TriangleMesh lightMesh = new TriangleMesh();
			TriangleMeshFactory.CreateSphere(lightMesh, 0.02f, 10);
			TriangleMeshNode lightMeshNode = new TriangleMeshNode(lightMesh);
			lightMeshNode.SetColor(Color.Yellow);
			lightMeshNode.CastsShadow = false;
			lightNode.AddChild(lightMeshNode);
			lightNode.Translation = GetRoot().LightPosition;
			GetRoot().AddChild(lightNode);
		}

		private void CreateSimpleScene()
		{
			// Plane
			TriangleMesh planeMesh = new TriangleMesh();
			TriangleMeshFactory.CreateSquare(planeMesh, 2);
			TriangleMeshNode planeMeshNode = new TriangleMeshNode(planeMesh);
			planeMeshNode.SetColor(Color.Green);
			GetRoot().AddChild(planeMeshNode);

			// Sphere
			TriangleMesh mesh = new TriangleMesh();
			TriangleMeshFactory.CreateSphere(mesh, 0.25f, 10);
			TriangleMeshNode meshNode = new TriangleMeshNode(mesh);
			meshNode.SetColor(Color.Red);
			TranslationNode translationNode =
				new TranslationNode(new Vector3(0.5f, 0.5f, 0.5f));
			translationNode.AddChild(meshNode);
			GetRoot().AddChild(translationNode);

			GetRoot().LightPosition = new Vector3(0.25f, 1, 0.25f);
			GetRoot().Animated = false;
		}

		private void CreateCubeRoomScene()
		{
			baseLightPosition = new Vector3(0, 0, -0.4f);

			ObjReader reader = new ObjReader();
			TriangleMesh roomMesh = new TriangleMesh();
			reader.Read("meshes/inverse_cube.obj", roomMesh);
			TriangleMeshNode roomNode = new TriangleMeshNode(roomMesh);
			roomNode.SetColor(Color.Khaki);
			roomNode.ShowNormals = false;
			ScaleNode roomScale = new ScaleNode(new Vector3(1, 1, 1));
			roomScale.AddChild(roomNode);
			GetRoot().AddChild(roomScale);
			int numberOfObjects = 5;
			for (int i = 0; i < numberOfObjects; i++)
			{
				INode objectNode = CreateObject();
				GetRoot().AddChild(objectNode);
			}

			GetRoot().LightPosition = baseLightPosition;
			GetRoot().Animated = false;
		}

		private Color4 CreateColor()
		{
			return new Color4((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1);
		}

		private INode CreateObject()
		{
			InnerNode objectNode = new InnerNode();

			TriangleMesh mesh = new TriangleMesh();
			double randomValue = random.NextDouble();
			if (randomValue < 1 / 3.0)
			{
				TriangleMeshFactory.CreateSphere(mesh, 0.25f, 10);
			}
			else if (randomValue < 2 / 3.0)
			{
				TriangleMeshFactory.CreateCube(mesh, 0.25f);
			}
			else {
				ObjReader reader = new ObjReader();
				reader.Read("meshes/cow.obj", mesh);
			}

			TriangleMeshNode meshNode = new TriangleMeshNode(mesh);
			meshNode.SetColor(CreateColor());
			ScaleNode scaleNode = new ScaleNode(new Vector3(0.4f, 0.4f, 0.4f));
			scaleNode.AddChild(meshNode);
			objectNode.AddChild(scaleNode);

			float posMin = -0.4f;
			Vector3 t = new Vector3((float)(random.NextDouble() * posMin * 2 - posMin),
							(float)(random.NextDouble() * posMin * 2 - posMin),
									(float)(random.NextDouble() * posMin * 2 - posMin));
			TranslationNode translationNode = new TranslationNode(t);
			translationNode.AddChild(objectNode);

			Vector3 axis = new Vector3((float)(random.NextDouble()) + 0.2f,
									   (float)(random.NextDouble()) + 0.2f,
									   (float)(random.NextDouble()) + 0.2f).Normalized();
			RotationNode rotationNode = new RotationNode(0, axis);
			rotationNode.Animated = true;
			rotationNode.AnimationSpeed = 0.01f;
			rotationNode.AddChild(translationNode);

			Console.WriteLine("Created object: " + t);

			return rotationNode;
		}

		private void CreateHelicopterForestScene()
		{
			baseLightPosition = new Vector3(0.3f, 1, 0.3f);

			// Tree scene
			for (int i = 0; i < numberOfTrees; i++)
			{
				if (numberOfTrees == 1)
				{
					// Debugging: single tree
					GetRoot().AddChild(CreateTree(new Vector3(0, 0, 0)));
				}
				else {
					GetRoot().AddChild(CreateTree(new Vector3((float)(random.NextDouble() * 2 - 1), 0, (float)(random.NextDouble() * 2 - 1))));
				}
			}
			GetRoot().AddChild(CreatePlane());

			// Helicopter
			INode helicopter = new Helicopter(new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0.7f, 0.4f, 0));
			RotationNode heliRotation = new RotationNode(0, new Vector3(0, 1, 0));
			heliRotation.Animated = true;
			heliRotation.AnimationSpeed = 0.005f;
			heliRotation.AddChild(helicopter);
			GetRoot().AddChild(heliRotation);

			GetRoot().LightPosition = baseLightPosition;
			GetRoot().Animated = false;
		}

		public INode CreateTree(Vector3 position)
		{
			TriangleMesh sphereMesh = new TriangleMesh();
			TriangleMeshFactory.CreateSphere(sphereMesh, 1.0f, sphereResolution);
			TriangleMeshNode sphereNode = new TriangleMeshNode(sphereMesh);
			sphereNode.ShowNormals = false;
			sphereNode.SetColor(Color.DarkGreen);
			ScaleNode sphereScale = new ScaleNode(new Vector3(treeSize / 1.5f, treeSize / 1.5f, treeSize / 1.5f));
			sphereScale.AddChild(sphereNode);
			TranslationNode sphereTranslation = new TranslationNode(new Vector3(0, 1.5f * treeSize, 0));
			sphereTranslation.AddChild(sphereScale);

			TriangleMesh cubeMesh = new TriangleMesh();
			TriangleMeshFactory.CreateCube(cubeMesh, 1.0f);
			TriangleMeshNode cubeNode = new TriangleMeshNode(cubeMesh);
			cubeNode.SetColor(Color.Brown);
			ScaleNode cubeScale = new ScaleNode(new Vector3(treeSize / 3.0f, treeSize, treeSize / 3.0f));
			cubeScale.AddChild(cubeNode);
			TranslationNode cubeTranslation = new TranslationNode(new Vector3(0, treeSize / 2.0f, 0));
			cubeTranslation.AddChild(cubeScale);

			TranslationNode treeNode = new TranslationNode(position);
			treeNode.AddChild(sphereTranslation);
			treeNode.AddChild(cubeTranslation);
			return treeNode;
		}

		public INode CreatePlane()
		{
			TriangleMesh mesh = new TriangleMesh();
			TriangleMeshFactory.CreateCube(mesh,1.0f);
			TriangleMeshNode node = new TriangleMeshNode(mesh);
			node.SetColor(Color.DarkGreen);
			ScaleNode scaleNode = new ScaleNode(new Vector3(3, 0.02f, 3));
			scaleNode.AddChild(node);
			return scaleNode;
		}

		public override void TimerTick(int counter)
		{
			if (GetRoot().Animated)
			{
				base.TimerTick(counter);
				alpha += 0.05f;
				GetRoot().LightPosition = MathHelper.Multiply(Matrix3.CreateRotationY(alpha), baseLightPosition);
				lightNode.Translation = GetRoot().LightPosition;
			}
		}

		public override void KeyPressed(Key key)
		{
		}
	}
}

