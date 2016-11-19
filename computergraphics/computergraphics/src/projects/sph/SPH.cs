namespace computergraphics
{
    using System;
    using System.Collections.Generic;

    using computergraphics.projects.sph;

    using OpenTK;

    public class Sph
    {

        private readonly CoreCloud coreCloud;

        private List<Core> coreList;

        private float cubeSize;

        private int cubeNumXZ, cubeNumY, refreshRate = 5;

        private List<List<List<List<Core>>>> rasterList = new List<List<List<List<Core>>>>();

        public Sph(CoreCloud coreCloud)
        {
            this.coreCloud = coreCloud;
            this.coreList = coreCloud.Cores;
            this.cubeSize = coreCloud.H * 2;
            this.cubeNumXZ = (int)(1.0 / cubeSize);
            this.cubeNumY = (int)(2.0 / cubeSize);
            for(int x = 0; x < cubeNumXZ; x++)
            {
                rasterList.Add(new List<List<List<Core>>>());
                for(int y = 0; y < cubeNumY; y++)
                {
                    rasterList[x].Add(new List<List<Core>>());
                    for(int z = 0; z < cubeNumXZ; z++)
                    {
                        rasterList[x][y].Add(new List<Core>());
                    }
                }
            }
        }

        public void StartCalc()
        {
            int i = 0;
            while (true)
            {
                if (i % refreshRate == 0)
                    refreshList();
                foreach (var core in coreCloud.Cores)
                {
                    List<Core> coreListNow = rasterList[(int)((core.Position.X - 0.01) * cubeNumXZ)][(int)((core.Position.Y/2 - 0.01) * cubeNumY)][(int)((core.Position.Z - 0.01) * cubeNumXZ)];
                    var density = CalcDensity(coreList, core, coreCloud.H);
                    core.Pressure = density - core.RestingDensity;
                    core.Density = density;
                    var pressure = CalcPressure(coreListNow, core, coreCloud.H);
                    var viscosity = CalcViscosity(coreListNow, core, coreCloud.H);
                    var velocity = coreCloud.Gravity - pressure + viscosity;
                    bool x = false, y = false, z = false;

                    if (core.Position.Y + velocity.Y >= 0 && core.Position.Y + velocity.Y <= 2)
                        y = true;
                    if (core.Position.X + velocity.X <= 1 && core.Position.X + velocity.X >= 0)
                        x = true;
                    if (core.Position.Z + velocity.Z <= 1 && core.Position.Z + velocity.Z >= 0)
                        z = true;

                    if (!x && y && z)
                    {
                        velocity.X = 0f;
                    }
                    else if (x && !y && z)
                    {
                        velocity.Y = 0f;
                    }
                    else if (x && y)
                    {
                        velocity.Z = 0f;
                    }
                    else if (!x && !y && z)
                    {
                        velocity.X = 0f;
                        velocity.Y = 0f;
                    }
                    else if (x)
                    {
                        velocity.Y = 0f;
                        velocity.Z = 0f;
                    }
                    else
                    {
                        velocity = Vector3.Zero;
                    }

                    core.Position += velocity;
                }
                i++;
            }
        }

        public float CalcDensity(List<Core> coreL, Core core, float h)
        {
            float result = 0f;
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
            float x = 0f, y = 0f, z = 0f;
            foreach (var c in coreL)
            {
                var r = core.Position - c.Position;
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
            var result = new Vector3(x, y, z);
            return result;
        }

        public Vector3 CalcViscosity(List<Core> coreL, Core core, float h)
        {
            float x = 0f, y = 0f, z = 0f;
            if (core.Density > 0f)
            {
                foreach (var c in coreL)
                {
                    var r = core.Position - c.Position;
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
            var result = new Vector3(x, y, z);
            return result;
        }

        public void refreshList()
        {
            for (int x = 0; x < cubeNumXZ; x++)
            {
                for (int y = 0; y < cubeNumY; y++)
                {
                    for (int z = 0; z < cubeNumXZ; z++)
                    {
                        rasterList[x][y][z].Clear();
                    }
                }
            }
            int cx, cy, cz;
            foreach (Core c in coreList)
            {
                cx = (int)((c.Position.X - 0.01) * cubeNumXZ);
                cy = (int)((c.Position.Y/2 - 0.01) * cubeNumY);
                cz = (int)((c.Position.Z - 0.01) * cubeNumXZ);
                rasterList[cx][cy][cz].Add(c);
            }
        }

    }
}
