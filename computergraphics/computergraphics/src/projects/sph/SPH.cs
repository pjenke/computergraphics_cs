using OpenTK;
using System;
using System.Collections.Generic;

namespace computergraphics
{
    class SPH
    {

        public SPH()
        {
        }

        public float CalcDensity(List<Core> coreL, Core core, float h)
        {
            float result = 0;
            foreach(Core c in coreL)
            {
                Vector3 r = core.Position - c.Position;
                if(c != core)
                    result += (float)(c.Mass * 315 / (64 * Math.PI * Math.Pow(h,9)) * Math.Pow(Math.Pow((Math.Pow(h,2) - r.Length),2),3));
            }
            return result;
        }

        public Vector3 CalcPressure(List<Core> coreL, Core core, float h)
        {
            float x = 0, y = 0, z = 0;
            foreach(Core c in coreL)
            {
                Vector3 r = core.Position - c.Position;
                if (c != core)
                {
                    x += (float)(c.Mass * ((core.Pressure / Math.Pow(core.Density, 2)) + (c.Pressure / Math.Pow(c.Density, 2)))
                              * (-45 / (Math.PI * Math.Pow(h, 6))) * Math.Pow((h - r.Length), 2) * (r.X / r.Length));
                    y += (float)(c.Mass * ((core.Pressure / Math.Pow(core.Density, 2)) + (c.Pressure / Math.Pow(c.Density, 2)))
                              * (-45 / (Math.PI * Math.Pow(h, 6))) * Math.Pow((h - r.Length), 2) * (r.Y / r.Length));
                    z += (float)(c.Mass * ((core.Pressure / Math.Pow(core.Density, 2)) + (c.Pressure / Math.Pow(c.Density, 2)))
                              * (-45 / (Math.PI * Math.Pow(h, 6))) * Math.Pow((h - r.Length), 2) * (r.Z / r.Length));
                }
            }
            Vector3 result = new Vector3(x, y, z);
            return result;
        }

        public Vector3 CalcViscosity(List<Core> coreL, Core core, float h)
        {
            float x = 0, y = 0, z = 0;
            foreach(Core c in coreL)
            {
                Vector3 r = core.Position - c.Position;
                if(c != core)
                {
                    x += (float)(c.Mass * ((c.Velocity.X - core.Velocity.X) / c.Density) * (45 / (Math.PI * Math.Pow(h, 6))) 
                         * (h - r.Length));
                    y += (float)(c.Mass * ((c.Velocity.Y - core.Velocity.Y) / c.Density) * (45 / (Math.PI * Math.Pow(h, 6))) 
                         * (h - r.Length));
                    z += (float)(c.Mass * ((c.Velocity.Z - core.Velocity.Z) / c.Density) * (45 / (Math.PI * Math.Pow(h, 6)))
                         * (h - r.Length));
                }
            }
            x *= (core.Viscosity / core.Density);
            y *= (core.Viscosity / core.Density);
            z *= (core.Viscosity / core.Density);
            Vector3 result = new Vector3(x,y,z);
            return result;
        }

    }
}
