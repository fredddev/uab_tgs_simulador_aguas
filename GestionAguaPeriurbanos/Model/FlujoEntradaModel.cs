using System;
using System.Collections.Generic;

namespace GestionAguaPeriurbanos.Model
{
    public class FlujoEntradaModel
    {
        // Daily inflow rates (index 0 = day 1). If the requested day is beyond the array,
        // the last value is reused.
        private readonly double[] _dailyRates;

        public FlujoEntradaModel(IEnumerable<double> dailyRates)
        {
            if (dailyRates == null) throw new ArgumentNullException(nameof(dailyRates));
            _dailyRates = new List<double>(dailyRates).ToArray();
            if (_dailyRates.Length == 0) throw new ArgumentException("dailyRates must contain at least one value");
        }

        public double GetForDay(int day)
        {
            if (day < 1) throw new ArgumentOutOfRangeException(nameof(day));
            int index = Math.Min(day - 1, _dailyRates.Length - 1);
            return _dailyRates[index];
        }
    }
}