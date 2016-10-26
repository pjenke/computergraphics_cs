using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace computergraphics
{
    class Core : LeafNode
    {
  
        private float viscosity = 0, mass = 0, density = 0, pressure = 0, restingDensity = 0;
        private VertexBufferObject vbo = new VertexBufferObject();
        private Vector3 position = new Vector3(0, 0, 0), velocity = new Vector3(0,0,0);
        List<RenderVertex> point;

        public float Viscosity
        {
            get
            {
                return viscosity;
            }
        }

        public float Mass
        {
            get
            {
                return mass;
            }
        }

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

        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }
        }

        public float RestingDensity
        {
            get
            {
                return restingDensity;
            }
        }

        public Core(Vector3 pos, float vis, float m, float d, float p, Vector3 vel)
        {
            viscosity = vis;
            mass = m;
            density = d;
            restingDensity = d;
            pressure = p;
            velocity = vel;
            position = pos;
            point = new List<RenderVertex>();
            point.Add(new RenderVertex(position,position.Normalized(),Color4.Aquamarine));
            vbo.Setup(point, PrimitiveType.Points);
        }

        public override void DrawGL(RenderMode mode, Matrix4 modelMatrix)
        {
            point.RemoveAt(0);
            point.Add(new RenderVertex(position, position.Normalized(), Color4.Aquamarine));
            vbo = new VertexBufferObject();
            vbo.Setup(point, PrimitiveType.Points);
            GL.PointSize(3);
            vbo.Draw();
        }

        public override void TimerTick(int counter)
        {
        }

        public Core Clone()
        {
            return new Core(position, viscosity, mass, density, pressure, velocity);
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
