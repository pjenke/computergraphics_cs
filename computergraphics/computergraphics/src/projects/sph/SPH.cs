namespace computergraphics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using computergraphics.projects.sph;
    using computergraphics.projects.sph.Datastructures;

    using OpenTK;
    using System.Threading.Tasks;
    using OpenTK.Graphics;

    public class Sph
    {
        private const int RefreshRate = 5,
                          CubeSizeX = 1,
                          CubeSizeY = 2,
                          CubeSizeZ = 1;

        private readonly CoreCloud _coreCloud;

        private readonly List<RasterUnit> _raster;

        private readonly List<Core> _coreList;

        private readonly int _cubeNumX;

        private readonly int _cubeNumZ;

        private readonly int _cubeNumY;

        private readonly float _RestingDensity;

        private readonly double _DensityMagic;

        private readonly double _PressureMagic;

        private readonly double _ViscosityMagic;

        private const float _StepSize = 0.01f, k = 0.00000002f, sigma = 0.01f;

        public Sph(CoreCloud coreCloud)
        {
            _coreCloud = coreCloud;
            _coreList = coreCloud.Cores;
            var cubeSize = _coreCloud.H * 2;
            var hPow9 = Math.Pow(coreCloud.H, 9);
            var hPow6 = Math.Pow(coreCloud.H, 6);
            _DensityMagic = 315 / (64 * Math.PI * hPow9);
            _PressureMagic = (-45 / (Math.PI * hPow6));
            _ViscosityMagic = (45 / (Math.PI * hPow6));
            _RestingDensity = (float)(_coreList[0].Mass * _DensityMagic * Math.Pow(_coreCloud.H * _coreCloud.H, 3)) / 2;
            _cubeNumZ = (int)(CubeSizeZ / cubeSize);
            _cubeNumX = (int)(CubeSizeX / cubeSize);
            _cubeNumY = (int)(CubeSizeY / cubeSize);
            _raster = new List<RasterUnit>();
            Initialize();
        }

        private void Initialize()
        {
            InitializeRaster();
            InitializeUnits();
        }

        private void InitializeRaster()
        {
            for (int x = 0; x < _cubeNumX; x++)
            {
                for (int y = 0; y < _cubeNumY; y++)
                {
                    for (int z = 0; z < _cubeNumZ; z++)
                    {
                        var rasterUnit = new RasterUnit(x, y, z);
                        _raster.Add(rasterUnit);
                    }
                }
            }
        }

        private void InitializeUnits()
        {
            foreach (var unit in _raster)
            {
                unit.Initialize(_raster);
            }
        }

        public void StartCalculation()
        {
            int i = 0;
            while (true)
            {
                if (i % RefreshRate == 0)
                {
                    RefreshList();
                }

                foreach (var core in _coreCloud.Cores)
                {
                    var rasterUnit = FindRasterUnitForCore(core);
                    var neighborCores = rasterUnit.Neighbors.SelectMany(unit => unit.Cores);
                    var coresToCheck = new List<Core>(rasterUnit.Cores);
                    coresToCheck.AddRange(neighborCores);

                    var density = CalculateDensity(coresToCheck, core, _coreCloud.H);

                    float pressure = k * (density - _RestingDensity);
                    var pressureTask = Task.Run(() => CalculatePressure(coresToCheck, core, _coreCloud.H, pressure, density));
                    var viscosityTask = Task.Run(() => CalculateViscosity(coresToCheck, core, _coreCloud.H, density));
                    viscosityTask.Wait();
                    pressureTask.Wait();

                    var velocity = _coreCloud.Gravity - pressureTask.Result + viscosityTask.Result;

                    bool x = false, y = false, z = false;

                    if (core.Position.Y + _StepSize * velocity.Y >= 0 && core.Position.Y + _StepSize * velocity.Y <= CubeSizeY)
                        y = true;
                    if (core.Position.X + _StepSize * velocity.X <= CubeSizeX && core.Position.X + _StepSize * velocity.X >= 0)
                        x = true;
                    if (core.Position.Z + _StepSize * velocity.Z <= CubeSizeZ && core.Position.Z + _StepSize * velocity.Z >= 0)
                        z = true;

                    if (!x && y && z)
                    {
                        //velocity.X = -velocity.X;
                        velocity.X = 0;
                    }
                    else if (x && !y && z)
                    {
                        //velocity.Y = -velocity.Y;
                        velocity.Y = 0;
                    }
                    else if (x && y)
                    {
                        //velocity.Z = -velocity.Z;
                        velocity.Z = 0;
                    }
                    else if (!x && !y && z)
                    {
                        //velocity.X = -velocity.X;
                        //velocity.Y = -velocity.Y;
                        velocity.X = 0;
                        velocity.Y = 0;
                    }
                    else if (x)
                    {
                        //velocity.Y = -velocity.Y;
                        //velocity.Z = -velocity.Z;
                        velocity.Y = 0;
                        velocity.Z = 0;
                    }
                    else
                    {
                        //velocity = -velocity;
                        velocity = Vector3.Zero;
                    }
                    core.Velocity = _StepSize * velocity;
                    core.Position += _StepSize * velocity;
                }
                i++;
            }
        }

        private float CalculateDensity(List<Core> coreL, Core core, float h)
        {
            var result = _RestingDensity;
            foreach (Core c in coreL)
            {
                var r = core.Position - c.Position;
                if (!c.Equals(core))
                    result += (float)(c.Mass * _DensityMagic * Math.Pow(h * h - r.LengthSquared, 3));
            }
            return result;
        }

        private Vector3 CalculatePressure(List<Core> cores, Core core, float h, float pressure, float density)
        {
            var result = new Vector3();
            foreach (var c in cores)
            {
                var r = core.Position - c.Position;
                if (!c.Equals(core))
                {
                    result += (float)(c.Mass * (pressure / (2 * density) + pressure / (2 * density))
                              * _PressureMagic * W(r.Length,h) * W(r.Length, h)) * (r / r.Length);
                }
            }
            return result;
        }

        private Vector3 CalculateViscosity(List<Core> coreL, Core core, float h, float density)
        {
            var result = new Vector3();
            foreach (var c in coreL)
            {
                var r = core.Position - c.Position;
                if (!c.Equals(core))
                {
                    result += (float)(c.Mass * _ViscosityMagic * W(r.Length, h)) * ((c.Velocity - core.Velocity) / density);
                }
            }
            result = result * (_coreCloud.Viscosity / density);
            return result;
        }

        private void RefreshList()
        {
            ClearRaster();
            FillRaster();
        }

        private void FillRaster()
        {
            foreach (var core in _coreList)
            {
                RasterUnit rasterUnit = FindRasterUnitForCore(core);
                rasterUnit.Cores.Add(core);
            }
        }

        private RasterUnit FindRasterUnitForCore(Core core)
        {
            var coreX = (int)(Math.Abs(core.Position.X / CubeSizeX - 0.01) * _cubeNumX);
            var coreY = (int)(Math.Abs(core.Position.Y / CubeSizeY - 0.01) * _cubeNumY);
            var coreZ = (int)(Math.Abs(core.Position.Z / CubeSizeZ - 0.01) * _cubeNumZ);
            var position = new Position { X = coreX, Y = coreY, Z = coreZ };
            var rasterUnit = _raster.Find(unit => unit.IsPositionInUnit(position));
            return rasterUnit;
        }

        private void ClearRaster()
        {
            foreach (var unit in _raster)
            {
                unit.Cores.Clear();
            }
        }

        private float W(float length, float h)
        {
            if (length / h >= 0 && length / h <= 1)
                //return sigma / (4 * h) * (4 - 6 * length * length + 3 * length * length * length);
                return 2 * h - length;
            else if (length / h >= 1 && length / h <= 2)
                //return sigma / (4 * h) * ((2 - length) * (2 - length) * (2 - length));
                return 2 * h - length;
            else
                return 0;
        }
    }
}
