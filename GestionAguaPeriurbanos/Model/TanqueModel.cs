using System;

namespace GestionAguaPeriurbanos.Model
{
    // Representa el depósito principal del sistema: el tanque de agua.
    public class TanqueModel
    {
        // Capacidad máxima física del tanque (no cambia durante la simulación)
        public decimal Capacidad { get; }

        // Nivel actual de agua dentro del tanque
        public decimal Nivel { get; private set; }

        // Constructor: define la capacidad y el nivel inicial
        public TanqueModel(decimal capacidad, decimal nivelInicial)
        {
            Capacidad = capacidad;      // Límite superior del stock
            Nivel = nivelInicial;       // Estado inicial del sistema
        }

        // Aplica el flujo de entrada (lleno por cisterna)
        public void Agregar(decimal litros)
        {
            Nivel += litros;            // Se suma agua al stock

            if (Nivel > Capacidad)
                Nivel = Capacidad;      // No puede superar el límite físico
        }

        // Aplica el flujo de salida (consumo del barrio)
        public void Consumir(decimal litros)
        {
            Nivel -= litros;            // Se reduce el stock

            if (Nivel < 0)
                Nivel = 0;              // El stock nunca puede ser negativo
        }
    }
}