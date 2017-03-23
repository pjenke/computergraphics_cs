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
        private const int RefreshRate = 5;
        
        private const float CubeSizeX = 0.4f,
                            CubeSizeY = 2f,
                            CubeSizeZ = 0.4f;

        private readonly CoreCloud _coreCloud;

        private readonly List<RasterUnit> _raster;

        private readonly List<Core> _coreList;

        private readonly int _cubeNumX;

        private readonly int _cubeNumZ;

        private readonly int _cubeNumY;

        private readonly float _restingDensity;

        private float _lambda;

        private readonly float _WMagic, _WPresMagic;

        private const float _StepSize = 0.00001f, k = 0.00002f, _Bouncy = 3;

        private readonly LeafNode _container;

        public Sph(CoreCloud coreCloud, LeafNode container)
        {
            _container = container;
            _coreCloud = coreCloud;
            _coreList = coreCloud.Cores;
            var cubeSize = _coreCloud.H * 2;
            var hPow9 = Math.Pow(coreCloud.H, 9);
            var hPow6 = Math.Pow(coreCloud.H, 6);
            _cubeNumZ = (int)(CubeSizeZ / cubeSize);
            _cubeNumX = (int)(CubeSizeX / cubeSize);
            _cubeNumY = (int)(CubeSizeY / cubeSize);
            _WMagic = (float)(315 / (64 * Math.PI * Math.Pow(coreCloud.H , 9)));
            _WPresMagic = (float)(15 / (Math.PI * Math.Pow(coreCloud.H, 6)));
            _restingDensity = (float)(_coreList[0].Mass * _WMagic * Math.Pow((coreCloud.H * coreCloud.H - coreCloud.H/2 * coreCloud.H/2),3));
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
                    core.Density = density;
                    float pressure = k * (density - _restingDensity);
                    core.Pressure = pressure;
                }
                foreach (var core in _coreCloud.Cores)
                {
                    var rasterUnit = FindRasterUnitForCore(core);
                    var neighborCores = rasterUnit.Neighbors.SelectMany(unit => unit.Cores);
                    var coresToCheck = new List<Core>(rasterUnit.Cores);
                    coresToCheck.AddRange(neighborCores);
                    var pressureTask = Task.Run(() => CalculatePressure(coresToCheck, core, _coreCloud.H, core.Pressure, core.Density));
                    var viscosityTask = Task.Run(() => CalculateViscosity(coresToCheck, core, _coreCloud.H, core.Density));
                    viscosityTask.Wait();
                    pressureTask.Wait();

                    var velocity = _coreCloud.Gravity - pressureTask.Result + viscosityTask.Result;
                    core.Velocity += _StepSize * velocity;

                    for(int l = 0; l < (_container.Triangles.Count / 3); l++)
                    {
                        bool cut = PlainVertices(_container.Triangles[i], _container.Triangles[i+1], _container.Triangles[i+2], core.Position, core.Velocity);
                        if(cut)
                        {

                        }
                    }

                    core.SetPosition(core.Position + core.Velocity);
                    core.Position += core.Velocity;
                }
                i++;
            }
        }

        private float CalculateDensity(List<Core> coreL, Core core, float h)
        {
            var result = _restingDensity;
            foreach (Core c in coreL)
            {
                var r = core.Position - c.Position;
                if (!c.Equals(core))
                    result += (float)(c.Mass * W(r,h));
            }
            return result;
        }

        private Vector3 CalculatePressure(List<Core> cores, Core core, float h, float pressure, float density)
        {
            var result = new Vector3();
            foreach (var c in cores)
            {
                Vector3 r = core.Position - c.Position;
                if (!c.Equals(core) && r.Length != 0)
                {
                    result += c.Mass * (core.Pressure + c.Pressure) / (2 * c.Density) * WPress(r, h) * (r / r.Length);
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
                    result += c.Mass * (c.Velocity - core.Velocity) / c.Density * WVis(r,h);
                }
            }
            result = result * _coreCloud.Viscosity;
            return result;
        }

        private bool PlainVertices(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 sPoint, Vector3 sDirection)
        {
            return PlainCrossed(CrossProduct(p2 - p1, p3 - p1), sPoint, sDirection, p1);
        }

        private bool PlainCrossed(Vector3 n, Vector3 sPoint, Vector3 sDirection, Vector3 a)
        {
            if (VectorMult(n, sDirection) == 0)
                return false;
            _lambda = -VectorMult(n,sPoint) / VectorMult(n,sDirection) + VectorMult(n,a);
            Vector3 sPointcPoint = _lambda * sDirection;
            if (_lambda <= 1)
                if (_lambda >= 0)
                    return true;
            return false;
        }

        private float VectorMult(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        private Vector3 CrossProduct(Vector3 v1, Vector3 v2)
        {
            Vector3 result = new Vector3();
            result.X = v1.Y * v2.Z - v1.Z * v2.Y;
            result.Y = v1.Z * v2.X - v1.X * v2.Z;
            result.Z = v1.X * v2.Y - v1.Y * v2.X;
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

        private float W(Vector3 r, float h)
        {
            if (r.Length == h)
                return 0;
            if (0 <= r.Length && r.Length <= h)
                return (float)(_WMagic * (Math.Pow(h * h - r.Length * r.Length, 3)));
            else
                return 0;
        }

        private float WPress(Vector3 r, float h)
        {
            if (r.Length == h)
                return 0;
            if (0 <= r.Length && r.Length <= h)
                return (float)(_WPresMagic * Math.Pow(h - r.Length, 3));
            else
                return 0;
        }

        private float WVis(Vector3 r, float h)
        {
            if (0 <= r.Length && r.Length <= h)
                return (float)(15 / (2 * Math.PI * h * h * h) - (r.Length * r.Length * r.Length) / (2 * h * h * h) + (r.Length * r.Length) / (h * h) + h / (2 * r.Length) - 1);
            else
                return 0;
        }
    }
}
