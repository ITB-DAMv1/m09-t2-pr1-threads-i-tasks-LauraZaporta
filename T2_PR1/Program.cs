using System.Formats.Asn1;
using CsvHelper;
using System.Globalization;
using T2_PR1.Models;

public class Program()
{
    private static object listLocker = new object();
    public static void Main()
    {
        const string CsvPath = "../../../Files/Statistics.csv";

        List<Thread> threads = new List<Thread>();
        List<Comensal> comensals = new List<Comensal>();
        List<Chopstick> chopsticks = new List<Chopstick>();
        List<Statistic> statistics = new List<Statistic>();

        for (int i = 0; i < 5; i++)
        {
            chopsticks.Add(new Chopstick { NumChopstick = i + 1 });
        }
        for (int i = 0; i < 5; i++)
        {
            Chopstick chopstickL = chopsticks[(i + 4) % 5];  
            Chopstick chopstickR = chopsticks[i];          

            Comensal comensal = new Comensal(i + 1, chopstickL, chopstickR);
            comensals.Add(comensal);
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

            foreach (Comensal comensal in comensals)
            {
                statistics.Add(new Statistic { 
                    NumComensal = comensal.NumComensal,
                    MaxTimeInHunger = Math.Round(comensal.TimesOfHunger.Max()/1000.0, 4),
                    NumTimesAte = comensal.TimesOfHunger.Count()
                });
            }
            using (StreamWriter sw = new StreamWriter(CsvPath))
            {
                sw.WriteLine("Número de comensal, Màxim temps en gana (segons), Número de vegades que ha menjat"); // Header

                using (var csvWriter = new CsvWriter(sw, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false
                }))
                {
                    csvWriter.WriteRecords(statistics);
                }
            }

            Console.WriteLine("\n Csv escrit correctament!\n");
            Console.ResetColor();
        }
    }
}