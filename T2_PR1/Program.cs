using T2_PR1.Models;

public class Program()
{
    private static object listLocker = new object();
    private static readonly object consoleLocker = new object();
    public static void Main()
    {
        List<Thread> threads = new List<Thread>();
        List<Chopstick> chopsticks = new List<Chopstick>();

        for (int i = 0; i < 5; i++)
        {
            chopsticks.Add(new Chopstick { NumChopstick = i + 1 });
        }
        for (int i = 0; i < 5; i++)
        {
            Chopstick chopstickL = chopsticks[(i + 4) % 5];  
            Chopstick chopstickR = chopsticks[i];          

            Comensal comensal = new Comensal(i + 1, chopstickL, chopstickR);
            threads.Add(comensal.GenerateComensalThread(listLocker, consoleLocker));
        }

        Console.Clear(); 
        Console.ResetColor();
        foreach (Thread thread in threads)
        {
            thread.Start();
        }
        foreach (Thread thread in threads)
        {
            thread.Join();
        }
    }
}