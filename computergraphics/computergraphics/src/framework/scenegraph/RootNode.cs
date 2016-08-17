using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Root node in the scene graph. Contains information about the scene.
	 * */
	public class RootNode : InnerNode
	{
		/**
		 * Currently used shader
		 * */
		private Shader shader;

		/**
		 * Shader property
		 * */
		public Shader Shader
		{
			get { return shader; }
		}

		/**
		 * Scene camera
		 * */
		private Camera camera;

		/**
		 * Camera property
		 * */
		public Camera Camera
		{
			get { return camera; }
		}

		/**
		 * This flags indicates that the scene should be animated*/
		private bool animated;

		/**
		 * Animated property.
		 * */
		public bool Animated
		{
			get { return animated; }
			set { animated = value; }
		}

		/**
		 * Position of the light source
		 * */
		private Vector3 lightPosition;

		/**
		 * Light position property
		 * */
		public Vector3 LightPosition
		{
			get { return lightPosition; }
			set { lightPosition = value; }
		}

		/**
   		* Background color
   		*/
		private Vector3 backGroundColor;

		/**
		 * Background color property
		 * */
		public Vector3 BackgroundColor
		{
			get { return backGroundColor; }
			set { backGroundColor = value; }
		}

		public RootNode(Shader shader, Camera camera)
		{
			this.shader = shader;
			this.camera = camera;
			animated = true;
			lightPosition = new Vector3(1, 1, 0);
			backGroundColor = new Vector3(0.25f, 0.25f, 0.25f);
		}

		public override RootNode GetRootNode()
		{
			return this;
		}
	}
}

