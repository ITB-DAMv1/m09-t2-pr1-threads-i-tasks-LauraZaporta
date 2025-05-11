using T2_PR1_Tasks;

public class Program
{
    public static async Task Main()
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        GameFunctions game = new GameFunctions(cts);
        await game.RunGame();

        Console.Clear();
        Console.ResetColor();

        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"\n The web has finished charging!" +
            $"\n -----------------------------");
        
        ConsoleColor color = game.statictics.NumCollisions > 0 ? ConsoleColor.Red : ConsoleColor.Green;
        
        Console.ForegroundColor = color;
        Console.WriteLine($"\n Your game statistics are: " +
            $"\n -------------------------" +
            $"\n Number of collisions: {game.statictics.NumCollisions}" +
            $"\n Number of asteroids generated: {game.statictics.NumAsteroidsGenerated}");
        Console.ResetColor();
    }
}