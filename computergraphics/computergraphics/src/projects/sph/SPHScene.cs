using OpenTK;
using System;
using System.Collections.Generic;
using System.Threading;

namespace computergraphics
{
    class SPHScene : Scene
    {
        private const float PARTICLE_COUNT = 500, VISCOSITY = 0.1f, MASS = 0.1f, DENSITY = 0.001f, PRESSURE = 0, RADIUS = 1, H = 0.2f;
        private Vector3 velocity = new Vector3(0, 0, 0), gravity = new Vector3(0,-0.1f,0);

        public SPHScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
        {
            Random rand = new Random();
            GetRoot().LightPosition = new Vector3(1, 1, 1);
            GetRoot().Animated = true;
            CoreCloud cloud;
            Core core;
            cloud = new CoreCloud(gravity, H);
            for (int i = 0; i < PARTICLE_COUNT; i++)
            { 
                core = new Core(new Vector3(rand.Next(1,50)*0.01f, rand.Next(1, 50) * 0.01f, rand.Next(1, 50) * 0.01f), VISCOSITY, MASS, DENSITY, PRESSURE, velocity);
                if (!cloud.CoreList.Contains(core))
                {
                    cloud.Add(core);
                    GetRoot().AddChild(core);
                } else { i--; }
            }
            SPH sph = new SPH(ref cloud);
            Thread calcThread = new Thread(sph.StartCalc);
            calcThread.Start();            
        }

    }
}
