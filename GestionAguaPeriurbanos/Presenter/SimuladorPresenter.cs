using System;
using System.Collections.Generic;
using GestionAguaPeriurbanos.Model;
using GestionAguaPeriurbanos.View;

namespace GestionAguaPeriurbanos.Presenter
{
    public class SimuladorPresenter
    {
        private readonly ISimuladorView _view;
        private readonly SimuladorModel _simulador;

        public SimuladorPresenter(ISimuladorView view, SimuladorModel simulador)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _simulador = simulador ?? throw new ArgumentNullException(nameof(simulador));
            _view.RequestRun += OnRequestRun;
        }

        private void OnRequestRun(object? sender, int days)
        {
            Run(days);
        }

        public void Run(int days)
        {
            if (days < 1) throw new ArgumentOutOfRangeException(nameof(days));
            var results = _simulador.Run(days);
            _view.ShowResults(results);
        }

        // Expose a convenience to run and return results programmatically
        public List<ResultadoDia> RunAndGetResults(int days)
        {
            if (days < 1) throw new ArgumentOutOfRangeException(nameof(days));
            return _simulador.Run(days);
        }
    }
}