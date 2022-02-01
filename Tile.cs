namespace Minesharp
{
    /// <summary>
    /// A representation of a minesweeper tile
    /// </summary>
    public struct Tile
    {
        /// <summary>
        /// Create a new tile from a state bitflag
        /// </summary>
        /// <param name="state"></param>
        public Tile(TileState state)
            => this.state = state;

        /// <summary>
        /// Bitflag enum representing the state of the tile
        /// </summary>
        public TileState state;
    }

    /// <summary>
    /// A bitflag enum representing the state of a minesweeper tile
    /// </summary>
    [Flags]
    public enum TileState : byte
    {
        /// <summary>
        /// The tile is not a mine, flagged, or revealed
        /// </summary>
        None = 0,
        /// <summary>
        /// The tile holds a mine
        /// </summary>
        Mine = 1,
        /// <summary>
        /// The tile is flagged
        /// </summary>
        Flagged = 2,
        /// <summary>
        /// The tile is revealed
        /// </summary>
        Revealed = 4,
    }
}
