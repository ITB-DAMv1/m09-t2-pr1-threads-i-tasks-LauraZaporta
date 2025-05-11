using CsvHelper;
using System.Globalization;
using T2_PR1_Tasks;

public class Program
{
    public static async Task Main()
    {
        const string CsvPath = "../../../Files/Statistics.csv";

        CancellationTokenSource cts = new CancellationTokenSource();
        GameFunctions game = new GameFunctions(cts);
        await game.RunGame();

        Console.Clear();
        Console.ResetColor();
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"\n The web has finished charging!" +
            $"\n ------------------------------");
        
        ConsoleColor color = game.statictics.NumCollisions > 0 ? ConsoleColor.Red : ConsoleColor.Green;
        
        Console.ForegroundColor = color;
        Console.WriteLine($"\n Your game statistics are: " +
            $"\n -------------------------" +
            $"\n Number of collisions: {game.statictics.NumCollisions}" +
            $"\n Number of asteroids generated: {game.statictics.NumAsteroidsGenerated}");

        try
        {
            using (StreamWriter sw = new StreamWriter(CsvPath, append: true))
            {
                if (new FileInfo(CsvPath).Length == 0)
                {
                    sw.WriteLine("Número de col·lisions, Número d'asteroides generats"); // Header
                }
                using (var csvWriter = new CsvWriter(sw, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                }))
                {
                    csvWriter.WriteRecords(new List<Statistics> { game.statictics });
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n Dades de la partida inserides correctament al csv :D");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n No s'han pogut inserir les estadístiques: {0}", ex);
        }
        Console.ResetColor();
    }
}