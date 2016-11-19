namespace computergraphics
{
    using framework;

    /**
	 * Application main class.
	 * */
	class Application
	{
		public static void Main()
		{
			Scene scene = new SphScene(); 
			//Scene scene = new PointCloudScene();
	        OpenTKWindow window = new OpenTKWindow(scene);
	        window.Run();
	    }
	}
}
