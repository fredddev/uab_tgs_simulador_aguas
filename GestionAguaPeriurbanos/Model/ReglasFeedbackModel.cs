using System;

namespace GestionAguaPeriurbanos.Model
{
    public class ReglasFeedbackModel
    {
        // When tank percent < LowThreshold, inflow is multiplied by LowFactor (>1 to increase inflow)
        // When tank percent > HighThreshold, inflow is multiplied by HighFactor (<1 to reduce inflow)
        public double LowThresholdPercent { get; set; } = 30.0;
        public double HighThresholdPercent { get; set; } = 80.0;
        public double LowFactor { get; set; } = 1.25;
        public double HighFactor { get; set; } = 0.75;

        public ReglasFeedbackModel() { }

        public ReglasFeedbackModel(double lowThresholdPercent, double highThresholdPercent, double lowFactor, double highFactor)
        {
            if (lowThresholdPercent < 0 || lowThresholdPercent >= highThresholdPercent)
                throw new ArgumentException(nameof(lowThresholdPercent));
            if (highThresholdPercent > 100 || highThresholdPercent <= lowThresholdPercent)
                throw new ArgumentException(nameof(highThresholdPercent));
            LowThresholdPercent = lowThresholdPercent;
            HighThresholdPercent = highThresholdPercent;
            LowFactor = lowFactor;
            HighFactor = highFactor;
        }

        public double AdjustInflow(double baseInflow, TanqueModel tanque)
        {
            if (tanque == null) throw new ArgumentNullException(nameof(tanque));
            if (baseInflow <= 0) return baseInflow;

            double pct = tanque.GetFillPercentage();
            if (pct < LowThresholdPercent) return baseInflow * LowFactor;
            if (pct > HighThresholdPercent) return baseInflow * HighFactor;
            return baseInflow;
        }
    }
}