using System;
using System.Collections.Generic;
using GestionAguaPeriurbanos.Model;
using GestionAguaPeriurbanos.View;

namespace GestionAguaPeriurbanos.Presenter
{
    // El Presentador actúa como intermediario entre:
    // 1. La Vista (UI)
    // 2. El Modelo (lógica sistémica)
    public class SimuladorPresenter
    {
        private readonly ISimuladorView view;
        // Referencia a la Vista. Es readonly porque nunca debe cambiar.

        public SimuladorPresenter(ISimuladorView view)
        {
            this.view = view;
            // Inyectamos la Vista. Esto permite testear Presenter sin UI real.
        }


        // ============================================================
        // MÉTODO PRINCIPAL DE EJECUCIÓN
        // Este es el “cerebro” que une Vista → Modelo → Vista
        // ============================================================
        public void EjecutarSimulacion()
        {
            try
            {
                // 1) VALIDACIONES BÁSICAS ------------------------------

                if (view.CapacidadTanque <= 0)
                    throw new Exception("La capacidad del tanque debe ser mayor que 0.");

                if (view.FrecuenciaCisterna <= 0)
                    throw new Exception("La frecuencia de llegada debe ser mayor que 0.");

                if (view.DuracionDias <= 0)
                    throw new Exception("La duración debe ser mayor que 0.");

                if (view.ConsumoBase < 0)
                    throw new Exception("El consumo no puede ser negativo.");


                // 2) CONSTRUCCIÓN DEL MODELO --------------------------

                var modelo = new SimuladorModel
                {
                    // Stock
                    Tanque = new TanqueModel(
                        capacidad: view.CapacidadTanque,
                        nivelInicial: view.NivelInicial
                    ),

                    // Flujo de entrada
                    Entrada = new FlujoEntradaModel
                    {
                        FrecuenciaDias = view.FrecuenciaCisterna,
                        VolumenPorLlegada = view.VolumenCisterna
                    },

                    // Flujo de salida
                    Consumo = new FlujoConsumoModel
                    {
                        ConsumoBase = view.ConsumoBase,
                        IncrementoPorAcaparamientoPct = view.IncrementoConsumoPorAcaparamientoPct,
                        ReduccionPorRacionamientoPct = view.ReduccionConsumoPorRacionamientoPct
                    },

                    // Retroalimentaciones
                    ReforzadorActivo = view.ReforzadorActivo,
                    BalanceadorActivo = view.BalanceadorActivo,

                    // Umbrales de los bucles
                    UmbralReforzadorPct = view.UmbralReforzadorPct,
                    UmbralBalanceadorPct = view.UmbralBalanceadorPct
                };


                // 3) EJECUCIÓN DE LA DINÁMICA -------------------------

                var resultados = modelo.Simular(view.DuracionDias);
                // Este método devuelve una lista de ResultadoDia
                // o sea, la serie temporal completa de comportamiento emergente.


                // 4) ANÁLISIS DEL RESULTADO ---------------------------

                // Buscamos el primer día donde el tanque llegó a 0
                var diaEscasez = resultados
                                    .FirstOrDefault(r => r.HuboEscasez)?.Dia ?? -1;


                // 5) ENVIAR RESULTADOS A LA VISTA ---------------------

                view.MostrarResultados(resultados, diaEscasez);
                // La Vista se encarga de pintar tabla y gráficos (si tiene).


            }
            catch (Exception ex)
            {
                // Si algo sale mal, mostramos error en la UI
                view.MostrarMensaje("Error en la simulación: " + ex.Message);
            }
        }
    }

}