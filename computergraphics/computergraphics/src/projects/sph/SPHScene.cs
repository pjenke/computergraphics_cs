namespace computergraphics
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using computergraphics.projects.sph;

    using OpenTK;
    using src.projects.sph;
    using System.Threading.Tasks;

    public class SphScene : Scene
    {
        private const float ParticleCount = 500;

        private const float H = 0.1f;

        private const float Viscosity = 0.1f;

        private const float Mass = 1f;

        private readonly Vector3 _gravity = new Vector3(0, -0.1f, 0);

        private readonly Vector3 _velocity = new Vector3(0,0,0);

        public SphScene() : base(100, Shader.ShaderMode.PHONG, RenderMode.REGULAR)
        {
            GetRoot().LightPosition = new Vector3(1, 1, 1);
            GetRoot().Animated = true;

            //
            var load = new LoadFile(@"C:\Users\Maxi\Source\Repos\computergraphics_cs\Save.txt", GetRoot());
            var thread = new Thread(load.PlayScene);
            thread.Start();
            //

            /*
            var cores = new List<Core>();
            BuildCoreTower(cores);
            var cloud = new CoreCloud(_gravity, H, cores, Viscosity);
            //var container = new CubeNode(1f);
            var container = new SphereNode(1f,20);
            var sph = new Sph(cloud, container, true);
            var calcThread = new Thread(sph.StartCalculation);
            calcThread.Start();
            */
        }

        private void BuildSceneWithRandomPlacedCores(List<Core> cores)
        {
            var rand = new Random();
            for (int i = 0; i < ParticleCount; i++)
            {
                var core = new Core(new Vector3(rand.Next(1, 50) * 0.01f, rand.Next(1, 50) * 0.01f, rand.Next(1, 50) * 0.01f), Mass, _velocity,GetRoot());
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
            for (var x = -0.5f; x < 0.5f; x += 0.07f)
            {
                for (var y = -0.5f; y < 0.5f; y += 0.07f)
                {
                    for (var z = -0.5f; z < 0.5f; z += 0.07f)
                    {
                        var core = new Core(new Vector3(x, y, z), Mass, _velocity,GetRoot());
                        cores.Add(core);
                    }
                }
            }
        }

    }
}
