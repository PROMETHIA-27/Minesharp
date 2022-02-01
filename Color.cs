namespace Minesharp
{
    /// <summary>
    /// An rgb struct representing color
    /// </summary>
    public struct Color : IEquatable<Color>
    {
        /// <summary>
        /// Create a new color from rgb values
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public Color(byte r, byte g, byte b)
            => (this.R, this.G, this.B) = (r, g, b);

        /// <summary>
        /// Red value
        /// </summary>
        public byte R;
        /// <summary>
        /// Blue value
        /// </summary>
        public byte G;
        /// <summary>
        /// Green value
        /// </summary>
        public byte B;

        /// <summary>
        /// Tests whether two colors are exactly the same
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(Color lhs, Color rhs) 
            => lhs.R == rhs.R && lhs.G == rhs.G && lhs.B == rhs.B;

        /// <summary>
        /// Tests whether two colors are not exactly the same
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(Color lhs, Color rhs)
            => lhs.R != rhs.R || lhs.G != rhs.G || lhs.B != rhs.B;

        /// <summary>
        /// Tests whether two colors are exactly the same
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Color other) 
            => this == other;

        /// <summary>
        /// Tests whether an object is an identical color
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) => obj is Color color && this == color;

        /// <summary>
        /// Gets the hash code of this color
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => HashCode.Combine(this.R, this.G, this.B);

        /// <summary>
        /// Black
        /// </summary>
        public static readonly Color Black = new();
        /// <summary>
        /// Dark Grey
        /// </summary>
        public static readonly Color DarkGrey = new(6, 6, 6);
        /// <summary>
        /// Slightly Darker Grey (than light grey)
        /// </summary>
        public static readonly Color SlightlyDarkerGrey = new(50, 50, 50);
        /// <summary>
        /// Light Grey
        /// </summary>
        public static readonly Color LightGrey = new(75, 75, 75);
        /// <summary>
        /// White
        /// </summary>
        public static readonly Color White = new(255, 255, 255);
        /// <summary>
        /// Darker Red
        /// </summary>
        public static readonly Color DarkerRed = new(200, 0, 0);
        /// <summary>
        /// Red
        /// </summary>
        public static readonly Color Red = new(255, 0, 0);
        /// <summary>
        /// Darker Green
        /// </summary>
        public static readonly Color DarkerGreen = new(0, 200, 0);
        /// <summary>
        /// Green
        /// </summary>
        public static readonly Color Green = new(0, 255, 0);
        /// <summary>
        /// Darker Cyan
        /// </summary>
        public static readonly Color DarkerCyan = new(0, 225, 225);
        /// <summary>
        /// Cyan
        /// </summary>
        public static readonly Color Cyan = new(0, 255, 255);
        /// <summary>
        /// Darker Blue
        /// </summary>
        public static readonly Color DarkerBlue = new(0, 0, 200);
        /// <summary>
        /// Blue
        /// </summary>
        public static readonly Color Blue = new(0, 0, 255);
        /// <summary>
        /// Brighter Blue
        /// </summary>
        public static readonly Color BrighterBlue = new(75, 75, 255);
        /// <summary>
        /// Purple
        /// </summary>
        public static readonly Color Purple = new(255, 0, 255);
    }
}
