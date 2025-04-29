using System.Net.Sockets;
using System.Reflection;

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

        public Comensal(int numComensal, Chopstick leftChopstick, Chopstick rightChopstick)
        {
            NumComensal = numComensal;
            LeftChopstick = leftChopstick;
            RightChopstick = rightChopstick;
        }

        public Thread GenerateComensalThread(object lockerAdd)
        {
            Random r = new Random();
            int timeThink = r.Next(MinThink, MaxThink + 1);
            int timeEat = r.Next(MinEat, MaxEat + 1);

            Thread comensal = new Thread(() =>
            {
                // Pensa
                Console.WriteLine(Action, NumComensal, "pensant...");
                Thread.Sleep(timeThink);

                // S'afegeix a la llista d'espera dels palets
                // Només es pot afegir un a la llista a la vegada
                lock (lockerAdd)
                {
                    LeftChopstick.WantedByGuest.Add(NumComensal);
                    RightChopstick.WantedByGuest.Add(NumComensal);
                }

                Console.WriteLine(Action, NumComensal, "esperant els seus palets");
                // Mentres no estigui a la posició [0] per menjar dels dos palets s'espera
                while (LeftChopstick.WantedByGuest[0] != NumComensal || RightChopstick.WantedByGuest[0] != NumComensal)
                {
                    Thread.Sleep(1);
                }

                // Menja
                Console.WriteLine(Action, NumComensal, "menjant!");
                Thread.Sleep(timeEat);

                // Deixa els palets
                LeftChopstick.WantedByGuest.Remove(NumComensal);
                RightChopstick.WantedByGuest.Remove(NumComensal);
            });
            return comensal;
        }
    }
}