namespace computergraphics
{
	/**
	 * Application main class.
	 * */
	class Application
	{
		public static void Main()
		{
			Scene scene = new ShadowScene(); 
			OpenTKWindow window = new OpenTKWindow(scene);
			window.Run ();
		}
	}
}
