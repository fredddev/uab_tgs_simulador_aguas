namespace GestionAguaPeriurbanos.Model
{
    // Representa un registro de un día completo en la simulación.
    public class ResultadoDia
    {
        // Día dentro de la simulación (1..N)
        public int Dia { get; set; }

        // Estado inicial del stock (nivel del tanque al comenzar el día)
        public decimal NivelInicial { get; set; }

        // Litros que ingresan ese día (flujo de entrada)
        public decimal EntradaLitros { get; set; }

        // Litros consumidos ese día (flujo de salida)
        public decimal ConsumoLitros { get; set; }

        // Nivel del tanque al final del día
        public decimal NivelFinal { get; set; }

        // Indica si ese día actuó el bucle reforzador (acaparamiento)
        public bool ReforzadorAplicado { get; set; }

        // Indica si ese día actuó el bucle balanceador (racionamiento)
        public bool BalanceadorAplicado { get; set; }

        // True si el tanque llegó a 0 → escasez
        public bool HuboEscasez => NivelFinal <= 0;
    }
}