namespace Minesharp
{
    /// <summary>
    /// A struct representing the upper left corner and lower right corner of a renderable object in the console
    /// </summary>
    public struct Bounds
    {
        /// <summary>
        /// Construct a new bounds from two points
        /// </summary>
        /// <param name="upperLeft"></param>
        /// <param name="lowerRight"></param>
        public Bounds(Point upperLeft, Point lowerRight)
            => (this.UpperLeftCorner, this.LowerRightCorner) = (upperLeft, lowerRight);

        /// <summary>
        /// The upper left corner
        /// </summary>
        public Point UpperLeftCorner;
        /// <summary>
        /// The lower right corner
        /// </summary>
        public Point LowerRightCorner;
    }
}
