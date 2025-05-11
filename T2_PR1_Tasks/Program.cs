using T2_PR1_Tasks;

public class Program
{
    public static async Task Main()
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        GameFunctions game = new GameFunctions(cts, '0');
        await game.RunGame();

        Console.Clear();
    }
}