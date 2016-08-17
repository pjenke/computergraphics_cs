using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace computergraphics
{
	/**
	 * Rendering vie vertex buffer objects (VBO).
	 * */
	public class VertexBufferObject
	{
		/**
		 * List of vertices to be rendered.
		 * */
		private List<RenderVertex> renderVertices = null;

		/**
		 * Buffer indicides.
		 * */
		private int vertexBufferId = -1, normalBufferId = -1, colorBufferId = -1, indexBufferId;

		/**
		 * Use this primitive type for rendering. Attentions: This implies the number of vertices, normals and colors required; e.g. triangles require three vertices each.
		 * */
		private PrimitiveType primitiveType = PrimitiveType.Triangles;

		public VertexBufferObject()
		{
		}

		/**
		 * Set the data for the Buffer. The format is described together with the vertices, normals and colors attributes.
		 * */
		public void Setup(List<RenderVertex> renderVertices, PrimitiveType primitiveType)
		{
			this.renderVertices = renderVertices;
			this.primitiveType = primitiveType;
		}

		/**
		 * Init VBO, called only once (or if the date changed).
		 * */
		private void Init()
		{
			if (renderVertices == null || renderVertices.Count == 0)
			{
				//Console.WriteLine("Invalid render vertex list - must contain at least vertices for one primitive.");
				return;
			}

			// Vertices
			float[] vertices = new float[renderVertices.Count * 3];
			for (int i = 0; i < renderVertices.Count; i++)
			{
				vertices[i * 3] = renderVertices[i].Position.X;
				vertices[i * 3 + 1] = renderVertices[i].Position.Y;
				vertices[i * 3 + 2] = renderVertices[i].Position.Z;
			}
			GL.GenBuffers(1, out vertexBufferId);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

			// Normals
			float[] normals = new float[renderVertices.Count * 3];
			for (int i = 0; i < renderVertices.Count; i++)
			{
				normals[i * 3] = renderVertices[i].Normal.X;
				normals[i * 3 + 1] = renderVertices[i].Normal.Y;
				normals[i * 3 + 2] = renderVertices[i].Normal.Z;
			}
			GL.GenBuffers(1, out normalBufferId);
			GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferId);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(normals.Length * sizeof(float)), normals, BufferUsageHint.StaticDraw);

			// Colors
			float[] colors = new float[renderVertices.Count * 4];
			for (int i = 0; i < renderVertices.Count; i++)
			{
				colors[i * 4] = renderVertices[i].Color.R;
				colors[i * 4 + 1] = renderVertices[i].Color.G;
				colors[i * 4 + 2] = renderVertices[i].Color.B;
				colors[i * 4 + 3] = renderVertices[i].Color.A;
			}
			GL.GenBuffers(1, out colorBufferId);
			GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferId);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(colors.Length * sizeof(float)), colors, BufferUsageHint.StaticDraw);

			// Indices
			int[] indices = new int[renderVertices.Count];
			for (int i = 0; i < renderVertices.Count; i++)
			{
				indices[i] = i;
			}
			GL.GenBuffers(1, out indexBufferId);
			GL.BindBuffer(BufferTarget.ArrayBuffer, indexBufferId);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(indices.Length * sizeof(int)), indices, BufferUsageHint.StaticDraw);

			Shader.CheckError();
		}

		/**
		 * Draw using the VBO
		 * */
		public void Draw()
		{
			if (vertexBufferId < 0 || normalBufferId < 0 || colorBufferId < 0 || indexBufferId < 0 )
			{
				Init();
			}

			GL.EnableVertexAttribArray(
				ShaderAttributes.GetInstance().LocationPosition);
			GL.EnableVertexAttribArray(
				ShaderAttributes.GetInstance().LocationNormal);
			GL.EnableVertexAttribArray(
				ShaderAttributes.GetInstance().LocationColor);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
			GL.VertexAttribPointer(ShaderAttributes.GetInstance().LocationPosition,
								   3, VertexAttribPointerType.Float, false, 0, IntPtr.Zero);

			GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferId);
			GL.VertexAttribPointer(ShaderAttributes.GetInstance().LocationNormal,
				3, VertexAttribPointerType.Float, false, 0, IntPtr.Zero);

			GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferId);
			GL.VertexAttribPointer(ShaderAttributes.GetInstance().LocationColor,
				4, VertexAttribPointerType.Float, false, 0, IntPtr.Zero);

			GL.DrawArrays(primitiveType, 0, renderVertices.Count);

			Shader.CheckError();
		}

		/**
		 * Delete all buffers.
		 * */
		public void Invalidate()
		{
			if (vertexBufferId >= 0)
			{
				GL.DeleteBuffers(1, ref vertexBufferId);
				vertexBufferId = -1;
			}

			if (normalBufferId >= 0)
			{
				GL.DeleteBuffers(1, ref normalBufferId);
				normalBufferId = -1;
			}

			if (colorBufferId >= 0)
			{
				GL.DeleteBuffers(1, ref colorBufferId);
				colorBufferId = -1;
			}

			if (indexBufferId >= 0)
			{
				//GL.DeleteBuffers(1, ref indexBufferId);
				indexBufferId = -1;
			}
		}
	}
}

