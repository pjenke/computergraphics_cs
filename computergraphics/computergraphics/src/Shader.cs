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
		string vertexShaderFilename;

		/**
		 * Name of file with fragment shader code
		 * */
		string fragmentShaderFilename;

		bool useTexture = false;

		public bool UseTexture {
			set { useTexture = value; }
			get { return useTexture; }
		}

		/**
		 * Id of the created shader program, -1 before initialization.
		 * */
		int shaderProgramId = -1;

		public Shader (string fragmentShaderFilename, string vertexShaderFilename)
		{
			this.fragmentShaderFilename = fragmentShaderFilename;
			this.vertexShaderFilename = vertexShaderFilename;
		}

		/**
		 * Compile and link shader (should be called only once).
		 * */
		public void CompileAndLink ()
		{
			int vertexShaderId = CompileShader (fragmentShaderFilename, ShaderType.FragmentShader);
			int fragmentShaderId = CompileShader (vertexShaderFilename, ShaderType.VertexShader);
			shaderProgramId = LinkProgram (vertexShaderId, fragmentShaderId);
			Console.WriteLine ("Shader program created (id=" + shaderProgramId + ")");
		}

		/**
		 * Apply shader program as active in GL context.
		 * */
		public void Use ()
		{
			GL.UseProgram (shaderProgramId);
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
			return System.IO.File.ReadAllText (filename);
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
			return shaderProgram;
		}

		public void SetCameraEyeShaderParameter(Vector3 eye)
		{
			int location = GL.GetUniformLocation(shaderProgramId, "camera_position");
			float[] values = { (float)eye.X, (float)eye.Y, (float)eye.Z };
			GL.ProgramUniform3 (shaderProgramId, location, 3, values);
		}

		public void SetUseTextureParameter(bool useTexture)
		{
			int location = GL.GetUniformLocation(shaderProgramId, "useTexture");
			float[] values = { -1, -1, -1 };
			if (useTexture) {
				values[0] = 1;	
			}
			GL.ProgramUniform3(shaderProgramId, location, 3, values);
		}
	}
}

