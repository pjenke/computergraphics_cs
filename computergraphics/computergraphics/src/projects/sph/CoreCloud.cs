namespace computergraphics.projects.sph
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenTK;
    using OpenTK.Graphics;
    using OpenTK.Graphics.OpenGL;

    public class CoreCloud : LeafNode
    {
        private readonly VertexBufferObject _vbo;

        private List<RenderVertex> _renderVertices;

        public CoreCloud(Vector3 g, float h, List<Core> cores, float vis)
        {
            Viscosity = vis;
            Cores = cores;
            Gravity = g;
            H = h;
            _renderVertices = new List<RenderVertex>();
            _vbo = new VertexBufferObject();

            Initialize();
        }

        private void Initialize()
        {
            foreach (var core in Cores)
            {
                _renderVertices.Add(new RenderVertex(core.Position, Vector3.UnitY, Color4.MediumPurple));
            }

            _vbo.Setup(_renderVertices, PrimitiveType.Points);
        }

        public Vector3 Gravity { get; }

        public float H { get; }

        public List<Core> Cores { get; }

        public float Viscosity { get; }

        public override void TimerTick(int counter)
        {
            _renderVertices = _renderVertices.Zip(Cores, (vertex, core) => UpdateVertex(vertex, core)).ToList();
            _vbo.Invalidate();
        }

        private RenderVertex UpdateVertex(RenderVertex renderVertex, Core core)
        {
            renderVertex.Position = core.Position;
            renderVertex.Color = core._Color;
            return renderVertex;
        }

        public override void DrawGL(RenderMode mode, Matrix4 modelMatrix)
        {
            GL.PointSize(3);
            _vbo.Draw();
        }
    }
}
