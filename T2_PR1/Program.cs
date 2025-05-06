using T2_PR1.Models;

public class Program()
{
    private static object listLocker = new object();
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
            threads.Add(comensal.GenerateComensalThread(listLocker));
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

        if (Comensal.HasSomeoneDiedOfHunger)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n (:c) La simulació ha acabat perquè un comensal ha mort de gana.");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n (:D) Tots els comensals han acabat de menjar.");
            Console.ResetColor();
        }
    }
}