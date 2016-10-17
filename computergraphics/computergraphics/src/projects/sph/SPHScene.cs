using OpenTK;
using System;
using System.Collections.Generic;

namespace computergraphics
{
    class SPHScene : Scene
    {

        private List<CoreCloud> cloudList = new List<CoreCloud>();
        private const float PARTICLE_COUNT = 96, CLOUD_NUM = 32, VISCOSITY = 1, MASS = 1, DENSITY = 1, PRESSURE = 1, RADIUS = 1;
        private Vector3 velocity = new Vector3(0, 1, 0), gravity = new Vector3(0,-10,0);

        public SPHScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
        {
            GetRoot().LightPosition = new Vector3(1, 1, 1);
            GetRoot().Animated = true;
            CoreCloud cloud;
            Core core;
            cloud = new CoreCloud(gravity);
            cloudList.Add(cloud);
            core = new Core(new Vector3(0, 0, 0), VISCOSITY, MASS, DENSITY, PRESSURE, velocity);
            cloud.Add(core.Clone());
            GetRoot().AddChild(core.Clone());
        }

    }
}
