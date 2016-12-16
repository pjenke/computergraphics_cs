namespace computergraphics.projects.sph.Datastructures
{
    public class Position
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public bool IsPositionNextTo(Position other)
        {
            return IsNextTo(X, other.X) && IsNextTo(Y, other.Y) && IsNextTo(Z, other.Z);
        }

        private bool IsNextTo(int coordinate, int coordinateNextTo)
        {
            return coordinateNextTo == coordinate || coordinateNextTo == coordinate + 1 || coordinateNextTo == coordinate - 1;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Position;

            return other != null && other.X == X && other.Y == Y && other.Z == Z;
        }

        public override string ToString()
        {
            return $"X={X} Y={Y} Z={Z}";
        }
    }
}