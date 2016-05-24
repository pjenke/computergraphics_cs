using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Represents a OpenGL fragment + vertex shader.
	 * */
	public class Shader
	{
		public enum ShaderMode
		{
			PHONG,
			TEXTURE,
			NO_LIGHTING
		}

		/**
		 * Name of file with vertex shader code
		 * */
		private string vertexShaderFilename;

		/**
		 * Name of file with fragment shader code
		 * */
		private string fragmentShaderFilename;

		/**
		 * Uniform parameter location in the shader program.
		 * */
		private int locationCameraPosition = -1;

		/**
		 * Uniform parameter location in the shader program.
		 * */
		private int locationShaderMode = -1;

		/**
		 * Current shader mode.
		 * */
		private ShaderMode mode;

		/**
		 * useTexture-Property
		 * */
		public ShaderMode Mode {
			set { mode = value; }
			get { return mode; }
		}

		/**
		 * Id of the created shader program, -1 before initialization.
		 * */
		int shaderProgramId = -1;

		public Shader (ShaderMode mode)
		{
			this.mode = mode;
			this.fragmentShaderFilename = "shader/fragment_shader.glsl";
			this.vertexShaderFilename = "shader/vertex_shader.glsl";
		}

		/**
		 * Compile and link shader (should be called only once).
		 * */
		public void CompileAndLink ()
		{
			int fragmentShaderId = CompileShader (fragmentShaderFilename, ShaderType.FragmentShader);
			int vertexShaderId = CompileShader (vertexShaderFilename, ShaderType.VertexShader);
			shaderProgramId = LinkProgram (vertexShaderId, fragmentShaderId);
			Console.WriteLine ("Created Shader program from vertex shader " + vertexShaderFilename + " and fragment shader " + fragmentShaderFilename);
		}

		/**
		 * Apply shader program as active in GL context.
		 * */
		public void Use ()
		{
			GL.UseProgram (shaderProgramId);
			locationCameraPosition = GL.GetUniformLocation (shaderProgramId, "camera_position");
			locationShaderMode = GL.GetUniformLocation (shaderProgramId, "shaderMode");
			CheckError ();
		}

		/**
		 * Read source from file and compile
		 * */
		private int CompileShader (string shaderFilename, ShaderType shaderType)
		{
			string shaderSource = ReadShaderSource (shaderFilename);
			int id = CompileShaderFromSource (shaderType, shaderSource);
			return id;
		}

		/**
		 * Compile source, check for errors.
		 * */
		private int CompileShaderFromSource (ShaderType shaderType, string shaderSource)
		{
			int id = GL.CreateShader (shaderType);
			GL.ShaderSource (id, shaderSource);
			GL.CompileShader (id);
			int status_code = -1;
			string info = "";

			// Check error
			GL.GetShaderInfoLog (id, out info);
			GL.GetShader (id, ShaderParameter.CompileStatus, out status_code);
			if (status_code != 1) {
				Console.WriteLine ("Failed to Compile " + shaderType + " source." +
				Environment.NewLine + info + Environment.NewLine + "Status Code: " + status_code.ToString ());
				return -1;
			}

			return id;
		}

		/**
		 * Read source from text file.
		 * */
		private string ReadShaderSource (string filename)
		{
			return System.IO.File.ReadAllText (AssetPath.getPathToAsset (filename));
		}

		/**
		 * Link the compiled shaders to program.
		 * */
		private int LinkProgram (int vertexShaderId, int fragmentShaderId)
		{
			int shaderProgram = GL.CreateProgram ();
			GL.AttachShader (shaderProgram, vertexShaderId);
			GL.AttachShader (shaderProgram, fragmentShaderId);
			GL.LinkProgram (shaderProgram);
			GL.ValidateProgram (shaderProgram);
			CheckError ();
			return shaderProgram;
		}

		public void SetCameraEyeShaderParameter (Vector3 eye)
		{
			if (locationCameraPosition >= 0) {
				GL.Uniform3 (locationCameraPosition, (float)eye.X, (float)eye.Y, (float)eye.Z);
				CheckError ();
			}
		}

		public void SetUseTextureParameter ()
		{
			if (locationShaderMode < 0) {
				return; 
			}
			int value = 0;
			switch (mode) {
			case ShaderMode.PHONG: 
				value = 0;
				break;
			case ShaderMode.TEXTURE: 
				value = 1;
				break;
			case ShaderMode.NO_LIGHTING: 
				value = 2;
				break;

			}
			GL.Uniform1 (locationShaderMode, value);
			CheckError ();

		}

		private void CheckError ()
		{
			ErrorCode ec = GL.GetError ();
			if (ec != 0) {
				throw new System.Exception (ec.ToString ());
			}
		}
	}
}

