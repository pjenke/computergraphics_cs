namespace computergraphics.projects.sph.Datastructures
{
    using System.Collections.Generic;
    using System.Linq;

    public class RasterUnit
    {
        public RasterUnit(int x, int y, int z)
        {
            Cores = new List<Core>();
            Position = new Position { X = x, Y = y, Z = z };
        }

        public void Initialize(IEnumerable<RasterUnit> raster)
        {
            Neighbors = FindNeighborsOfRasterUnit(raster).ToList();
        }

        public List<Core> Cores { get; }

        public List<RasterUnit> Neighbors { get; private set; }

        public Position Position { get; }

        public bool IsPositionInUnit(Position position)
        {
            return Position.Equals(position);
        }

        private IEnumerable<RasterUnit> FindNeighborsOfRasterUnit(IEnumerable<RasterUnit> raster)
        {
            foreach (var unit in raster)
            {
                var position = unit.Position;
                if (!Equals(unit) && Position.IsPositionNextTo(position))
                {
                    yield return unit;
                }
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as RasterUnit;

            if (other == null)
            {
                return false;
            }

            return other.Position.Equals(Position) && other.Cores.Equals(Cores);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}