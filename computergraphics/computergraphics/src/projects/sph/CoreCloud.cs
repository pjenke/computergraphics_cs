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

        public CoreCloud(Vector3 g, float h, List<Core> cores)
        {
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
       
        public override void TimerTick(int counter)
        {
            _renderVertices = _renderVertices.Zip(Cores, (vertex, core) => UpdateVertexPosition(vertex, core.Position)).ToList();
            _vbo.Invalidate();
        }

        private RenderVertex UpdateVertexPosition(RenderVertex renderVertex, Vector3 newPosition)
        {
            renderVertex.Position = newPosition;
            return renderVertex;
        }

        public override void DrawGL(RenderMode mode, Matrix4 modelMatrix)
        {
            GL.PointSize(5);
            _vbo.Draw();
        }
    }
}
