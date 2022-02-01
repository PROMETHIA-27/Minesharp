namespace Minesharp
{
    /// <summary>
    /// A settings object for the minefield
    /// </summary>
    public struct MinefieldSettings
    {
        /// <summary>
        /// The fewest mines that can be placed on generation of the minefield
        /// </summary>
        public long MineMin;
        /// <summary>
        /// The most mines that can be placed on generation of the minefield
        /// </summary>
        public long MineMax;
        /// <summary>
        /// The current amount of mines that will be placed during generation of the minefield
        /// </summary>
        public long MineCount;

        /// <summary>
        /// Calculate the number of mines that will be placed on the minefield, assinging it to MineCount and returning it
        /// </summary>
        /// <param name="rand">A random generator to use to determine number of mines to place</param>
        /// <returns>The number of mines to place</returns>
        public long CalculateMineCount(Random rand)
        {
            this.MineCount = rand.NextInt64(this.MineMin, this.MineMax);
            return this.MineCount;
        }
    }
}
