namespace computergraphics
{
    using System.Collections.Generic;

    using OpenTK;
    using OpenTK.Graphics;
    using OpenTK.Graphics.OpenGL;

    public class Core
    {
        private readonly List<RenderVertex> point;

        private readonly float viscosity;

        private readonly float mass;

        private readonly Vector3 velocity;

        private float density, pressure;

        private Vector3 position;

        public float Viscosity => viscosity;

        public float Mass => mass;

        public float Density
        {
            get
            {
                return density;
            }

            set
            {
                density = value;
            }
        }

        public float Pressure
        {
            get
            {
                return pressure;
            }

            set
            {
                pressure = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Vector3 Velocity => velocity;

        public float RestingDensity { get; }

        public Core(Vector3 pos, float vis, float m, float d, float p, Vector3 vel)
        {
            viscosity = vis;
            mass = m;
            density = d;
            RestingDensity = d;
            pressure = p;
            velocity = vel;
            position = pos;
            point = new List<RenderVertex> { new RenderVertex(position, position.Normalized(), Color4.Aquamarine) };
        }

        public override string ToString()
        {
            return position.ToString();
        }

        public bool Equals(Core c)
        {
            return c.position == this.position;
        }
    }
}
