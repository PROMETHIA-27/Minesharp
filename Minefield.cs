namespace Minesharp
{
    /// <summary>
    /// A logical representation of a minefield for the purposes of minesweeper, which also can be rendered
    /// </summary>
    public struct Minefield : IRenderable
    {
        Tile[,] data;
        /// <summary>
        /// Settings blob for the minefield
        /// </summary>
        public MinefieldSettings Settings;

        static readonly Point[] AdjacentPoints = { new(-1, 1), new(0, 1), new(1, 1),
                                                   new(-1, 0),            new(1, 0),
                                                   new(-1, -1),new(0, -1),new(1, -1) };
        
        /// <summary>
        /// Create a new minefield of a specific width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Minefield(int width, int height)
        {
            this.data = new Tile[width, height];
            this.Settings = new();
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Get the tile at a position in this field
        /// </summary>
        /// <param name="p">Position to get tile at</param>
        /// <returns>Minefield tile</returns>
        public ref Tile this[Point p] => ref this.data[p.x, p.y];

        /// <summary>
        /// Get the tile at a position in this field
        /// </summary>
        /// <param name="x">X coordinate of the position to get tile at</param>
        /// <param name="y">Y coordinate of the position to get tile at</param>
        /// <returns>Minefield tile</returns>
        public ref Tile this[int x, int y] => ref this.data[x, y];

        /// <summary>
        /// Width of the minefield
        /// </summary>
        public int Width { get; init; }
        /// <summary>
        /// Height of the minefield
        /// </summary>
        public int Height { get; init; }

        /// <summary>
        /// Use a random number generator to get a randomized point, constrained to the size of this minefield
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        public Point GetRandomPoint(Random rand)
            => new() { x = rand.Next(0, this.Width), y = rand.Next(0, this.Height) };

        /// <summary>
        /// Use a random number generator to get a randomized point, constrained to a particular size
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="maxHeight"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static Point GetRandomPoint(Random rand, int maxWidth, int maxHeight)
            => new() { x = rand.Next(0, maxWidth), y = rand.Next(0, maxHeight) };

        /// <summary>
        /// Get the number of mines directly bordering a tile at a point, including diagonals
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int GetAdjacentMines(Point point)
        {
            int mines = 0;
            var bounds = this.GetBounds();
            for (int i = 0; i < AdjacentPoints.Length; i++)
            {
                var checkPoint = point + AdjacentPoints[i];
                if (bounds.ContainsPoint(checkPoint))
                    mines += ((this[checkPoint].state & TileState.Mine) != 0) ? 1 : 0;
            }
            return mines;
        }

        /// <summary>
        /// Fills an array with points adjacent to a given point that match a certain criteria, given by the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="points"></param>
        /// <returns>The number of points returned in the array</returns>
        public int GetAdjacentPointsWhere(Point point, Func<Tile, bool> predicate, Point[] points)
        {
            int lastIdx = 0;
            var bounds = this.GetBounds();
            for (int i = 0; i < AdjacentPoints.Length; i++)
            {
                var adjPoint = point + AdjacentPoints[i];
                if (bounds.ContainsPoint(adjPoint) && predicate(this[adjPoint]))
                {
                    points[lastIdx] = adjPoint;
                    lastIdx++;
                }
            }
            return lastIdx;
        }

        // In edge cases may create colossal garbage, but at least it's one giant blob so it should be fine. Faster than trying to pool a 100mil long array
        /// <summary>
        /// Resets the minefield, allowing a new size to be set
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ResetMinefield(int width, int height)
            => this.data = new Tile[width, height];

        /// <summary>
        /// Randomly places remainder of mines around the board. Should be called before any chunks are randomized
        /// </summary>
        /// <param name="rand"></param>
        public void RandomizeRemainder(Random rand)
        {
            // Calculate exactly how many mines are being automatically placed via RandomizeChunk(), assuming it's called on every chunk eventually, and randomly place
            // remaining mines across the board.
            long mineCount = this.Settings.MineCount;
            int width = this.Width;
            int height = this.Height;
            long area = width * height;
            long wholeWidth = width / chunkSize;
            long wholeHeight = height / chunkSize;
            long wholeArea = wholeWidth * wholeHeight;
            long mineCountPerWholeChunk = (long)(mineCount * (chunkSize * chunkSize / (double)area));
            long wholeMines = mineCountPerWholeChunk * wholeArea;
            double edgeWidth = ((double)width / chunkSize) - wholeWidth;
            double edgeHeight = ((double)height / chunkSize) - wholeHeight;
            long mineCountPerWidthEdgeChunk = (long)(mineCount * (edgeWidth * chunkSize * chunkSize / area));
            long widthEdgeMines = mineCountPerWidthEdgeChunk * wholeHeight;
            long mineCountPerHeightEdgeChunk = (long)(mineCount * (edgeHeight * chunkSize * chunkSize / area));
            long heightEdgeMines = mineCountPerHeightEdgeChunk * wholeWidth;
            long mineCountPerCornerChunk = (long)(mineCount * (edgeHeight * edgeWidth * chunkSize * chunkSize / area));
            long totalMinesPlaced = wholeMines + widthEdgeMines + heightEdgeMines + mineCountPerCornerChunk;
            long remainder = mineCount - totalMinesPlaced;

            for (int i = 0; i < remainder; i++)
            {
                var point = GetRandomPoint(rand, width, height);
                while ((this[point].state & TileState.Mine) != 0)
                    point = GetRandomPoint(rand, width, height);
                this[point].state |= TileState.Mine;
            }
        }

        /// <summary>
        /// Width and height of chunks for purposes of dividing the minefield for rapid generation
        /// </summary>
        public const int chunkSize = 256;
        /// <summary>
        /// Randomize a region of the minefield. Is designed to be lazily evaluated when the player moves their cursor into an unevaluated chunk.
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="chunk"></param>
        public void RandomizeChunk(Random rand, Point chunk)
        {
            int width = this.Width;
            int height = this.Height;

            Point chunkOrigin = chunkSize * chunk;
            int chunkWidth = Math.Min(width - chunkOrigin.x, chunkSize);
            int chunkHeight = Math.Min(height - chunkOrigin.y, chunkSize);

            long area = width * height;
            long mineCountPerChunk = (long)(this.Settings.MineCount * (chunkWidth * chunkHeight / (double)area));
            mineCountPerChunk = Math.Min(mineCountPerChunk, chunkWidth * chunkHeight);

            for (int i = 0; i < chunkWidth; i++)
                for (int j = 0; j < chunkHeight; j++)
                    this[chunkOrigin + new Point(i, j)].state &= ~(TileState.Flagged | TileState.Revealed);

            for (long i = 0; i < mineCountPerChunk; i++)
            {
                var point = GetRandomPoint(rand, chunkWidth, chunkHeight);
                while ((this[chunkOrigin + point].state & TileState.Mine) != 0)
                    point = GetRandomPoint(rand, chunkWidth, chunkHeight);
                this[chunkOrigin + point].state |= TileState.Mine;
            }
        }

        /// <summary>
        /// Reveal all 0-tiles connected to the given tile, as well as n-tiles connected directly to a revealed tile
        /// </summary>
        /// <param name="p">0-tile to reveal</param>
        /// <returns>True if a mine is revealed</returns>
        public bool FloodReveal(Point p)
        {
            HashSet<Point> blacklist = new();
            Queue<Point> openSet = new();
            Point[] adjacents = new Point[8];
            if (this.GetAdjacentMines(p) == 0)
                openSet.Enqueue(p);
            else if (this.RevealTile(p))
                return true;

            while (openSet.Count > 0)
            {
                var point = openSet.Dequeue();
                this.RevealTile(point);
                var adjCount = this.GetAdjacentPointsWhere(point, t => (t.state & TileState.Mine) == 0, adjacents);
                for (int i = 0; i < adjCount; i++)
                {
                    var adjPoint = adjacents[i];
                    if (this.GetAdjacentMines(adjPoint) == 0 && blacklist.Add(adjPoint))
                    {
                        openSet.Enqueue(adjPoint);
                    }
                    else
                        this.RevealTile(adjPoint);
                }
            }
            return false;
        }

        /// <summary>
        /// Find which chunk of the board a point lies in
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point PointToChunk(Point point)
        {
            var x = point.x / chunkSize;
            var y = point.y / chunkSize;
            return new(x, y);
        }

        /// <summary>
        /// Set a tile's state to flagged
        /// </summary>
        /// <param name="pos"></param>
        public void ToggleTileFlag(Point pos) => 
            this[pos].state ^= TileState.Flagged;

        /// <summary>
        /// Reveal a tile, and return true if it's a mine
        /// </summary>
        /// <param name="pos">Position to reveal</param>
        /// <returns>True if the revealed tile is a mine</returns>
        public bool RevealTile(Point pos)
        {
            var state = this[pos].state;
            state |= TileState.Revealed;
            this[pos].state = state;
            return (state & TileState.Mine) != 0;
        }

        /// <summary>
        /// Get the bounds of this minefield as a renderable object
        /// </summary>
        /// <returns></returns>
        public Bounds GetBounds() => new(new(0, 0), new(this.Width - 1, this.Height - 1));

        static Color minefieldBgColor = Color.LightGrey;
        /// <summary>
        /// Gets the displayable tile at a certain point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DisplayTile GetDisplayTile(Point p)
        {
            var state = this[p].state;
            if ((state & TileState.Flagged) != 0)
                return new('x', Color.Red, minefieldBgColor);
            else if ((state & TileState.Revealed) != 0)
            {
                if ((state & TileState.Mine) != 0)
                    return new('m', Color.Red, minefieldBgColor);
                else
                {
                    var adj = this.GetAdjacentMines(p);
                    Color numCol = adj switch
                    {
                        0 => Color.LightGrey,
                        1 => Color.BrighterBlue,
                        2 => Color.DarkerGreen,
                        3 => Color.Red,
                        4 => Color.Blue,
                        5 => Color.DarkerRed,
                        6 => Color.DarkerCyan,
                        7 => Color.Black,
                        _ => Color.White,
                    };
                    return new((char)(48 + adj), numCol, minefieldBgColor);
                }       
            }
            else
                return new('\u25A1', Color.White, minefieldBgColor);
        }
    }
}
