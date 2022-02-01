namespace Minesharp
{
    /// <summary>
    /// Interface for objects that can be rendered to the console
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Gets a bounds struct representing the upper left corner and lower right corner of the object in the console coordinates
        /// </summary>
        /// <returns></returns>
        public Bounds GetBounds();

        /// <summary>
        /// Gets a display tile at a certain point from this renderable object
        /// </summary>
        /// <param name="p">The point to render</param>
        /// <returns></returns>
        public DisplayTile GetDisplayTile(Point p);

        /// <summary>
        /// Gets a display tile at a certain point from this renderable object
        /// </summary>
        /// <param name="x">The x coordinate of the point to render</param>
        /// <param name="y">The y coordinate of the point to render</param>
        /// <returns></returns>
        public DisplayTile GetDisplayTile(int x, int y)
            => this.GetDisplayTile(new(x, y));
    }
}
