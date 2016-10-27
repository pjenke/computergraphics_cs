using OpenTK;
using System;
using System.Collections.Generic;

namespace computergraphics
{
    class SPH
    {

        private CoreCloud coreC;

        public SPH(ref CoreCloud coreCloud)
        {
            this.coreC = coreCloud;
        }

        public void StartCalc()
        {
            float density = 0;
            Vector3 pressure = new Vector3(0, 0, 0), viscosity = new Vector3(0, 0, 0), velocity = new Vector3(0, 0, 0);
            while (true)
            {
                foreach (Core c in coreC.CoreList)
                {
                    density = CalcDensity(coreC.CoreList, c, coreC.H);
                    c.Pressure = density - c.RestingDensity;
                    c.Density = density;
                    pressure = CalcPressure(coreC.CoreList, c, coreC.H);
                    viscosity = CalcViscosity(coreC.CoreList, c, coreC.H);
                    velocity = coreC.Gravity - pressure + viscosity;
                    Boolean x = false, y = false, z = false;
                    if (c.Position.Y + velocity.Y >= 0 && c.Position.Y + velocity.Y <= 2)
                        y = true;
                    if (c.Position.X + velocity.X <= 1 && c.Position.X + velocity.X >= 0)
                        x = true;
                    if (c.Position.Z + velocity.Z <= 1 && c.Position.Z + velocity.Z >= 0)
                        z = true;
                    if (x && y && z)
                        c.Position += velocity;
                    else if (!x && y && z)
                        c.Position += new Vector3(0, velocity.Y, velocity.Z);
                    else if (x && !y && z)
                        c.Position += new Vector3(velocity.X, 0, velocity.Z);
                    else if (x && y && !z)
                        c.Position += new Vector3(velocity.X, velocity.Y, 0);
                    else if (!x && !y && z)
                        c.Position += new Vector3(0, 0, velocity.Z);
                    else if (x && !y && !z)
                        c.Position += new Vector3(velocity.X, 0, 0);
                    else if (!x && y && !z)
                        c.Position += new Vector3(0, velocity.Y, 0);
                }
            }
        }

        public float CalcDensity(List<Core> coreL, Core core, float h)
        {
            float result = 0;
            foreach (Core c in coreL)
            {
                Vector3 r = core.Position - c.Position;
                if (!c.Equals(core))
                    result += (float)(c.Mass * 315 / (64 * Math.PI * Math.Pow(h, 9)) * Math.Pow(Math.Pow((Math.Pow(h, 2) - r.Length), 2), 3));
            }
            return result;
        }

        public Vector3 CalcPressure(List<Core> coreL, Core core, float h)
        {
            float x = 0, y = 0, z = 0;
            foreach (Core c in coreL)
            {
                Vector3 r = core.Position - c.Position;
                if (h - r.Length > 0)
                {
                    if (!c.Equals(core))
                    {
                        x += (float)(c.Mass * ((core.Pressure / Math.Pow(core.Density, 2)) + (c.Pressure / Math.Pow(c.Density, 2)))
                                  * (-45 / (Math.PI * Math.Pow(h, 6))) * Math.Pow((h - r.Length), 2) * (r.X / r.Length));
                        y += (float)(c.Mass * ((core.Pressure / Math.Pow(core.Density, 2)) + (c.Pressure / Math.Pow(c.Density, 2)))
                                  * (-45 / (Math.PI * Math.Pow(h, 6))) * Math.Pow((h - r.Length), 2) * (r.Y / r.Length));
                        z += (float)(c.Mass * ((core.Pressure / Math.Pow(core.Density, 2)) + (c.Pressure / Math.Pow(c.Density, 2)))
                                  * (-45 / (Math.PI * Math.Pow(h, 6))) * Math.Pow((h - r.Length), 2) * (r.Z / r.Length));
                    }
                }
            }
            Vector3 result = new Vector3(x, y, z);
            return result;
        }

        public Vector3 CalcViscosity(List<Core> coreL, Core core, float h)
        {
            float x = 0, y = 0, z = 0;
            if (core.Density != 0)
            {
                foreach (Core c in coreL)
                {
                    Vector3 r = core.Position - c.Position;
                    if (h - r.Length > 0)
                    {
                        if (!c.Equals(core))
                        {
                            x += (float)(c.Mass * ((c.Velocity.X - core.Velocity.X) / c.Density) * (45 / (Math.PI * Math.Pow(h, 6)))
                                 * (h - r.Length));
                            y += (float)(c.Mass * ((c.Velocity.Y - core.Velocity.Y) / c.Density) * (45 / (Math.PI * Math.Pow(h, 6)))
                                 * (h - r.Length));
                            z += (float)(c.Mass * ((c.Velocity.Z - core.Velocity.Z) / c.Density) * (45 / (Math.PI * Math.Pow(h, 6)))
                                 * (h - r.Length));
                        }
                    }
                }
                x = x * (core.Viscosity / core.Density);
                y = y * (core.Viscosity / core.Density);
                z = z * (core.Viscosity / core.Density);
            }
            Vector3 result = new Vector3(x, y, z);
            return result;
        }

    }
}
