namespace Minesharp
{
    /// <summary>
    /// A renderable object to represent the ability to pan left, right, up, and down the minefield
    /// </summary>
    public struct PanningBorder : IRenderable
    {
        const char leftArrow = '\u2190';
        const char upArrow = '\u2191';
        const char rightArrow = '\u2192';
        const char downArrow = '\u2193';

        /// <summary>
        /// The bounds of the border on the view
        /// </summary>
        public Bounds Bounds;
        
        /// <summary>
        /// Create a new border from a bounds
        /// </summary>
        /// <param name="bounds"></param>
        public PanningBorder(Bounds bounds)
            => this.Bounds = bounds;
        
        /// <summary>
        /// Convert a point to a char within the border
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public char PointToChar(Point p)
        {
            if (p.x == this.Bounds.UpperLeftCorner.x)
                return leftArrow;
            else if (p.x == this.Bounds.LowerRightCorner.x)
                return rightArrow;
            else if (p.y == this.Bounds.UpperLeftCorner.y)
                return upArrow;
            else if (p.y == this.Bounds.LowerRightCorner.y)
                return downArrow;
            else
                return '\0';
        }

        /// <summary>
        /// Return the bounds of this border
        /// </summary>
        /// <returns></returns>
        public Bounds GetBounds() => this.Bounds;

        /// <summary>
        /// Create a display tile from a point on this border
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DisplayTile GetDisplayTile(Point p) => new(this.PointToChar(p), Color.White);
    }
}
