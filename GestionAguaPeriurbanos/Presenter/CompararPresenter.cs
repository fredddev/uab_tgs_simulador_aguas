using System;
using System.Collections.Generic;
using System.Linq;
using GestionAguaPeriurbanos.Model;
using GestionAguaPeriurbanos.View;

namespace GestionAguaPeriurbanos.Presenter
{
    public class CompararPresenter
    {
        private readonly ICompararView _view;

        public CompararPresenter(ICompararView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public ComparisonResult Compare(IEnumerable<ResultadoDia> a, IEnumerable<ResultadoDia> b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            var A = a.ToList();
            var B = b.ToList();

            var result = new ComparisonResult
            {
                NameA = "A",
                NameB = "B",
                DaysCompared = Math.Min(A.Count, B.Count),
                TotalOverflowDifference = A.Sum(x => x.Overflow) - B.Sum(x => x.Overflow),
                TotalShortageDifference = A.Sum(x => x.Shortage) - B.Sum(x => x.Shortage),
                AverageEndVolumeDifference = (A.Count == 0 ? 0 : A.Average(x => x.EndVolume)) - (B.Count == 0 ? 0 : B.Average(x => x.EndVolume))
            };

            _view.ShowComparison(result);
            return result;
        }
    }

    public class ComparisonResult
    {
        public string? NameA { get; set; }
        public string? NameB { get; set; }
        public int DaysCompared { get; set; }
        public double TotalOverflowDifference { get; set; }
        public double TotalShortageDifference { get; set; }
        public double AverageEndVolumeDifference { get; set; }
    }
}