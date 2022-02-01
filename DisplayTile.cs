namespace Minesharp
{
    /// <summary>
    /// A struct containing information for displaying to console; a character, and foreground and background colors.
    /// </summary>
    public struct DisplayTile
    {
        /// <summary>
        /// Create a new display tile from a character, with white on black foreground/background colors
        /// </summary>
        /// <param name="character"></param>
        public DisplayTile(char character)
            => (this.character, this.fgColor, this.bgColor, this._padding) = (character, Color.White, Color.Black, default);

        /// <summary>
        /// Create a new display tile from a character, with a certain foreground color and optional background color, defaulting to black
        /// </summary>
        /// <param name="character"></param>
        /// <param name="fgColor"></param>
        /// <param name="bgColor"></param>
        public DisplayTile(char character, Color fgColor, Color bgColor = default)
            => (this.character, this.fgColor, this.bgColor, this._padding) = (character, fgColor, bgColor, default);

        /// <summary>
        /// The character that a DisplayTile displays to the console
        /// </summary>
        public char character;
        /// <summary>
        /// The foreground color of a DisplayTile
        /// </summary>
        public Color fgColor;
        /// <summary>
        /// The background color of a DisplayTile
        /// </summary>
        public Color bgColor;
        private readonly char _padding; // Round the struct to 8 bytes
    }
}
