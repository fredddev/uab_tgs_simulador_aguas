using System;
using System.Collections.Generic;

namespace GestionAguaPeriurbanos.Model
{
    // Representa el flujo de entrada: llegada de cisterna.
    public class FlujoEntradaModel
    {
        // Cada cuántos días llega agua
        public int FrecuenciaDias { get; set; }

        // Litros que se agregan cada vez que llega una cisterna
        public decimal VolumenPorLlegada { get; set; }

        // Calcula la entrada según el día actual
        public decimal CalcularEntrada(int dia)
        {
            // Si hoy es múltiplo de la frecuencia → llega agua
            return (dia % FrecuenciaDias == 0) ? VolumenPorLlegada : 0m;
        }
    }
}