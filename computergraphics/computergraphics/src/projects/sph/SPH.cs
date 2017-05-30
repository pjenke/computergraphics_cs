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
        
        private const float CubeSizeX = 2f,
                            CubeSizeY = 2f,
                            CubeSizeZ = 2f;

        private readonly CoreCloud _coreCloud;

        private readonly List<RasterUnit> _raster;

        private readonly List<Core> _coreList;

        private readonly int _cubeNumX;

        private readonly int _cubeNumZ;

        private readonly int _cubeNumY;

        private readonly float _restingDensity;

        private float _lambda;

        private Vector3 _normal;

        private readonly float _WMagic, _WPresMagic, _WVisMagic;

        private readonly String _Path = @"C:\Users\Maxi\Source\Repos\computergraphics_cs\Save.txt";

        private const float _StepSize = 0.001f, k = 0.06f;

        private readonly LeafNode _container;

        private bool _saveCalc = false;

        public Sph(CoreCloud coreCloud, LeafNode container, bool save)
        {
            _saveCalc = save;
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
            _WPresMagic = (float)(-45 / (Math.PI * Math.Pow(coreCloud.H, 6)));
            _WVisMagic = (float)(45 / (Math.PI * Math.Pow(coreCloud.H, 6)));
            _restingDensity = _coreList[0].Mass;
            _raster = new List<RasterUnit>();
            Initialize();
            if (_saveCalc)
            {
                if (System.IO.File.Exists(_Path))
                    System.IO.File.Delete(_Path);
                System.IO.File.AppendAllLines(_Path, new string[] {coreCloud.Cores.Count.ToString()});
            }
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
                    core.Velocity += velocity;
                    float lengthLeft = 1f, lambda = 2f;
                    Vector3 strahl = _StepSize * core.Velocity, pos = core.Position, normal = Vector3.Zero;
                    bool cut = false;
                    do
                    {
                        cut = ContainerCut(pos, strahl * lengthLeft);
                        lambda = _lambda;
                        normal = _normal;
                        if(cut)
                        {
                            if(lambda != 0)
                            {
                                lengthLeft -= (lengthLeft / 100) * (lambda * 100);
                                Vector3 newPos = pos + strahl * lambda;
                                Vector3 newStrahl = (2 * ((strahl * -1) * (normal * -1)) * (normal * -1) - (strahl * -1)) * lengthLeft;
                                bool secCut = ContainerCut(newPos, newStrahl);
                                if (_lambda == 0)
                                    secCut = false;
                                if (secCut)
                                {
                                    pos = newPos;
                                    strahl = (2 * ((strahl * -1) * (normal * -1)) * (normal * -1) - (strahl * -1));
                                }
                                else if(InBoundCheck(newPos + newStrahl))
                                {
                                    pos = newPos;
                                    strahl = (2 * ((strahl * -1) * (normal * -1)) * (normal * -1) - (strahl * -1));
                                }
                                else if(InBoundCheck((pos + strahl * lambda) + (strahl * -1 * lengthLeft)))
                                {
                                    pos = pos + strahl * lambda;
                                    strahl *= -1;
                                }
                                else if (InBoundCheck(pos + new Vector3(strahl.X, 0, strahl.Z) * lengthLeft))
                                {
                                    strahl = new Vector3(strahl.X, 0, strahl.Z);
                                }
                                else
                                {
                                    pos = pos + strahl * lambda;
                                    strahl = Vector3.Zero;
                                }
                            }
                            else
                            {
                                cut = false;
                                if (InBoundCheck(pos + strahl * lengthLeft))
                                {
                                    
                                }
                                else if(InBoundCheck(pos + (2 * (strahl * (normal * -1)) * (normal * -1) - strahl) * lengthLeft))
                                { 
                                    strahl = (2 * (strahl * (normal * -1)) * (normal * -1) - strahl);
                                }
                                else if(InBoundCheck(pos + new Vector3(strahl.X, 0, strahl.Z) * lengthLeft))
                                {
                                    strahl = new Vector3(strahl.X, 0, strahl.Z);
                                }
                                else
                                {
                                    strahl = Vector3.Zero;
                                }
                            }
                        }
                    } while (cut);

                    if (InBoundCheck(pos + strahl * lengthLeft))
                    {
                        core.SetPosition(pos + strahl * lengthLeft);
                        core.Position = pos + strahl * lengthLeft;
                        core.Velocity = strahl / _StepSize;
                    }
                    else if (InBoundCheck(pos + (strahl * -1) * lengthLeft))
                    {
                        core.SetPosition(pos + (strahl * -1) * lengthLeft);
                        core.Position = pos + (strahl * -1) * lengthLeft;
                        core.Velocity = strahl / _StepSize * -1;
                    }
                    else if(InBoundCheck(pos + new Vector3(strahl.X,0,strahl.Z) * lengthLeft))
                    {
                        core.SetPosition(pos + new Vector3(strahl.X, 0, strahl.Z) * lengthLeft);
                        core.Position = pos + new Vector3(strahl.X, 0, strahl.Z) * lengthLeft;
                        core.Velocity = new Vector3(strahl.X / _StepSize, 0, strahl.Z / _StepSize);
                    }
                    else
                    {
                        core.Velocity = Vector3.Zero;
                    }

                    if (_saveCalc)
                    {
                        string[] write = new string[] { core.Position.X.ToString(), core.Position.Y.ToString(), core.Position.Z.ToString()};
                        System.IO.File.AppendAllLines(_Path, write);
                    }
                }
                i++;
            }
        }

        private bool ContainerCut(Vector3 pos, Vector3 strahl)
        {
            Vector3 tmpNormal = Vector3.Zero;
            float tmpLambda = 2f;
            for (int l = 0; l < (_container.Triangles.Count / 3); l++)
            {
                bool cut = PlainVertices(_container.Triangles[3 * l], _container.Triangles[3 * l + 1], _container.Triangles[3 * l + 2], pos, strahl);
                if (cut)
                {
                    if (_lambda != 0)
                    {
                        if (tmpLambda > _lambda || tmpLambda == 0)
                        {
                            tmpLambda = _lambda;
                            tmpNormal = _normal;
                        }
                    }
                    else
                    {
                        if (tmpLambda == 2)
                        {
                            tmpLambda = _lambda;
                            tmpNormal = _normal;
                        }
                    }
                }
            }
            if(tmpLambda != 2)
            {
                _lambda = tmpLambda;
                _normal = tmpNormal;
                return true;
            }
            return false;
        }

        private bool InBoundCheck(Vector3 pos)
        {
            for(int i = 0; i < _container.Triangles.Count / 3; i++)
            {
                Vector3 normal = Vector3.Cross(_container.Triangles[i * 3 + 1] - _container.Triangles[i * 3], _container.Triangles[i * 3 + 2] - _container.Triangles[i * 3]).Normalized();
                float oob = VectorMult(pos, normal) - VectorMult(normal, _container.Triangles[i * 3]);
                if (oob < 0)
                    return false;
            }
            return true;
        }

        private float CalculateDensity(List<Core> coreL, Core core, float h)
        {
            var result = _restingDensity;
            foreach (Core c in coreL)
            {
                var r = core.Position - c.Position;
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
                    result += c.Mass * (core.Pressure + c.Pressure) / (core.Density * core.Density + c.Density * c.Density) * WPress(r, h) * r.Normalized();
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
                    result += c.Mass * ((c.Velocity - core.Velocity) / c.Density) * WVis(r,h);
                }
            }
            result = result * (_coreCloud.Viscosity / core.Density);
            return result;
        }

        private bool PlainVertices(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 sPoint, Vector3 sDirection)
        {
            return PlainCrossed(Vector3.Cross(p3 - p1, p2 - p1).Normalized(), sPoint, sDirection, p1);
        }

        private bool PlainCrossed(Vector3 n, Vector3 sPoint, Vector3 sDirection, Vector3 a)
        {
            if (VectorMult(n, sDirection) == 0)
                return false;
            _lambda = (-VectorMult(n,sPoint) + VectorMult(n,a)) / VectorMult(n, sDirection);
            if (_lambda <= 1)
                if (_lambda >= 0)
                {
                    _normal = n;
                    return true;
                }
            return false;
        }

        private float VectorMult(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
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
                return (float)(_WPresMagic * Math.Pow(h - r.Length, 2));
            else
                return 0;
        }

        private float WVis(Vector3 r, float h)
        {
            if (0 <= r.Length && r.Length <= h)
                return (float)(_WVisMagic * (h - r.Length));
            else
                return 0;
        }
    }
}
