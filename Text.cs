namespace Minesharp
{
    class Text : IRenderable
    {
        public string String = "";

        public Bounds GetBounds()
            => new(new(35, 3), new(50, 3));

        public DisplayTile GetDisplayTile(Point p) 
            => new(p.x < this.String.Length ? this.String[p.x] : '\0', Color.White, Color.Black);
    }
}
