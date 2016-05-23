using System;

namespace computergraphics
{
	/**
	 * Compute the relative path to the assets folder.
	 * */
	public class AssetPath
	{
		/**
		 * Current implementation: working-dir = bin/<target>.
		 * */
		private static string assetPath = "../../../../assets/";

		/**
		 * Returns the relative path to the asset filename.
		 * */
		public static string getPathToAsset(string assetFilename)
		{
			return assetPath + assetFilename;
		}
	}
}

