using System;
using System.Collections.Generic;

namespace GestionAguaPeriurbanos.Model
{
    public class SimuladorModel
    {
        public TanqueModel Tanque { get; }
        public FlujoEntradaModel Entrada { get; }
        public FlujoConsumoModel Consumo { get; }
        public ReglasFeedbackModel Reglas { get; }

        public SimuladorModel(TanqueModel tanque, FlujoEntradaModel entrada, FlujoConsumoModel consumo, ReglasFeedbackModel reglas = null)
        {
            Tanque = tanque ?? throw new ArgumentNullException(nameof(tanque));
            Entrada = entrada ?? throw new ArgumentNullException(nameof(entrada));
            Consumo = consumo ?? throw new ArgumentNullException(nameof(consumo));
            Reglas = reglas ?? new ReglasFeedbackModel();
        }

        public List<ResultadoDia> Run(int days)
        {
            if (days < 1) throw new ArgumentOutOfRangeException(nameof(days));
            var results = new List<ResultadoDia>(days);

            for (int d = 1; d <= days; d++)
            {
                double start = Tanque.Volume;
                double baseInflow = Entrada.GetForDay(d);
                double inflow = Reglas.AdjustInflow(baseInflow, Tanque);
                double overflow = Tanque.AddVolume(inflow);
                double consumption = Consumo.GetForDay(d);
                double shortage = Tanque.RemoveVolume(consumption);
                double end = Tanque.Volume;

                results.Add(new ResultadoDia
                {
                    Day = d,
                    StartVolume = start,
                    Inflow = inflow,
                    Consumption = consumption,
                    EndVolume = end,
                    Overflow = overflow,
                    Shortage = shortage
                });
            }

            return results;
        }
    }
}