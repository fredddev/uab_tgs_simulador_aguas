namespace GestionAguaPeriurbanos.Model
{
    public class ResultadoDia
    {
        public int Day { get; set; }
        public double StartVolume { get; set; }
        public double Inflow { get; set; }
        public double Consumption { get; set; }
        public double EndVolume { get; set; }
        public double Overflow { get; set; }
        public double Shortage { get; set; }
    }
}