using System;

namespace GestionAguaPeriurbanos.Model
{
    public class TanqueModel
    {
        public double Capacity { get; }
        public double Volume { get; private set; }

        public TanqueModel(double capacity, double initialVolume = 0)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            Volume = Math.Clamp(initialVolume, 0, capacity);
        }

        // Adds volume and returns overflow amount (0 if none)
        public double AddVolume(double amount)
        {
            if (amount <= 0) return 0;
            double available = Capacity - Volume;
            if (amount <= available)
            {
                Volume += amount;
                return 0;
            }
            Volume = Capacity;
            return amount - available;
        }

        // Removes volume and returns shortage amount (0 if none)
        public double RemoveVolume(double amount)
        {
            if (amount <= 0) return 0;
            if (amount <= Volume)
            {
                Volume -= amount;
                return 0;
            }
            double shortage = amount - Volume;
            Volume = 0;
            return shortage;
        }

        public double GetFillPercentage()
        {
            if (Capacity == 0) return 0;
            return (Volume / Capacity) * 100.0;
        }
    }
}