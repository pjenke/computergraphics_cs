using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
    class Core : LeafNode
    {

        private VertexBufferObject vbo = new VertexBufferObject();
        private Vector3 pos = new Vector3(0, 0, 0);

        public Core(Vector3 position)
        {
            pos = position;
            List<RenderVertex> point = new List<RenderVertex>();
            point.Add(new RenderVertex(pos,pos.Normalized(),Color4.Aquamarine));
            vbo.Setup(point, PrimitiveType.Points);
        }

        public override void DrawGL(RenderMode mode, Matrix4 modelMatrix)
        {
            GL.PointSize(3);
            vbo.Draw();
        }

        public override void TimerTick(int counter)
        {
        }
    }
}
