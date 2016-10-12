using System;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Represents a OpenGL fragment + vertex shader.
	 * */
	public class Shader
	{
		/**
		 * Predefined shader bahaviour
		 * */
		public enum ShaderMode
		{
			PHONG,
			TEXTURE,
			NO_LIGHTING,
			AMBIENT_ONLY
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
		 * Current shader mode.
		 * */
		private ShaderMode mode;

		/**
		 * mode property
		 * */
		public ShaderMode Mode
		{
			set { mode = value; }
			get { return mode; }
		}

		/**
		 * Id of the created shader program, -1 before initialization.
		 * */
		int programId = -1;

		public int ProgramId
		{
			get { return programId; }
		}

		public Shader(ShaderMode mode)
		{
			this.mode = mode;
			this.fragmentShaderFilename = "shader/fragment_shader.glsl";
			this.vertexShaderFilename = "shader/vertex_shader.glsl";
		}

		/**
		 * Compile and link shader (should be called only once).
		 * */
		public void CompileAndLink()
		{
			int fragmentShaderId = CompileShader(fragmentShaderFilename, ShaderType.FragmentShader);
			int vertexShaderId = CompileShader(vertexShaderFilename, ShaderType.VertexShader);
			programId = LinkProgram(vertexShaderId, fragmentShaderId);
			Console.WriteLine("Created Shader program from vertex shader " + vertexShaderFilename + " and fragment shader " + fragmentShaderFilename);
		}

		/**
		 * Apply shader program as active in GL context.
		 * */
		public void Use()
		{
			GL.UseProgram(programId);
			CheckError();
		}

		/**
		 * Read source from file and compile
		 * */
		private int CompileShader(string shaderFilename, ShaderType shaderType)
		{
			string shaderSource = ReadShaderSource(shaderFilename);
			int id = CompileShaderFromSource(shaderType, shaderSource);
			return id;
		}

		/**
		 * Compile source, check for errors.
		 * */
		private int CompileShaderFromSource(ShaderType shaderType, string shaderSource)
		{
			int id = GL.CreateShader(shaderType);
			GL.ShaderSource(id, shaderSource);
			GL.CompileShader(id);
			int status_code = -1;
			string info = "";

			// Check error
			GL.GetShaderInfoLog(id, out info);
			GL.GetShader(id, ShaderParameter.CompileStatus, out status_code);
			if (status_code != 1)
			{
				Console.WriteLine("Failed to Compile " + shaderType + " source." +
				Environment.NewLine + info + Environment.NewLine + "Status Code: " + status_code.ToString());
				return -1;
			}

			return id;
		}

		/**
		 * Read source from text file.
		 * */
		private string ReadShaderSource(string filename)
		{
			return System.IO.File.ReadAllText(AssetPath.GetPathToAsset(filename));
		}

		/**
		 * Link the compiled shaders to program.
		 * */
		private int LinkProgram(int vertexShaderId, int fragmentShaderId)
		{
			CheckError(); 
			int shaderProgram = GL.CreateProgram();
			GL.AttachShader(shaderProgram, vertexShaderId);
			GL.AttachShader(shaderProgram, fragmentShaderId);
			GL.LinkProgram(shaderProgram);
			GL.ValidateProgram(shaderProgram);
			CheckError();
			return shaderProgram;
		}

		/**
		 * Check OpenGL error
		 * */
		public static void CheckError()
		{
			ErrorCode ec = GL.GetError();
			if (ec != 0)
			{
				Console.WriteLine("GL error: " + ec.ToString());
			}
		}
	}
}

