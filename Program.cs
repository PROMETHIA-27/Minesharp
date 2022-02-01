namespace Minesharp
{
    class Program
    {
        static void Main()
        {
            InitAdvancedTerminal(); 

            Console.Write("\x1b[?1049h"); // Switch to alternate screen buffer
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Write("\x1b[?12l"); // Disable cursor blinking
            Console.Write("\x1b[?25l"); // Disable cursor visibility

            Game game = new();
            game.Start();

            Console.Write("\x1b[?1049l"); // Switch to main screen buffer
        }

        static void InitAdvancedTerminal()
        {
            if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 10586))
                return;
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
                return;

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
                return;
        }

        #region ugly p/invoke stuff
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        #endregion
    }
}