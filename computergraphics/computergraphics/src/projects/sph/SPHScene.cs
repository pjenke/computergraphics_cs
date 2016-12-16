﻿namespace computergraphics
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using computergraphics.projects.sph;

    using OpenTK;

    public class SphScene : Scene
    {
        private const float ParticleCount = 500;

        private const float H = 0.08f;

        private const float Viscosity = 0.01f;

        private const float Mass = 18f;

        private const float Density = 1000f;

        private readonly Vector3 _gravity = new Vector3(0, -10f, 0);

        private readonly Vector3 _velocity = new Vector3(0,0,0);

        public SphScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
        {
            GetRoot().LightPosition = new Vector3(1, 1, 1);
            GetRoot().Animated = true;

            var cores = new List<Core>();
            BuildCoreTower(cores);

            var cloud = new CoreCloud(_gravity, H, cores, Viscosity);
            GetRoot().AddChild(cloud);

            var sph = new Sph(cloud);
            var calcThread = new Thread(sph.StartCalculation);
            calcThread.Start();
        }

        private void BuildSceneWithRandomPlacedCores(List<Core> cores)
        {
            var rand = new Random();
            for (int i = 0; i < ParticleCount; i++)
            {
                var core = new Core(new Vector3(rand.Next(1, 50) * 0.01f, rand.Next(1, 50) * 0.01f, rand.Next(1, 50) * 0.01f), Mass, _velocity);
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
                for (var y = 0.4f; y < 1.6f; y += 0.04f)
                {
                    for (var z = 0.4f; z < 0.6f; z += 0.04f)
                    {
                        var core = new Core(new Vector3(x, y, z), Mass, _velocity);
                        cores.Add(core);
                    }
                }
            }
        }
    }
}
