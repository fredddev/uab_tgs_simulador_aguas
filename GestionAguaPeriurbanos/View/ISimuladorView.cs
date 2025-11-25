using GestionAguaPeriurbanos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAguaPeriurbanos.View
{
        public interface ISimuladorView
        // Definimos qué puede pedirle el Presenter a la Vista y qué puede entregarle la Vista al Presenter.
        {
            // ===============================
            // ENTRADAS (inputs del usuario)
            // ===============================

            decimal NivelInicial { get; }
            // La Vista entrega el valor ingresado del stock inicial (estado del sistema).
            // Es get-only porque la Vista NO debe ser modificada por el Presenter en este dato.

            decimal CapacidadTanque { get; }
            // Capacidad máxima del tanque (límite físico del stock).
            // Define la frontera del depósito dentro del modelo.

            decimal ConsumoBase { get; }
            // Flujo de salida base (litros/día).
            // Es el consumo “normal” antes de retroalimentación.

            int FrecuenciaCisterna { get; }
            // Flujo de entrada periódico.
            // Es el intervalo (en días) entre recargas por cisterna.

            decimal VolumenCisterna { get; }
            // Magnitud del flujo de entrada.
            // Litros que entran cada vez que llega una cisterna.

            int DuracionDias { get; }
            // Horizonte temporal de simulación.
            // En sistemas dinámicos el tiempo es clave: sin horizonte no hay comportamiento.

            bool ReforzadorActivo { get; }
            // Interruptor sistémico que activa el bucle reforzador (acaparamiento).
            // Permite comparar escenario “con miedo social” vs “sin miedo social”.

            bool BalanceadorActivo { get; }
            // Interruptor sistémico que activa el bucle balanceador (racionamiento).
            // Representa intervención comunitaria para homeostasis.

            decimal UmbralReforzadorPct { get; }
            // Porcentaje del tanque donde empieza el acaparamiento.
            // Define en qué punto del stock una regla reforzadora entra en acción.

            decimal UmbralBalanceadorPct { get; }
            // Porcentaje del tanque donde empieza el racionamiento.
            // Punto de activación del bucle estabilizador.

            decimal IncrementoConsumoPorAcaparamientoPct { get; }
            // Intensidad del reforzador.
            // Cuánto sube el consumo cuando hay escasez percibida.

            decimal ReduccionConsumoPorRacionamientoPct { get; }
            // Intensidad del balanceador.
            // Cuánto baja el consumo cuando el nivel es crítico.

            // ===============================
            // SALIDAS (outputs hacia UI)
            // ===============================

            void MostrarResultados(List<ResultadoDia> resultados, int diaEscasez);
            // El Presenter manda a la Vista toda la serie temporal (dinámica del sistema).
            // Y también el día exacto de colapso (si ocurre).

            void MostrarMensaje(string mensaje);
            // Canal genérico de comunicación para errores o avisos.
            // Mantiene al Presenter libre de MessageBox (para no mezclar capas).
        }
    
}
