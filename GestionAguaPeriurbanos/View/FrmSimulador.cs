using GestionAguaPeriurbanos.Model;
using GestionAguaPeriurbanos.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GestionAguaPeriurbanos.View
{
    public partial class FrmSimulador : Form, ISimuladorView
    {
        private readonly SimuladorPresenter presenter;

        public FrmSimulador()
        {
            InitializeComponent();
            presenter = new SimuladorPresenter(this);
        }

        // ===============================
        // IMPLEMENTACIÓN DE LA INTERFAZ
        // ===============================

        public decimal NivelInicial => numericNivelInicial.Value;
        public decimal CapacidadTanque => numericCapacidad.Value;
        public decimal ConsumoBase => numericConsumoBase.Value;
        public int FrecuenciaCisterna => (int)numericFrecuencia.Value;
        public decimal VolumenCisterna => numericVolumenCisterna.Value;
        public int DuracionDias => (int)numericDuracionDias.Value;

        public bool ReforzadorActivo => chkReforzador.Checked;
        public bool BalanceadorActivo => chkBalanceador.Checked;

        public decimal UmbralReforzadorPct => numericUmbralReforzadorPct.Value;
        public decimal UmbralBalanceadorPct => numericUmbralBalanceadorPct.Value;

        public decimal IncrementoConsumoPorAcaparamientoPct => numericIncrementoAcaparamiento.Value;
        public decimal ReduccionConsumoPorRacionamientoPct => numericReduccionRacionamiento.Value;


        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje);
        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
            presenter.EjecutarSimulacion();
        }

        private void FrmSimulador_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        public void MostrarResultados(List<ResultadoDia> resultados, int diaEscasez)
        {
            // Tabla
            gridResultados.DataSource = null;
            gridResultados.DataSource = resultados;

            // Gráfico sistémico
            PintarGrafico(resultados);

            // Mensaje escasez
            if (diaEscasez > 0)
                MessageBox.Show($"Escasez detectada en el día {diaEscasez}.", "Resultado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("No hubo escasez durante la simulación.", "Resultado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void PintarGrafico(List<ResultadoDia> resultados)
        {
            // Limpia todo
            chartNivelTanque.Series.Clear();
            chartNivelTanque.ChartAreas.Clear();
            chartNivelTanque.Legends.Clear();
            chartNivelTanque.Titles.Clear();

            // Área del gráfico
            var area = new ChartArea("areaNivel");
            area.AxisX.Title = "Día";
            area.AxisY.Title = "Litros";
            area.AxisX.Interval = 1;
            chartNivelTanque.ChartAreas.Add(area);

            // Leyenda
            chartNivelTanque.Legends.Add(new Legend("legend1"));

            // ==============================
            // SERIE PRINCIPAL (nivel tanque)
            // ==============================
            var serieNivel = new Series("serieNivel")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.RoyalBlue,
                ChartArea = "areaNivel",
                Legend = "legend1"
            };
            chartNivelTanque.Series.Add(serieNivel);

            // ==============================
            // SERIES DE EVENTOS (marcadores)
            // ==============================

            // Llegada de cisterna
            var serieCisterna = new Series("Cisterna")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Star5,
                MarkerSize = 10,
                Color = Color.ForestGreen,
                ChartArea = "areaNivel",
                Legend = "legend1"
            };
            chartNivelTanque.Series.Add(serieCisterna);

            // Escasez
            var serieEscasez = new Series("Escasez")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 9,
                Color = Color.Red,
                ChartArea = "areaNivel",
                Legend = "legend1"
            };
            chartNivelTanque.Series.Add(serieEscasez);

            // Reforzador aplicado (acaparamiento)
            var serieReforzador = new Series("Reforzador")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Square,
                MarkerSize = 8,
                Color = Color.DarkOrange,
                ChartArea = "areaNivel",
                Legend = "legend1"
            };
            chartNivelTanque.Series.Add(serieReforzador);

            // Balanceador aplicado (racionamiento)
            var serieBalanceador = new Series("Balanceador")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Diamond,
                MarkerSize = 8,
                Color = Color.MediumPurple,
                ChartArea = "areaNivel",
                Legend = "legend1"
            };
            chartNivelTanque.Series.Add(serieBalanceador);

            // ==============================
            // CARGA DE PUNTOS
            // ==============================
            foreach (var r in resultados)
            {
                // Línea principal
                serieNivel.Points.AddXY(r.Dia, r.NivelFinal);

                // Evento cisterna
                if (r.EntradaLitros > 0)
                    serieCisterna.Points.AddXY(r.Dia, r.NivelFinal);

                // Evento escasez
                if (r.HuboEscasez)
                    serieEscasez.Points.AddXY(r.Dia, r.NivelFinal);

                // Evento reforzador
                if (r.ReforzadorAplicado)
                    serieReforzador.Points.AddXY(r.Dia, r.NivelFinal);

                // Evento balanceador
                if (r.BalanceadorAplicado)
                    serieBalanceador.Points.AddXY(r.Dia, r.NivelFinal);
            }

            // Título
            chartNivelTanque.Titles.Add("Nivel del Tanque vs Tiempo (con eventos sistémicos)");
        }

        public EscenarioInputDto ObtenerInputs()
        {
            return new EscenarioInputDto
            {
                NivelInicial = NivelInicial,
                CapacidadTanque = CapacidadTanque,
                ConsumoBase = ConsumoBase,
                FrecuenciaCisterna = FrecuenciaCisterna,
                VolumenCisterna = VolumenCisterna,
                DuracionDias = DuracionDias,
                ReforzadorActivo = ReforzadorActivo,
                BalanceadorActivo = BalanceadorActivo,
                UmbralReforzadorPct = UmbralReforzadorPct,
                UmbralBalanceadorPct = UmbralBalanceadorPct,
                IncrementoConsumoPorAcaparamientoPct = IncrementoConsumoPorAcaparamientoPct,
                ReduccionConsumoPorRacionamientoPct = ReduccionConsumoPorRacionamientoPct
            };
        }

        public void CargarInputs(EscenarioInputDto inputs)
        {
            numericNivelInicial.Value = inputs.NivelInicial;
            numericCapacidad.Value = inputs.CapacidadTanque;
            numericConsumoBase.Value = inputs.ConsumoBase;
            numericFrecuencia.Value = inputs.FrecuenciaCisterna;
            numericVolumenCisterna.Value = inputs.VolumenCisterna;
            numericDuracionDias.Value = inputs.DuracionDias;

            chkReforzador.Checked = inputs.ReforzadorActivo;
            chkBalanceador.Checked = inputs.BalanceadorActivo;

            numericUmbralReforzadorPct.Value = inputs.UmbralReforzadorPct;
            numericUmbralBalanceadorPct.Value = inputs.UmbralBalanceadorPct;

            numericIncrementoAcaparamiento.Value = inputs.IncrementoConsumoPorAcaparamientoPct;
            numericReduccionRacionamiento.Value = inputs.ReduccionConsumoPorRacionamientoPct;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            presenter.ImportarInputs();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            presenter.ExportarInputs();
        }
    }
}
