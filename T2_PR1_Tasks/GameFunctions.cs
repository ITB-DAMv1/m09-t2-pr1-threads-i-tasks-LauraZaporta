namespace T2_PR1_Tasks
{
    public class GameFunctions
    {
        const int FreqRender = 50;
        const int FreqLogic = 20;
        const char SpaceShip = '^';

        static int width = 150;
        static int height = 100;
        static int screenMargin = 5;
        static int speed = 2;
        static int playerX = width / 2 - 15;
        static int playerY = Console.WindowHeight - 2;
        static List<(int x, int y)> asteroids = new List<(int, int)>();

        public CancellationTokenSource Cts { get; set; }
        public CancellationToken Tk { get; set; }
        public char Asteroid { get; set; }

        public GameFunctions(CancellationTokenSource cts, char asteroidSprite) 
        { 
            Cts = cts; 
            Tk = cts.Token;
            Asteroid = asteroidSprite;
        }

        public async Task RunGame()
        {
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            Console.CursorVisible = false;

            Task renderTask = Task.Run(async () => await Render(SpaceShip, Asteroid));
            Task logicTask = Task.Run(async () =>
            {
                while (!Tk.IsCancellationRequested)
                {
                    lock (asteroids)
                    {
                        MoveAsteroids();
                        if (IsThereCollision())
                        {
                            playerX = width / 2 - 15;
                            asteroids.Clear();
                        }
                    }
                    await Task.Delay(FreqLogic);
                }
            });
            Task inputTask = Task.Run(ShipMovement);
            Task generateAsteroidsTask = Task.Run(GenerateAsteroids);

            await Task.WhenAny(renderTask, logicTask, inputTask, generateAsteroidsTask);
        }

        // Render of all
        private async Task Render(char characterSprite, char asteroidSprite)
        {
            while (!Tk.IsCancellationRequested)
            {
                Console.Clear();

                // Player
                Console.SetCursorPosition(playerX, playerY);
                Console.Write(SpaceShip);
                // Asteroids
                lock (asteroids)
                {
                    foreach (var asteroid in asteroids)
                    {
                        Console.SetCursorPosition(asteroid.x, asteroid.y);
                        Console.Write(Asteroid);
                    }
                }
                // Hz
                await Task.Delay(FreqRender);
            }
        }

        // Asteroids
        private async Task GenerateAsteroids()
        {
            Random r = new Random();
            while (!Tk.IsCancellationRequested)
            {
                lock (asteroids)
                {
                    // Screen margin left exagerated due to a window bug. With -30 everything works fine
                    asteroids.Add((r.Next(screenMargin, width - screenMargin - 30), 0));
                }
                await Task.Delay(300);
            }
        }
        private void MoveAsteroids()
        {
            lock (asteroids)
            {
                for (int i = 0; i < asteroids.Count; i++)
                {
                    asteroids[i] = (asteroids[i].x, asteroids[i].y + 1);
                    if (asteroids[i].y >= height)
                    {
                        asteroids.RemoveAt(i);
                    }
                }
            }
        }

        // SpaceShip
        private async Task ShipMovement()
        {
            while (!Tk.IsCancellationRequested)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.A:
                        if (playerX > screenMargin) { playerX -= speed; }
                        break;
                    case ConsoleKey.D:
                        if (playerX < width - screenMargin - 30) { playerX += speed; }
                        break;
                    case ConsoleKey.Q:
                        Cts.Cancel();
                        break;
                }
                await Task.Delay(FreqLogic);
            }
        }
        private bool IsThereCollision()
        {
            lock (asteroids)
            {
                foreach (var asteroid in asteroids)
                {
                    if (asteroid.y == playerY && Math.Abs(asteroid.x - playerX) <= 1)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}