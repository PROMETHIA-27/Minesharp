namespace Minesharp
{
    /// <summary>
    /// 
    /// </summary>
    public struct Background : IRenderable
    {
        /// <summary>
        /// 
        /// </summary>
        public Bounds Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Bounds GetBounds() => this.Size;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DisplayTile GetDisplayTile(Point p) => new(' ', default, this.Color);
    }
}
