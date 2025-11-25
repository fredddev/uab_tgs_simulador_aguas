using System;
using System.Collections.Generic;

namespace GestionAguaPeriurbanos.Model
{
    // Flujo de salida: consumo del barrio.
    public class FlujoConsumoModel
    {
        // Consumo normal sin retroalimentación
        public decimal ConsumoBase { get; set; }

        // Incremento del consumo cuando hay acaparamiento (bucle reforzador)
        public decimal IncrementoPorAcaparamientoPct { get; set; }

        // Reducción del consumo cuando hay racionamiento (bucle balanceador)
        public decimal ReduccionPorRacionamientoPct { get; set; }

        // Consumo modificado por acaparamiento si el nivel es bajo
        public decimal AplicarReforzador(decimal consumo, decimal nivel, decimal capacidad, decimal umbralPct)
        {
            if (nivel <= capacidad * (umbralPct / 100m))
                return consumo * (1m + IncrementoPorAcaparamientoPct / 100m);

            return consumo;
        }

        // Consumo modificado por racionamiento si el nivel es crítico
        public decimal AplicarBalanceador(decimal consumo, decimal nivel, decimal capacidad, decimal umbralPct)
        {
            if (nivel <= capacidad * (umbralPct / 100m))
                return consumo * (1m - ReduccionPorRacionamientoPct / 100m);

            return consumo;
        }
    }
}