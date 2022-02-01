using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesharp
{
    /// <summary>
    /// Class representing the minesweeper game
    /// </summary>
    public class Game
    {
        ConsoleRenderer renderer;
        int fieldIdx;
        int gameCursorIdx;
        readonly ConcurrentQueue<ConsoleKeyInfo> inputQueue = new();
        readonly Text fpsText = new();
        bool exit;

        /// <summary>
        /// A stack of renderable objects. They are drawn in order from 0 to n, where n is the last member of the list.
        /// </summary>
        public List<IRenderable> renderStack = new();

        /// <summary>
        /// Start the game
        /// </summary>
        public void Start()
        {
            Random rand = new(9457348);

            Minefield field = new(32, 32);

            field.Settings.MineMin = 204;
            field.Settings.MineMax = 312;
            field.Settings.CalculateMineCount(rand);

            field.RandomizeRemainder(rand);
            field.RandomizeChunk(rand, new());

            PanningBorder border = new(new(new(), new(32, 32)));

            Background bg = new();
            bg.Color = Color.DarkGrey;
            bg.Size = new(new(), new(100, 50));

            var inputThread = new Thread(InputLoop);
            Interlocked.Exchange(ref collectInput, 1);
            inputThread.Start(this.inputQueue);

            this.renderer = new ConsoleRenderer(100, 50);
            Console.Clear();

            ConsoleCursor gameCursor = new(' ', Color.Black, Color.White);

            this.renderStack.Add(bg);
            this.renderStack.Add(field);
            // this.renderStack.Add(border);
            this.renderStack.Add(this.fpsText);
            this.renderStack.Add(gameCursor);

            this.fieldIdx = 1;
            this.gameCursorIdx = 3;

            this.GameLoop();
        }

        /// <summary>
        /// Starts a game loop, exited by setting exit to true.
        /// </summary>
        public void GameLoop()
        {
            double blinkTimer = 0d;
            var lastTimestamp = Stopwatch.GetTimestamp();
            while (!this.exit)
            {
                var timestamp = Stopwatch.GetTimestamp();
                var deltaTime = (timestamp - lastTimestamp) / (double)Stopwatch.Frequency;

                // Here is where FPS is set
                while (deltaTime < (1.0d / 60.0d))
                {
                    Thread.Sleep(0);
                    timestamp = Stopwatch.GetTimestamp();
                    deltaTime = (timestamp - lastTimestamp) / (double)Stopwatch.Frequency;
                }

                var field = (Minefield)this.renderStack[this.fieldIdx];

                var gameCursor = (ConsoleCursor)this.renderStack[this.gameCursorIdx];
                blinkTimer += deltaTime;
                if (blinkTimer >= 0.25d)
                {
                    blinkTimer = 0d;
                    gameCursor.Character = gameCursor.Character == '\0' ? ' ' : '\0';
                }

                this.fpsText.String = $"FPS: {1.0 / deltaTime}";

                if (this.inputQueue.TryDequeue(out var input))
                {
                    switch (input.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            gameCursor.Position += new Point(-1, 0);
                            gameCursor.Character = ' ';
                            break;
                        case ConsoleKey.UpArrow:
                            gameCursor.Position += new Point(0, -1);
                            gameCursor.Character = ' ';
                            break;
                        case ConsoleKey.RightArrow:
                            gameCursor.Position += new Point(1, 0);
                            gameCursor.Character = ' ';
                            break;
                        case ConsoleKey.DownArrow:
                            gameCursor.Position += new Point(0, 1);
                            gameCursor.Character = ' ';
                            break;
                        case ConsoleKey.Spacebar:
                            if (field.FloodReveal(gameCursor.Position))
                                this.exit = true;
                            break;
                        case ConsoleKey.F:
                            field.ToggleTileFlag(gameCursor.Position);
                            break;
                    }
                }

                this.renderStack[this.fieldIdx] = field;
                this.renderStack[this.gameCursorIdx] = gameCursor;

                for (int i = 0; i < this.renderStack.Count; i++)
                    this.renderer.Draw(this.renderStack[i]);
                this.renderer.Render();

                lastTimestamp = Stopwatch.GetTimestamp();
            }

            Interlocked.Exchange(ref collectInput, 0);
        }

        static long collectInput = 1;
        static void InputLoop(object? queueObj)
        {
            if (queueObj is not ConcurrentQueue<ConsoleKeyInfo> queue)
                return;
            while (Interlocked.Read(ref collectInput) == 1)
                queue.Enqueue(Console.ReadKey(true));
        }

        string CollectNumber(StringBuilder builder, out bool finished, out int result)
        {
            string builtString;
            if (this.inputQueue.TryDequeue(out var keyInf))
            {
                if (keyInf.Key is 
                    ConsoleKey.D1 or 
                    ConsoleKey.D2 or 
                    ConsoleKey.D3 or
                    ConsoleKey.D4 or
                    ConsoleKey.D5 or
                    ConsoleKey.D6 or
                    ConsoleKey.D7 or
                    ConsoleKey.D8 or
                    ConsoleKey.D9 or
                    ConsoleKey.D0)
                {
                    builder.Append(keyInf);
                    builtString = builder.ToString();
                    finished = false;
                    result = int.Parse(builtString);
                    return builtString;
                }
                else if (keyInf.Key is ConsoleKey.Enter)
                {
                    builtString = builder.ToString();
                    finished = true;
                    result = int.Parse(builtString);
                    return builtString;
                }
            }
            else
                Thread.Sleep(0);
            finished = false;
            builtString = builder.ToString();
            result = int.Parse(builtString);
            return builtString;
        }
    }
}
