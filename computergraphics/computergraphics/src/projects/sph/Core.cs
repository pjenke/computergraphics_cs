namespace computergraphics
{
    using System.Collections.Generic;

    using OpenTK;
    using OpenTK.Graphics;

    public class Core
    {
        private readonly List<RenderVertex> point;

        private readonly float mass;

        private Vector3 velocity;

        private Vector3 position;

        public float Mass => mass;

        public Color4 _Color = Color4.Aqua;

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

        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }

        public float RestingDensity { get; }

        public Core(Vector3 pos, float m, Vector3 vel)
        {
            mass = m;
            velocity = vel;
            position = pos;
            point = new List<RenderVertex> { new RenderVertex(position, position.Normalized(), Color4.Aquamarine) };
        }

        public override string ToString()
        {
            return position.ToString();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Core;

            return other?.position == position;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}
