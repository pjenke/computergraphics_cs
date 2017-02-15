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

        private SphereNode sphere;

        public Color4 _Color = Color4.Aqua;

        private float density;

        private float pressure;

        private RootNode root;

        private TranslationNode tNode;

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

        public SphereNode Sphere
        {
            get
            {
                return sphere;
            }
        }

        public Core(Vector3 pos, float m, Vector3 vel,RootNode r)
        {
            mass = m;
            root = r;
            velocity = vel;
            position = pos;
            sphere = new SphereNode(0.05f,20);
            tNode = new TranslationNode(position);
            root.AddChild(tNode);
            tNode.AddChild(sphere);
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

        public void SetPosition(Vector3 pos)
        {
            tNode.Translation = pos;
        }
    }
}
