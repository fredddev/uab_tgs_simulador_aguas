using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAguaPeriurbanos.Model
{
    // DTO: “foto” de todos los parámetros de entrada
    public class EscenarioInputDto
    {
        public decimal NivelInicial { get; set; }
        public decimal CapacidadTanque { get; set; }
        public decimal ConsumoBase { get; set; }
        public int FrecuenciaCisterna { get; set; }
        public decimal VolumenCisterna { get; set; }
        public int DuracionDias { get; set; }

        public bool ReforzadorActivo { get; set; }
        public bool BalanceadorActivo { get; set; }

        public decimal UmbralReforzadorPct { get; set; }
        public decimal UmbralBalanceadorPct { get; set; }

        public decimal IncrementoConsumoPorAcaparamientoPct { get; set; }
        public decimal ReduccionConsumoPorRacionamientoPct { get; set; }
    }
}
