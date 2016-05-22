using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Reprsents a 3D scene.
	 * */
	public class Scene
	{
		/**
		 * Shader (fragment + vertex)
		 * */
		Shader shader;

		/**
		 * Virtual camera 
		 */
		Camera camera;

		/**
		 * Scene graph root node
		 * */
		GroupNode rootNode = new GroupNode ();



		public Scene ()
		{
			camera = new Camera ();
			shader = new Shader ("../../../../assets/shader/fragment_shader_phong.glsl", "../../../../assets/shader/vertex_shader_phong.glsl");

//			Triangle mesh = new TriangleMesh ();
//			ObjReader reader = new ObjReader ();
//			reader.read (mesh, "../../../../assets/meshes/cube.obj");
//			mesh.Tex = new Texture("../../../../assets/textures/lego.png");
//			TriangleMeshNode meshNode = new TriangleMeshNode (mesh);
//			getRootNode ().Add (meshNode);


			TranslationNode cubeTranslation = new TranslationNode(new Vector3(0,0,1));
			CubeNode cubeNode = new CubeNode ();
			cubeTranslation.Add (cubeNode);
			getRootNode ().Add (cubeTranslation);

			TranslationNode sphereTranslation = new TranslationNode(new Vector3(1,0,0));
			SphereNode sphereNode = new SphereNode (0.5f, 20);
			sphereTranslation.Add (sphereNode);
			getRootNode ().Add (sphereTranslation);
		}

		/**
		 * Called once when OpenGL is ready.
		 * */
		public void Init ()
		{
			// Setup shader
			shader.CompileAndLink ();
			shader.Use ();
		}

		/**
		 * Called for each frame redraw
		 * */
		public void Draw ()
		{
			shader.SetCameraEyeShaderParameter (camera.Eye);
			shader.SetUseTextureParameter (true);
			rootNode.Draw ();
		}

		/**
		 * Event callback: window resize.
		 * */
		public void Resize (int width, int height)
		{
			camera.AspectRatio = (float)width / (float)height;
		}

		/**
		 * Set the current projectio matrix.
		 * TODO: Only apply after change.
		 * */
		public void SetProjectionMatrix ()
		{
			GL.MatrixMode (MatrixMode.Projection);
			GL.LoadIdentity ();
			Matrix4 P = Matrix4.CreatePerspectiveFieldOfView (camera.getFovy (), camera.AspectRatio, camera.getZNear (), camera.getZFar ());
			GL.LoadMatrix (ref P);
		}

		/**
		 * Set the current model view matrix.
		 * TODO: Only apply after change.
		 * */
		public void SetModelViewMatrix ()
		{
			GL.MatrixMode (MatrixMode.Modelview);
			Matrix4 LookAt = camera.getLookAtMatrix ();
			GL.LoadMatrix (ref LookAt);
		}

		/**
		 * Rotate the camera left <-> right and up <-> down.
		 * */
		public void RotateCamera (float angleAroundUp, float angleUpDown)
		{
			camera.updateLookAtMatrix (angleAroundUp, angleUpDown);
		}

		public void Zoom(int factor)
		{
			camera.Zoom(factor);
		}

		public GroupNode getRootNode()
		{
			return rootNode;
		}
	}
}

