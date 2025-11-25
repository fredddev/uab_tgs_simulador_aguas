using System;
using System.Collections.Generic;
using System.Linq;
using GestionAguaPeriurbanos.Model;
using GestionAguaPeriurbanos.View;

namespace GestionAguaPeriurbanos.Presenter
{
    public class ResultadoPresenter
    {
        private readonly IResultadoView _view;

        public ResultadoPresenter(IResultadoView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Present(IEnumerable<ResultadoDia> resultados)
        {
            if (resultados == null) throw new ArgumentNullException(nameof(resultados));

            var list = resultados.ToList();
            var summary = new ResultadoSummary
            {
                Days = list.Count,
                TotalInflow = list.Sum(r => r.Inflow),
                TotalConsumption = list.Sum(r => r.Consumption),
                TotalOverflow = list.Sum(r => r.Overflow),
                TotalShortage = list.Sum(r => r.Shortage),
                AverageEndVolume = list.Count == 0 ? 0 : list.Average(r => r.EndVolume)
            };

            _view.ShowSummary(summary);
            _view.ShowFullResults(list);
        }
    }

    public class ResultadoSummary
    {
        public int Days { get; set; }
        public double TotalInflow { get; set; }
        public double TotalConsumption { get; set; }
        public double TotalOverflow { get; set; }
        public double TotalShortage { get; set; }
        public double AverageEndVolume { get; set; }
    }
}