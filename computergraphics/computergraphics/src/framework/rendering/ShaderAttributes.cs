using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Communiction netween the CPU-based software the the GPU shaders.
	 * */
	public class ShaderAttributes
	{
		/**
		 * Uniform parameter location in the shader program.
		 * */
		private int locationCameraPosition = -1;

		/**
		 * Uniform parameter location in the shader program.
		 * */
		private int locationShaderMode = -1;

		/**
		 * Uniform parameter location in the shader program.
		 * */
		private int locationLightPosition = -1;

		/**
		 * Uniform parameter location in the shader program.
		 * */
		private int locationModelMatrix = -1;

		/**
		 * Uniform parameter location in the shader program.
		 * */
		private int locationViewMatrix = -1;

		/**
		 * Attribute position
		 * */
		private int locationVertex = -1;

		/**
		 * Attribute normal
		 * */
		private int locationNormal = -1;

		/**
		 * Attribute color
		 * */
		private int locationColor = -1;


		public int LocationPosition
		{
			get
			{
				if (locationVertex < 0)
				{
					Console.WriteLine("Invalid position attribute location.");
				}
				return locationVertex;
			}
		}

		public int LocationNormal
		{
			get
			{
				if (locationNormal < 0)
				{
					Console.WriteLine("Invalid normal attribute location.");
				}
				return locationNormal;
			}
		}

		public int LocationColor
		{
			get
			{
				if (locationColor < 0)
				{
					Console.WriteLine("Invalid color attribute location.");
				}
				return locationColor;
			}
		}

		/**
   		 * Singleton instance
   		 */
		private static ShaderAttributes instance = null;

		private ShaderAttributes()
		{
		}

		/**
		 * Getter for the singleton instance.
		*/
		public static ShaderAttributes GetInstance()
		{
			if (instance == null)
			{
				instance = new ShaderAttributes();
			}
			return instance;
		}

		public void GetAttributes(int shaderProgramId)
		{
			GetAttributeLocation(shaderProgramId, out locationVertex, "inVertex");
			GetAttributeLocation(shaderProgramId, out locationNormal, "inNormal");
			GetAttributeLocation(shaderProgramId, out locationColor, "inColor");
			GetUniformLocation(shaderProgramId, out locationCameraPosition, "camera_position");
			GetUniformLocation(shaderProgramId, out locationShaderMode, "shaderMode");
			GetUniformLocation(shaderProgramId, out locationLightPosition, "lightPosition");
			GetUniformLocation(shaderProgramId, out locationModelMatrix, "modelMatrix");
			GetUniformLocation(shaderProgramId, out locationViewMatrix, "viewMatrix");
		}

		/**
		 * Request the location of a given attribute
		*/
		private void GetAttributeLocation(int shaderProgramId, out int location, String attributeName)
		{
			if (shaderProgramId < 0)
			{
				Console.WriteLine("Invalid shader program.");
				location = -1;
				return;
			}
			location = GL.GetAttribLocation(shaderProgramId, attributeName);
			if (location < 0)
			{
				Console.WriteLine("Failed to request location");
			}
		}

		/**
		 * Request the location of a given attribute
		*/
		private void GetUniformLocation(int shaderProgramId, out int location, String attributeName)
		{
			if (shaderProgramId < 0)
			{
				Console.WriteLine("Invalid shader program.");
				location = -1;
				return;
			}
			location = GL.GetUniformLocation(shaderProgramId, attributeName);
			if (location < 0)
			{
				Console.WriteLine("Failed to request location");
			}
		}

		public void SetCameraEyeParameter(Vector3 eye)
		{
			if (locationCameraPosition >= 0)
			{
				GL.Uniform3(locationCameraPosition, (float)eye.X, (float)eye.Y, (float)eye.Z);
				Shader.CheckError();
			}
		}

		public void SetLightPositionParameter(Vector3 lightPosition)
		{
			if (locationLightPosition >= 0)
			{
				GL.Uniform3(locationLightPosition, (float)lightPosition.X, (float)lightPosition.Y, (float)lightPosition.Z);
				Shader.CheckError();
			}
		}

		public void SetModelMatrixParameter(Matrix4 modelMatrix)
		{
			if (locationModelMatrix >= 0)
			{
				GL.UniformMatrix4(locationModelMatrix, false, ref modelMatrix);
				//System.Console.WriteLine("model: " + modelMatrix);
				Shader.CheckError();
			}
		}

		public void SetViewMatrixParameter(Matrix4 viewMatrix)
		{
			if (locationViewMatrix >= 0)
			{
				GL.UniformMatrix4(locationViewMatrix, false, ref viewMatrix);
				//System.Console.WriteLine("view: " + viewMatrix);
				Shader.CheckError();
			}
		}

		public void SetShaderModeParameter(Shader.ShaderMode mode)
		{
			if (locationShaderMode < 0)
			{
				return;
			}
			int value = 0;
			switch (mode)
			{
				case Shader.ShaderMode.PHONG:
					value = 0;
					break;
				case Shader.ShaderMode.TEXTURE:
					value = 1;
					break;
				case Shader.ShaderMode.NO_LIGHTING:
					value = 2;
					break;
				case Shader.ShaderMode.AMBIENT_ONLY:
					value = 3;
					break;
			}
			GL.Uniform1(locationShaderMode, value);
			Shader.CheckError();

		}
	}
}

