namespace computergraphics
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using computergraphics.projects.sph;

    using OpenTK;

    public class SphScene : Scene
    {
        private const float ParticleCount = 500;

        private const float Pressure = 0, H = 0.1f;

        private const float Viscosity = 0.1f;

        private const float Mass = 0.1f;

        private const float Density = 0.1f;

        private readonly Vector3 _gravity = new Vector3(0, -0.1f, 0);

        private readonly Vector3 _velocity = Vector3.Zero;

        public SphScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
        {
            GetRoot().LightPosition = new Vector3(1, 1, 1);
            GetRoot().Animated = true;

            var cores = new List<Core>();
            BuildCoreTower(cores);

            var cloud = new CoreCloud(_gravity, H, cores);
            GetRoot().AddChild(cloud);

            var sph = new Sph(cloud);
            var calcThread = new Thread(sph.StartCalc);
            calcThread.Start();
        }

        private void BuildSceneWithRandomPlacedCores(List<Core> cores)
        {
            var rand = new Random();
            for (int i = 0; i < ParticleCount; i++)
            {
                var core = new Core(new Vector3(rand.Next(1, 50) * 0.01f, rand.Next(1, 50) * 0.01f, rand.Next(1, 50) * 0.01f), Viscosity, Mass, Density, Pressure, _velocity);
                if (!cores.Contains(core))
                {
                    cores.Add(core);
                }
                else
                {
                    i--;
                }
            }
        }

        private void BuildCoreTower(List<Core> cores)
        {
            for (var x = 0.4f; x < 0.6f; x += 0.04f)
            {
                for (var y = 0.4f; y < 1.9f; y += 0.04f)
                {
                    for (var z = 0.4f; z < 0.6f; z += 0.04f)
                    {
                        var core = new Core(new Vector3(x, y, z), Viscosity, Mass, Density, Pressure, _velocity);
                        cores.Add(core);
                    }
                }
            }
        }
    }
}
