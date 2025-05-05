namespace T2_PR1.Models
{
    internal class Comensal
    {
        private readonly int MinThink = 500;
        private readonly int MinEat = 500;
        private readonly int MaxThink = 2000;
        private readonly int MaxEat = 1500;

        const string Action = "\n El comensal {0} està {1}";

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
                    return ConsoleColor.DarkGreen;
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

        public Thread GenerateComensalThread(object lockerAdd, object lockerConsole)
        {
            Random r = new Random();
            int timeThink = r.Next(MinThink, MaxThink + 1);
            int timeEat = r.Next(MinEat, MaxEat + 1);

            Thread comensal = new Thread(() =>
            {
                while (RemainingFood > 0)
                {
                    // Pensa
                    lock (lockerConsole)
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

                    lock (lockerConsole)
                    {
                        WriteSomethingInColor(ConsoleColor.White, GenerateForegroundConsoleColor(), "esperant els seus palets");
                    }
                    // Mentres no estigui a la posició [0] per menjar dels dos palets s'espera
                    while (LeftChopstick.WantedByGuest[0] != NumComensal || RightChopstick.WantedByGuest[0] != NumComensal)
                    {
                        Thread.Sleep(1);
                    }

                    // Menja
                    lock (lockerConsole)
                    {
                        WriteSomethingInColor(ConsoleColor.Green, GenerateForegroundConsoleColor(), "menjant!");
                    }
                    Thread.Sleep(timeEat);
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