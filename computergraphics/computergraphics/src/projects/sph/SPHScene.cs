using OpenTK;
using System.Collections.Generic;

namespace computergraphics
{
    class SPHScene : Scene
    {

        private List<CoreCloud> cloudList = new List<CoreCloud>();
        private const int PARTICLE_COUNT = 96;
        private const int CLOUD_NUM = 32;

        public SPHScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
        {
            GetRoot().LightPosition = new Vector3(1, 1, 1);
            GetRoot().Animated = true;
            CoreCloud cloud1 = new CoreCloud(new Vector3(0, 0, 1));
            Core core1 = new Core(new Vector3(0, 0, 1));
            cloud1.Add(core1);
            GetRoot().AddChild(core1);
        }

    }
}
