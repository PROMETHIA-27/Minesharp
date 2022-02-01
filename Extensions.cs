namespace Minesharp
{
    /// <summary>
    /// A class of extensions for various objects around the Minesharp codebase
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Get the total width of a renderable object based on its bounds
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="renderable"></param>
        /// <returns></returns>
        public static long GetWidth<T>(this T renderable)
            where T : IRenderable
        {
            var bounds = renderable.GetBounds();
            return bounds.LowerRightCorner.x - bounds.UpperLeftCorner.x + 1;
        }

        /// <summary>
        /// Get the total height of a renderable object based on its bounds
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="renderable"></param>
        /// <returns></returns>
        public static long GetHeight<T>(this T renderable)
            where T : IRenderable
        {
            var bounds = renderable.GetBounds();
            return bounds.LowerRightCorner.y - bounds.UpperLeftCorner.y + 1;
        }

        /// <summary>
        /// Returns whether or not a point lies in a bounds
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool ContainsPoint(this Bounds bounds, Point p)
        {
            return
                !(p.x < bounds.UpperLeftCorner.x ||
                p.y < bounds.UpperLeftCorner.y ||
                p.x > bounds.LowerRightCorner.x ||
                p.y > bounds.LowerRightCorner.y);
        }

        /// <summary>
        /// Return a point with the minimum x and y values of both points
        /// </summary>
        /// <param name="p"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static Point Min(this Point p, Point min)
        {
            if (p.x > min.x)
                p.x = min.x;
            if (p.y > min.y)
                p.y = min.y;
            return p;
        }

        /// <summary>
        /// Return a point with the maximum x and y values of both points
        /// </summary>
        /// <param name="p"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Point Max(this Point p, Point max)
        {
            if (p.x < max.x)
                p.x = max.x;
            if (p.y < max.y)
                p.y = max.y;
            return p;
        }

        /// <summary>
        /// Read the bytes from an int to transform it into a color
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Color ToColor(this int x)
        {
            unchecked
            {
                byte r = (byte)x;
                byte g = (byte)(x >> 8);
                byte b = (byte)(x >> 16);
                return new(r, g, b);
            }
        }

        /// <summary>
        /// Converts a color into an VT code for setting console foreground color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ToForegroundString(this Color color) 
            => $"\x1b[38;2;{color.R};{color.G};{color.B}m";

        /// <summary>
        /// Converts a color into a VT code for setting console background color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ToBackgroundString(this Color color)
            => $"\x1b[48;2;{color.R};{color.G};{color.B}m";
    }
}
