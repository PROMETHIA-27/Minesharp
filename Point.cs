namespace Minesharp
{
    /// <summary>
    /// A 2D point in space
    /// </summary>
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// Construct a new point from two coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(int x, int y)
            => (this.x, this.y) = (x, y);

        /// <summary>
        /// The x coordinate
        /// </summary>
        public int x;
        /// <summary>
        /// The y coordinate
        /// </summary>
        public int y;

        /// <summary>
        /// Add two points together, summing their coordinates
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns>Sum of the two points</returns>
        public static Point operator +(Point lhs, Point rhs) => new(lhs.x + rhs.x, lhs.y + rhs.y);
        /// <summary>
        /// Multiply a point by a scalar value, multiplying each of its coordinates
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns>A multiplied point</returns>
        public static Point operator *(Point lhs, int rhs) => new(lhs.x * rhs, lhs.y * rhs);
        /// <summary>
        /// Multiply a point by a scalar value, multiplying each of its coordinates
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns>A multiplied point</returns>
        public static Point operator *(int lhs, Point rhs) => new(lhs * rhs.x, lhs * rhs.y);
        /// <summary>
        /// Check if two points are exactly equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Point other) => this.x == other.x && this.y == other.y;
    }
}
