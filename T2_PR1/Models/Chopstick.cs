namespace T2_PR1.Models
{
    internal class Chopstick
    {
        public int NumChopstick { get; set; } // El número de palet, de l'1 al 5
        public List<int> WantedByGuest { get; set; } = new List<int>(); // Indica qui està esperant pel palet. El comensal a [0] l'està utilitzant
    }
}