using System;
using System.Collections.Generic;
using System.Timers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

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
		private Shader shader;

		/**
		 * Virtual camera 
		 */
		private Camera camera;

		/**
		 * Scene graph root node
		 * */
		private RootNode rootNode;

		/**
		 * Timer to create animation events
		 * */
		private Timer timer;

		/**
		 * Current timer value.
		 * */
		private int timerCounter = 0;

		/**
		 * This flag indicates that a timer tick needs to be done.
		 * */
		private bool timerUpdate = false;

		/**
		 * Projection matrix needs to be rebuilt.
		 * */
		private bool projectionMatrixUpdate = true;

		/**
		 * Model-view matrix needs to be rebuilt.
		 * */
		private bool modelviewMatrixUpdate = true;

		/**
		 * Current render mode.
		 * */
		private RenderMode currentRenderMode;

		/**
		 * Render mode propertiy
		 * */
		public RenderMode CurrentRenderMode
		{
			get { return currentRenderMode; }
			set { currentRenderMode = value; }
		}

		/**
		 * Mouse event button
		 * */
		public enum MouseButton
		{
			LEFT, MIDDLE, RIGHT, NONE
		}

		/**
		 * Remember last mouse position.
		 * */
		private Nullable<Vector2> lastMousePosition = null;

		/**
		 * Rememember last mouse button 
		 * .
		 * */
		private MouseButton lastMouseButton = MouseButton.NONE;

		public Scene(int timerTimeout, Shader.ShaderMode shaderMode) : this(timerTimeout, shaderMode, RenderMode.REGULAR)
		{
		}

		public Scene(int timerTimeout, Shader.ShaderMode mode, RenderMode renderMode)
		{
			CurrentRenderMode = renderMode;
			camera = new Camera();
			shader = new Shader(mode);
			rootNode = new RootNode(shader, camera);
			timer = new Timer(timerTimeout);
			timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
		}

		/**
		 * Called once when OpenGL is ready.
		 * */
		public void Init()
		{
			// Stencil test
			GL.Enable(EnableCap.StencilTest);

			// Depth test
			GL.Enable(EnableCap.DepthTest);

			// Blending
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			// Culling
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.FrontFace(FrontFaceDirection.Ccw);

			Shader.CheckError();

			// OpenGL-Information
			string version_string = GL.GetString(StringName.Version);
			Console.WriteLine("OpenGL-Version: " + version_string);
			Shader.CheckError(); 
			Console.WriteLine("Stencil buffer bits: " + GL.GetInteger(GetPName.StencilBits));
			Shader.CheckError(); 
			Console.WriteLine("Depth buffer bits: " + GL.GetInteger(GetPName.DepthBits));

			Shader.CheckError();

			// For OpenGL 2.1 and lower
			Dictionary<string, bool> extensions =
				new Dictionary<string, bool>();
			string extension_string = GL.GetString(StringName.Extensions);
			foreach (string extension in extension_string.Split(' '))
			{
				extensions.Add(extension, true);
			}

			GL.ClearColor(rootNode.BackgroundColor.X,
						  rootNode.BackgroundColor.Y,
				rootNode.BackgroundColor.Z, 1);

			// Setup shader
			shader.CompileAndLink();
			shader.Use();
			ShaderAttributes.GetInstance().GetAttributes(shader.ProgramId);

			// Start timer
			timer.Start();
		}

		/**
		 * Event callback: window resize.
		 * */
		public void Resize(int width, int height)
		{
			camera.AspectRatio = (float)width / height;
			projectionMatrixUpdate = true;
		}

		public void Redraw()
		{
			// Clear
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Shader.CheckError();

			// Render scene
			SetProjectionMatrix();

			Shader.CheckError();

			SetModelViewMatrix();

			Shader.CheckError();

			//Console.WriteLine("Rendering");
			if (timerUpdate && rootNode.Animated)
			{
				TimerTick(timerCounter);
				rootNode.TimerTick(timerCounter);
				timerUpdate = false;
			}

			if (CurrentRenderMode == RenderMode.REGULAR)
			{
				DrawRegular();
			}
			else if (currentRenderMode == RenderMode.SHADOW_VOLUME)
			{
				DrawShadowVolume();
			}
			else if (currentRenderMode == RenderMode.DEBUG_SHADOW_VOLUME)
			{
				DrawDebugShadowVolume();
			}
			else {
				DrawRegular();
			}
		}

		/**
		 * Timer event handler.
		 * */
		public virtual void TimerTick(int counter)
		{
		}

		public virtual void KeyPressed(Key key)
		{
		}

		/**
		 * Draw the scene graph content - regular mode.
		 * */
		private void DrawRegular()
		{
			ShaderAttributes.GetInstance().SetCameraEyeParameter(camera.Eye);
			ShaderAttributes.GetInstance().SetShaderModeParameter(GetRoot().Shader.Mode);
			ShaderAttributes.GetInstance().SetLightPositionParameter(rootNode.LightPosition);

			// No change in stencil buffer
			GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
			// Draw if stencil value is > 0
			GL.StencilFunc(StencilFunction.Always, 0, 255);

			rootNode.Traverse(RenderMode.REGULAR, Matrix4.Identity);
		}

		private void DrawDebugShadowVolume()
		{
			ShaderAttributes.GetInstance().SetCameraEyeParameter(camera.Eye);
			ShaderAttributes.GetInstance().SetShaderModeParameter(GetRoot().Shader.Mode);
			ShaderAttributes.GetInstance().SetLightPositionParameter(rootNode.LightPosition);

			// No change in stencil buffer
			GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
			// Draw if stencil value is > 0
			GL.StencilFunc(StencilFunction.Always, 0, 255);

			rootNode.Traverse(RenderMode.REGULAR, Matrix4.Identity);
			rootNode.Traverse(RenderMode.DEBUG_SHADOW_VOLUME, Matrix4.Identity);
		}

		private void DrawShadowVolume()
		{
			GL.ClearStencil(0);
			GL.Clear(ClearBufferMask.StencilBufferBit);

			// ************* ORIGINAL SCENE - NO LIGHTING *************

			// Stencil buffer never changed
			GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
			// Stencil test always passed
			GL.StencilFunc(StencilFunction.Always, 0, 255);

			// Setup shader
			shader.Mode = Shader.ShaderMode.AMBIENT_ONLY;
			ShaderAttributes.GetInstance().SetCameraEyeParameter(camera.Eye);
			ShaderAttributes.GetInstance().SetShaderModeParameter(GetRoot().Shader.Mode);
			ShaderAttributes.GetInstance().SetLightPositionParameter(rootNode.LightPosition);

			// Render scene w/o lighting
			rootNode.Traverse(RenderMode.REGULAR, Matrix4.Identity);

			// ************* SHADOW VOLUMS BACK FACES *************

			// Disable writes to the depth and color buffers.
			GL.DepthMask(false);
			GL.ColorMask(false, false, false, false);

			// Use back-face culling.
			GL.CullFace(CullFaceMode.Back);

			// Stencil buffer always inc
			GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Incr);
			// Stencil test always passed
			GL.StencilFunc(StencilFunction.Always, 0, 255);

			// Render the shadow volumes (because of culling, only their front faces are rendered).
			rootNode.Traverse(RenderMode.SHADOW_VOLUME, Matrix4.Identity);

			// ************* SHADOW VOLUMS FRONT FACES *************

			//			GL.Begin (PrimitiveType.Quads);
			//			GL.Vertex3 (-0.5f, 0.5, -0.5f);
			//			GL.Vertex3 (-0.5f, 0.5, 0.5f);
			//			GL.Vertex3 (0.5f, 0.5, 0.5f);
			//			GL.Vertex3 (0.5f, 0.5, -0.5f);
			//			GL.End ();
			//
			// Use front-face culling.
			GL.CullFace(CullFaceMode.Front);

			// Set the stencil operation to decrement on depth pass.
			GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Decr);

			// Render the shadow volumes (only their back faces are rendered).
			rootNode.Traverse(RenderMode.SHADOW_VOLUME, Matrix4.Identity);


			// ************* ORIGINAL SCENE *************

			// No change in stencil buffer
			GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
			// Draw if stencil value is > 0
			GL.StencilFunc(StencilFunction.Equal, 0, 255);

			GL.DepthMask(true);
			GL.ColorMask(true, true, true, true);
			GL.CullFace(CullFaceMode.Back);
			GL.Clear(ClearBufferMask.DepthBufferBit);
			shader.Mode = Shader.ShaderMode.PHONG;
			ShaderAttributes.GetInstance().SetShaderModeParameter(GetRoot().Shader.Mode);
			rootNode.Traverse(RenderMode.REGULAR, Matrix4.Identity);
		}

		/**
		 * Timer event handler.
		 * */
		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			timerUpdate = true;
			timerCounter++;
		}

		/**
		 * Set the current projectio matrix.
		 * */
		public void SetProjectionMatrix()
		{
			if (projectionMatrixUpdate)
			{
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				Matrix4 P = Matrix4.CreatePerspectiveFieldOfView(camera.GetFovy(), camera.AspectRatio, camera.GetZNear(), camera.GetZFar());
				GL.LoadMatrix(ref P);
				projectionMatrixUpdate = false;
			}
		}

		/**
		 * Set the current model view matrix.
		 * */
		public void SetModelViewMatrix()
		{
			if (modelviewMatrixUpdate)
			{
				GL.MatrixMode(MatrixMode.Modelview);
				Matrix4 LookAt = camera.GetViewMatrix();
				GL.LoadMatrix(ref LookAt);
				modelviewMatrixUpdate = false;
			}
		}

		/**
		 * Rotate the camera left <-> right and up <-> down.
		 * */
		public void RotateCamera(float angleAroundUp, float angleUpDown)
		{
			camera.UpdateLookAtMatrix(angleAroundUp, angleUpDown);
			modelviewMatrixUpdate = true;
		}

		public void Zoom(int factor)
		{
			camera.Zoom(factor);
			modelviewMatrixUpdate = true;
		}

		public RootNode GetRoot()
		{
			return rootNode;
		}

		/**
		 * Handle mouse event (mainly control camera).
		 * */
		public void HandleMouseEvent(MouseButton button, int x, int y)
		{
			if (button == MouseButton.LEFT && lastMouseButton == MouseButton.LEFT && lastMousePosition != null)
			{
				var deltaX = (x - lastMousePosition.Value.X) / 200.0f;
				var deltaY = (y - lastMousePosition.Value.Y) / 200.0f;
				RotateCamera(deltaX, deltaY);
			}
			else if (button == MouseButton.MIDDLE && lastMouseButton == MouseButton.MIDDLE && lastMousePosition != null)
			{
				int deltaY = (int)(y - lastMousePosition.Value.Y);
				Zoom(deltaY);
			}
			lastMouseButton = button;
			lastMousePosition = new Vector2(x, y);
		}

		public void HandleKeyEvent(Key key)
		{
			switch (key)
			{
				case Key.I:
					camera.Zoom(40);
					break;
				case Key.O:
					camera.Zoom(-40);
					break;
				case Key.A:
					rootNode.Animated = !rootNode.Animated;
					Console.WriteLine("Animated: " + rootNode.Animated);
					break;
				case Key.R:
					CurrentRenderMode = RenderMode.REGULAR;
					Console.WriteLine("Switched to render mode 'rehular'.");
					break;

				case Key.S:
					CurrentRenderMode = RenderMode.SHADOW_VOLUME;
					Console.WriteLine("Switched to render mode 'shadow volumes'.");
					break;

				case Key.D:
					CurrentRenderMode = RenderMode.DEBUG_SHADOW_VOLUME;
					Console.WriteLine("Switched to render mode 'debug shadow volumes'.");
					break;
			}
			KeyPressed(key);
		}
	}
}

