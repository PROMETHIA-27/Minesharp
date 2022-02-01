namespace Minesharp
{
    /// <summary>
    /// Represents a renderable cursor, to replace the default console cursor which should be hidden
    /// </summary>
    public struct ConsoleCursor : IRenderable
    {
        /// <summary>
        /// Create a new cursor with a certain character and default colors
        /// </summary>
        /// <param name="character"></param>
        public ConsoleCursor(char character)
        {
            this.Character = character;
            this.Position = new();
            this.FgColor = Color.White;
            this.BgColor = Color.Black;
        }

        /// <summary>
        /// Create a new cursor with a certain character and colors
        /// </summary>
        /// <param name="character"></param>
        /// <param name="fgColor"></param>
        /// <param name="bgColor"></param>
        public ConsoleCursor(char character, Color fgColor, Color bgColor)
        {
            this.Character = character;
            this.Position = new();
            this.FgColor = fgColor;
            this.BgColor = bgColor;
        }

        /// <summary>
        /// The character to display this cursor as
        /// </summary>
        public char Character { get; set; }
        /// <summary>
        /// The position to display this cursor at
        /// </summary>
        public Point Position { get; set; }
        /// <summary>
        /// The foreground color of the cursor
        /// </summary>
        public Color FgColor { get; set; }
        /// <summary>
        /// The background color of the cursor
        /// </summary>
        public Color BgColor { get; set; }

        /// <summary>
        /// Get the bounds of this cursor on the screen
        /// </summary>
        /// <returns></returns>
        public Bounds GetBounds() => new(this.Position, this.Position);
        /// <summary>
        /// Get the displayable tile of this cursor
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DisplayTile GetDisplayTile(Point p) => new(this.Character, this.FgColor, this.BgColor);
    }
}
