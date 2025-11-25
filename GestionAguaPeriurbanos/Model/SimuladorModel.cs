using System;
using System.Collections.Generic;

namespace GestionAguaPeriurbanos.Model
{
    // Motor dinámico del sistema: combina stocks, flujos y retroalimentaciones.
    public class SimuladorModel
    {
        // STOCK
        public TanqueModel Tanque { get; set; }

        // FLUJOS
        public FlujoEntradaModel Entrada { get; set; }
        public FlujoConsumoModel Consumo { get; set; }

        // Activación de bucles sistémicos
        public bool ReforzadorActivo { get; set; }
        public bool BalanceadorActivo { get; set; }

        // Umbrales
        public decimal UmbralReforzadorPct { get; set; }
        public decimal UmbralBalanceadorPct { get; set; }

        // Simulación principal
        public List<ResultadoDia> Simular(int dias)
        {
            var resultados = new List<ResultadoDia>();

            for (int dia = 1; dia <= dias; dia++)
            {
                var res = new ResultadoDia();
                res.Dia = dia;
                res.NivelInicial = Tanque.Nivel;

                // --- 1. FLUJO DE ENTRADA ---
                res.EntradaLitros = Entrada.CalcularEntrada(dia);
                Tanque.Agregar(res.EntradaLitros);

                // --- 2. CALCULAR CONSUMO ---
                decimal consumo = Consumo.ConsumoBase;

                // Bucle reforzador (acaparamiento)
                if (ReforzadorActivo)
                {
                    decimal previo = consumo;
                    consumo = Consumo.AplicarReforzador(consumo, Tanque.Nivel, Tanque.Capacidad, UmbralReforzadorPct);
                    res.ReforzadorAplicado = consumo != previo;
                }

                // Bucle balanceador (racionamiento)
                if (BalanceadorActivo)
                {
                    decimal previo = consumo;
                    consumo = Consumo.AplicarBalanceador(consumo, Tanque.Nivel, Tanque.Capacidad, UmbralBalanceadorPct);
                    res.BalanceadorAplicado = consumo != previo;
                }

                res.ConsumoLitros = consumo;

                // --- 3. APLICAR SALIDA ---
                Tanque.Consumir(consumo);

                // --- 4. NIVEL FINAL ---
                res.NivelFinal = Tanque.Nivel;

                resultados.Add(res);
            }

            return resultados;
        }
    }
}