namespace Minesharp
{
    /// <summary>
    /// Object for rendering to console efficiently using VT console commands
    /// </summary>
    public struct ConsoleRenderer
    {
        int viewWidth;
        int viewHeight;

        DisplayTile[,] view;
        Point viewCursor;

        /// <summary>
        /// The buffer the renderer uses to output to console
        /// </summary>
        public readonly StreamWriter buffer;

        /// <summary>
        /// Creates a new console renderer
        /// </summary>
        /// <param name="width">Width of the display view</param>
        /// <param name="height">Height of the display view</param>
        public ConsoleRenderer(int width = 25, int height = 15)
        {
            this.buffer = new(Console.OpenStandardOutput());
            this.buffer.AutoFlush = false;
            this.viewWidth = width;
            this.viewHeight = height;
            this.view = new DisplayTile[width, height];
            this.viewCursor = new();
        }

        /// <summary>
        /// Change the size of the display view
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ResizeBackendBuffer(int width, int height)
        {
            this.viewWidth = width;
            this.viewHeight = height;
            this.view = new DisplayTile[this.viewWidth, this.viewHeight];
        }

        /// <summary>
        /// Move the cursor used for writing to the view
        /// </summary>
        /// <param name="position">The position to set the cursor to</param>
        public void MoveViewCursor(Point position)
            => this.viewCursor = position;

        /// <summary>
        /// Write a displaytile into the view where the viewcursor is
        /// </summary>
        /// <param name="tile">DisplayTile to write</param>
        public void WriteToView(DisplayTile tile)
        {
            this.view[this.viewCursor.x, this.viewCursor.y] = tile;
            this.viewCursor.x++;
            if (this.viewCursor.x >= this.viewWidth)
                this.viewCursor = new(0, this.viewCursor.y + 1);
            if (this.viewCursor.y >= this.viewHeight)
                this.viewCursor = new(0, 0);
        }

        /// <summary>
        /// Move the view cursor ahead as if writing to the view, but writing nothing
        /// </summary>
        public void SkipCursor()
        {
            this.viewCursor.x++;
            if (this.viewCursor.x >= this.viewWidth)
                this.viewCursor = new(0, this.viewCursor.y + 1);
            if (this.viewCursor.y >= this.viewHeight)
                this.viewCursor = new(0, 0);
        }

        static readonly Dictionary<Color, string> colorCache = new();
        /// <summary>
        /// Write a color code to the given console buffer
        /// </summary>
        /// <param name="buffer">Console buffer to write a color code to</param>
        /// <param name="foreground">Whether the color is in the foreground or background</param>
        /// <param name="color">Color to write</param>
        public static void WriteColorToBuffer(StreamWriter buffer, bool foreground, Color color)
        {
            if (foreground)
                buffer.Write("\x1b[38;2;"); // Beginning of "set foreground color"
            else
                buffer.Write("\x1b[48;2;"); // Beginning of "set background color"
            if (colorCache.TryGetValue(color, out var colorString))
                buffer.Write(colorString);
            else
            {
                var str = $"{color.R};{color.G};{color.B}m"; // End of set color
                colorCache.Add(color, str);
                buffer.Write(str);
            }
        }

        /// <summary>
        /// Render a renderable object to the view, allowing it to be rendered to the console when Render() is called, assuming it is not drawn over later
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="renderable"></param>
        public void Draw<T>(T renderable)
            where T : IRenderable
        {
            var origin = renderable.GetBounds().UpperLeftCorner;
            if (!this.GetViewBounds().ContainsPoint(origin))
                return;
            this.MoveViewCursor(origin);
            long maxWidth = Math.Min(renderable.GetWidth(), this.viewWidth - 1);
            long maxHeight = Math.Min(renderable.GetHeight(), this.viewHeight - 1);
            for (int j = 0; j < maxHeight; j++)
            {
                for (int i = 0; i < maxWidth; i++)
                {
                    var display = renderable.GetDisplayTile(i, j);
                    if (display.character == '\0')
                    {
                        this.SkipCursor();
                        continue;
                    }
                    this.WriteToView(display);
                }
                this.MoveViewCursor(new(0, this.viewCursor.y + 1));
            }
        }

        /// <summary>
        /// Get the bounds of the view of this renderer
        /// </summary>
        /// <returns></returns>
        public Bounds GetViewBounds()
        {
            Point bottomCorner = new(Math.Min(this.viewWidth, Console.WindowWidth), Math.Min(this.viewHeight, Console.WindowHeight));
            return new(new(0, 0), bottomCorner);
        }

        /// <summary>
        /// Render the view to the console
        /// </summary>
        public void Render()
        {
            var lastFgColor = this.view[0, 0].fgColor;
            var lastBgColor = this.view[0, 0].bgColor;
            this.buffer.Write("\x001b7"); // Save cursor position to memory
            this.buffer.Write("\x1b[0;0H"); // Reset cursor to 0, 0 (Really it's 1, 1 I think)
            this.buffer.Write("\x1b[?25l"); // Disable cursor visibility
            WriteColorToBuffer(this.buffer, true, lastFgColor);
            WriteColorToBuffer(this.buffer, false, lastBgColor);
            var bounds = this.GetViewBounds();
            for (int j = 0; j < bounds.LowerRightCorner.y; j++)
            {
                for (int i = 0; i < bounds.LowerRightCorner.x; i++)
                {
                    var display = this.view[i, j];
                    if (display.character == '\0')
                        display.character = ' ';
                    if (display.fgColor != lastFgColor)
                    {
                        WriteColorToBuffer(this.buffer, true, display.fgColor);
                        lastFgColor = display.fgColor;
                    }
                    if (display.bgColor != lastBgColor)
                    {
                        WriteColorToBuffer(this.buffer, false, display.bgColor);
                        lastBgColor = display.bgColor;
                    }
                    this.buffer.Write(display.character);
                }
                if (j != bounds.LowerRightCorner.y - 1) 
                    this.buffer.Write(Environment.NewLine);
            }
            this.buffer.Write("\x1b[0m"); // Reset colors to black/white
            this.buffer.Write("\x001b8"); // Load cursor value from memory
            this.buffer.Flush();
        }
    }
}
