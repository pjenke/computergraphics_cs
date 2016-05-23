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
		/**
		 * Name of file with vertex shader code
		 * */
		private string vertexShaderFilename;

		/**
		 * Name of file with fragment shader code
		 * */
		private string fragmentShaderFilename;

		private bool useTexture = false;

		/**
		 * Uniform parameter location in the shader program.
		 * */
		private int locationCameraPosition = -1;

		/**
		 * Uniform parameter location in the shader program.
		 * */
		private int locationUseTexture = -1;

		/**
		 * useTexture-Property
		 * */
		public bool UseTexture {
			set { useTexture = value; }
			get { return useTexture; }
		}

		/**
		 * Id of the created shader program, -1 before initialization.
		 * */
		int shaderProgramId = -1;

		public Shader (bool useTexture)
		{
			this.fragmentShaderFilename = "shader/fragment_shader.glsl";
			this.vertexShaderFilename = "shader/vertex_shader.glsl";
			this.useTexture = useTexture;
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
			locationUseTexture = GL.GetUniformLocation (shaderProgramId, "useTexture");
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
			if (locationUseTexture >= 0) {
				int value = -1;
				if (useTexture) {
					value = 1;	
				}
				GL.Uniform1 (locationUseTexture, value);
				CheckError ();
			}
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

