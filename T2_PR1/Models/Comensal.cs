namespace T2_PR1.Models
{
    internal class Comensal
    {
        const string Action = "\n El comensal {0} està {1}";

        public static volatile bool HasSomeoneDiedOfHunger = false; // Senyal global a la que tots els comensals tenen accès

        private static readonly object LockerConsole = new object();
        private static readonly object LockerHunger = new object();
        private readonly int MinThink = 500;
        private readonly int MinEat = 500;
        private readonly int MaxThink = 2000;
        private readonly int MaxEat = 1500;
        private readonly int TimeLimitForHunger = 15000;

        private int _numComensal;
        public int NumComensal
        {
            get => _numComensal;
            set
            {
                if (!(value >= 1 && value <=5))
                {
                    throw new ArgumentException("El comensal només pot tenir un número de l'1 al 5");
                }
                _numComensal = value;
            }
        }
        public Chopstick LeftChopstick { get; set; }
        public Chopstick RightChopstick { get; set; }
        public int RemainingFood { get; set; } = 10; // Inicialment hi ha 10 unitats de menjar
        public DateTime LastTimeEating { get; set; } = DateTime.Now;
        public List<double> TimesOfHunger { get; set; } = new List<double>();

        public Comensal(int numComensal, Chopstick leftChopstick, Chopstick rightChopstick)
        {
            NumComensal = numComensal;
            LeftChopstick = leftChopstick;
            RightChopstick = rightChopstick;
        }

        private ConsoleColor GenerateForegroundConsoleColor()
        {
            switch (NumComensal)
            {
                case 1:
                    return ConsoleColor.DarkRed;
                case 2:
                    return ConsoleColor.DarkGray;
                case 3:
                    return ConsoleColor.DarkMagenta;
                case 4:
                    return ConsoleColor.DarkBlue;
                case 5:
                    return ConsoleColor.Black;
                default:
                    return Console.BackgroundColor;
            }
        }
        private void WriteSomethingInColor(ConsoleColor bg, ConsoleColor fg, string msg)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.WriteLine(Action, NumComensal, msg);
            Console.ResetColor();
        }
        private bool IsThereHunger()
        {
            return (DateTime.Now - LastTimeEating).TotalMilliseconds > TimeLimitForHunger;
        }

        public Thread GenerateComensalThread(object lockerAdd)
        {
            Random r = new Random();
            int timeThink = r.Next(MinThink, MaxThink + 1);
            int timeEat = r.Next(MinEat, MaxEat + 1);

            Thread comensal = new Thread(() =>
            {
                while (RemainingFood > 0 && !HasSomeoneDiedOfHunger)
                {
                    // Pensa
                    if (HasSomeoneDiedOfHunger) { return; }
                    lock (LockerConsole)
                    {
                        WriteSomethingInColor(ConsoleColor.Yellow, GenerateForegroundConsoleColor(), "pensant...");
                    }
                    Thread.Sleep(timeThink);

                    // S'afegeix a la llista d'espera dels palets
                    // Només es pot afegir un a la llista a la vegada
                    lock (lockerAdd)
                    {
                        LeftChopstick.WantedByGuest.Add(NumComensal);
                        RightChopstick.WantedByGuest.Add(NumComensal);
                    }

                    if (HasSomeoneDiedOfHunger) { return; }
                    lock (LockerConsole)
                    {
                        WriteSomethingInColor(ConsoleColor.White, GenerateForegroundConsoleColor(), "esperant els seus palets");
                    }
                    // Mentres no estigui a la posició [0] per menjar dels dos palets s'espera
                    while ((LeftChopstick.WantedByGuest[0] != NumComensal || RightChopstick.WantedByGuest[0] != NumComensal)
                        && !HasSomeoneDiedOfHunger)
                    {
                        Thread.Sleep(1);
                    }

                    // Menja
                    // Però primer veu si ja és mor i registra el temps que ha estat en gana
                    // Si té gana la simulació acabarà
                    if (IsThereHunger())
                    {
                        lock (LockerConsole)
                        {
                            WriteSomethingInColor(ConsoleColor.Black, ConsoleColor.Red, "mort de gana :c");
                        }
                        lock (LockerHunger)
                        {
                            HasSomeoneDiedOfHunger = true;
                        }
                        return;
                    }

                    TimesOfHunger.Add((DateTime.Now - LastTimeEating).TotalMilliseconds);
                    lock (LockerConsole)
                    {
                        WriteSomethingInColor(ConsoleColor.Green, GenerateForegroundConsoleColor(), "menjant!");
                    }
                    Thread.Sleep(timeEat);
                    LastTimeEating = DateTime.Now;
                    RemainingFood--;

                    // Deixa els palets
                    LeftChopstick.WantedByGuest.Remove(NumComensal);
                    RightChopstick.WantedByGuest.Remove(NumComensal);
                }
            });
            return comensal;
        }
    }
}