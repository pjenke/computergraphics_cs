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
            /* for (int i = 0; i < PARTICLE_COUNT; i++)
            { 
                core = new Core(new Vector3(rand.Next(1,50)*0.01f, rand.Next(1, 50) * 0.01f, rand.Next(1, 50) * 0.01f), VISCOSITY, MASS, DENSITY, PRESSURE, velocity);
                if (!cloud.CoreList.Contains(core))
                {
                    cloud.Add(core);
                    GetRoot().AddChild(core);
                } else { i--; }
            } */
            for(float x = 0.4f; x < 0.6f; x += 0.04f)
            {
                for(float y = 0.4f; y < 0.9f; y += 0.04f)
                {
                    for(float z = 0.4f; z < 0.6f; z += 0.04f)
                    {
                        core = new Core(new Vector3(x, y, z), VISCOSITY, MASS, DENSITY, PRESSURE, velocity);
                        cloud.Add(core);
                        GetRoot().AddChild(core);
                    }
                }
            }
            SPH sph = new SPH(ref cloud);
            Thread calcThread = new Thread(sph.StartCalc);
            calcThread.Start();            
        }

    }
}
