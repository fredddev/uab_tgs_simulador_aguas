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


        public void MostrarResultados(List<ResultadoDia> resultados, int diaEscasez)
        {
            gridResultados.DataSource = null;
            gridResultados.DataSource = resultados;

            if (diaEscasez > 0)
                MessageBox.Show($"Escasez detectada en el día {diaEscasez}");
            else
                MessageBox.Show("No hubo escasez durante la simulación.");
        }

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
    }
}
