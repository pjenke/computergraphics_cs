namespace computergraphics.projects.sph
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenTK;
    using OpenTK.Graphics;
    using OpenTK.Graphics.OpenGL;

    public class CoreCloud
    {

        public CoreCloud(Vector3 g, float h, List<Core> cores, float vis)
        {
            Viscosity = vis;
            Cores = cores;
            Gravity = g;
            H = h;
        }

        public Vector3 Gravity { get; }

        public float H { get; }

        public List<Core> Cores { get; }

        public float Viscosity { get; }

    }
}
