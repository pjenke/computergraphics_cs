using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Handles an OpnGL-Texture
	 * */
	public class Texture
	{

		private int textureId = -1;

		private string filename;

		public Texture (string filename)
		{
			this.filename = filename;
		}

		/**
		 * Load texture image from file and create GL texture object.
		 * */
		public void Load()
		{
			if (String.IsNullOrEmpty(filename))
				throw new ArgumentException(filename);

			textureId = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			// We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
			// We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
			// mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			Bitmap bmp = new Bitmap(filename);
			BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

			bmp.UnlockBits(bmp_data);

			Console.WriteLine ("Texture " + filename + " loaded.");
		}

		/**
		 * Bind the texture as current texture.
		 * */
		public void Bind()
		{
			GL.BindTexture(TextureTarget.Texture2D, textureId);
		}
	}
}

