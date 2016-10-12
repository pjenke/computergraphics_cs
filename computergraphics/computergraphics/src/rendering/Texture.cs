using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
	/**
	 * Handles an OpenGL-Texture.
	 * */
	public class Texture
	{

		/**
		 * OpenGL texture id.
		 * */
		private int textureId = -1;

		/**
		 * Texture filename
		 * */
		private string filename;

		public Texture (string filename)
		{
			this.filename = filename;
		}

		/**
		 * Returns true if the texture is loaded.
		 * */
		public bool IsLoaded()
		{
			return textureId >= 0;
		}

		/**
		 * Load texture image from file and create GL texture object.
		 * */
		public void Load()
		{
			Load (filename);
		}

		/**
		 * Load texture image from file and create GL texture object.
		 * */
		public void Load(string filename)
		{
			this.filename = filename;
			if (String.IsNullOrEmpty (filename)) {
				throw new ArgumentException (filename);
			}

			textureId = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			string path = AssetPath.GetPathToAsset(filename);
			Image image = Image.FromFile(path);
			Bitmap bmp = new Bitmap(image);
			BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

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

